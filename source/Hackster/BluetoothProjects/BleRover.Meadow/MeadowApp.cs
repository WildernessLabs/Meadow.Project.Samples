using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Gateways.Bluetooth;

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

            led.SetColor(RgbLed.Colors.Blue);

            Device.InitCoprocessor();

            bleTreeDefinition = new Definition(
                "Meadow Rover",
                new Service(
                    "Service",
                    253,
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
                    if (!(bool)d)
                    {
                        up.IsOn = down.IsOn = left.IsOn = right.IsOn = false;
                        carController.Stop();
                    }
                    else
                    {
                        switch (c.Name)
                        {
                            case "Up":
                                up.IsOn = true;
                                carController.MoveForward();
                                break;
                            case "Down":
                                down.IsOn = true;
                                carController.MoveBackward();
                                break;
                            case "Left":
                                left.IsOn = true;
                                carController.TurnLeft();
                                break;
                            case "Right":
                                right.IsOn = true;
                                carController.TurnRight();
                                break;
                        }
                    }
                };
            }

            led.SetColor(RgbLed.Colors.Green);
        }
    }
}