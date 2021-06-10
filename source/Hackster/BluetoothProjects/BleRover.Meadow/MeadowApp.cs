using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Gateways.Bluetooth;
using System;
using System.Threading;

namespace BleRover.Meadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbLed led;
        PwmLed up, down, left, right;
        CarController carController;

        public MeadowApp()
        {
            Initialize();

            InitializeBluetooth();

            //TestCar();
        }

        void Initialize() 
        {
            led = new RgbLed(
                    Device,
                    Device.Pins.OnboardLedRed,
                    Device.Pins.OnboardLedGreen,
                    Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            up = new PwmLed(Device, Device.Pins.D13, TypicalForwardVoltage.Red);
            down = new PwmLed(Device, Device.Pins.D10, TypicalForwardVoltage.Red);
            left = new PwmLed(Device, Device.Pins.D11, TypicalForwardVoltage.Red);
            right = new PwmLed(Device, Device.Pins.D12, TypicalForwardVoltage.Red);
            up.IsOn = down.IsOn = left.IsOn = right.IsOn = false;

            var motorLeft = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D05),
                a2Pin: Device.CreatePwmPort(Device.Pins.D06),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D07)
            );
            var motorRight = new HBridgeMotor
            (
                a1Pin: Device.CreatePwmPort(Device.Pins.D02),
                a2Pin: Device.CreatePwmPort(Device.Pins.D03),
                enablePin: Device.CreateDigitalOutputPort(Device.Pins.D04)
            );

            carController = new CarController(motorLeft, motorRight);

            led.SetColor(RgbLed.Colors.Yellow);
        }

        void InitializeBluetooth() 
        {
            Definition bleTreeDefinition;
            CharacteristicBool onOffCharacteristic;

            led.SetColor(RgbLed.Colors.Blue);

            Device.InitCoprocessor();

            onOffCharacteristic = new CharacteristicBool(
                "On_Off",
                Guid.NewGuid().ToString(),
                CharacteristicPermission.Read | CharacteristicPermission.Write,
                CharacteristicProperty.Read | CharacteristicProperty.Write);

            bleTreeDefinition = new Definition(
                "Meadow Rover",
                new Service(
                    "Service",
                    253,
                    onOffCharacteristic,
                    new CharacteristicBool(
                        "Up",
                            uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    new CharacteristicBool(
                        "Down",
                            uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    new CharacteristicBool(
                        "Left",
                            uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    new CharacteristicBool(
                        "Right",
                            uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300dd",
                            permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                            properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        )
                    )
                );

            Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);

            foreach (var characteristic in bleTreeDefinition.Services[0].Characteristics)
            {
                characteristic.ValueSet += (c, d) => 
                {
                    switch (c.Name) 
                    {
                        case "Up": 
                            up.IsOn = (bool)d;
                            break;

                        case "Down":
                            down.IsOn = (bool)d;
                            break;

                        case "Left":
                            left.IsOn = (bool)d;
                            break;

                        case "Right":
                            right.IsOn = (bool)d;
                            break;
                    }             
                };
            }

            led.SetColor(RgbLed.Colors.Green);
        }

        void TestCar()
        {
            while (true)
            {
                up.SetBrightness(0.1f);
                carController.MoveForward();
                Thread.Sleep(1000);
                up.SetBrightness(0.0f);

                carController.Stop();
                Thread.Sleep(500);

                down.SetBrightness(0.1f);
                carController.MoveBackward();
                Thread.Sleep(1000);
                down.SetBrightness(0.0f);

                carController.Stop();
                Thread.Sleep(500);

                left.SetBrightness(0.1f);
                carController.TurnLeft();
                Thread.Sleep(1000);
                left.SetBrightness(0.0f);

                carController.Stop();
                Thread.Sleep(500);

                right.SetBrightness(0.1f);
                carController.TurnRight();
                Thread.Sleep(1000);
                right.SetBrightness(0.0f);

                carController.Stop();
                Thread.Sleep(500);
            }
        }
    }
}