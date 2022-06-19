using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using System.Threading;

namespace MeadowLedRover
{
    // public class MeadowApp : App<F7FeatherV1, MeadowApp> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        PwmLed up, down, left, right;
        CarController carController;

        public MeadowApp()
        {
            var led = new RgbLed(
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
                device: Device,
                a1Pin: Device.Pins.D05,
                a2Pin: Device.Pins.D06,
                enablePin: Device.Pins.D07
            );
            var motorRight = new HBridgeMotor
            (
                device: Device,
                a1Pin: Device.Pins.D02,
                a2Pin: Device.Pins.D03,
                enablePin: Device.Pins.D04
            );

            carController = new CarController(motorLeft, motorRight);

            led.SetColor(RgbLed.Colors.Green);

            TestCar();
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