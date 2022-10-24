using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Motors;
using Meadow.Peripherals.Leds;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowLedRover
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        PwmLed up, down, left, right;
        CarController carController;

        public override Task Initialize()
        {
            var led = new RgbLed(
                Device, 
                Device.Pins.OnboardLedRed, 
                Device.Pins.OnboardLedGreen, 
                Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLedColors.Red);

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

            led.SetColor(RgbLedColors.Green);

            return base.Initialize();
        }

        public override async Task Run()
        {
            while (true)
            {
                up.Brightness = 0.1f;
                carController.MoveForward();
                await Task.Delay(1000);
                up.Brightness = 0.0f;

                carController.Stop();
                await Task.Delay(500);

                down.Brightness = 0.1f;
                carController.MoveBackward();
                await Task.Delay(1000);
                down.Brightness = 0.0f;

                carController.Stop();
                await Task.Delay(500);

                left.Brightness = 0.1f;
                carController.TurnLeft();
                await Task.Delay(1000);
                left.Brightness = 0.0f;

                carController.Stop();
                await Task.Delay(500);

                right.Brightness = 0.1f;
                carController.TurnRight();
                await Task.Delay(1000);
                right.Brightness = 0.0f;

                carController.Stop();
                await Task.Delay(500);
            }
        }
    }
}