using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using System;
using System.Threading;

namespace LedCar
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PwmLed up, down, left, right;
        CarController carController;

        public MeadowApp()
        {
            Console.WriteLine("MeadowApp()...");

            Initialize();
            TestCar();
        }

        protected void Initialize()
        {
            Console.WriteLine("Initialize()...");

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
        }

        protected void TestCar()
        {
            Console.WriteLine("TestCar()...");

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