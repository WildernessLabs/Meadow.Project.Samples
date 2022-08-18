using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Rotary;
using System;
using System.Threading.Tasks;

namespace RotaryLedBar
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        float percentage;
        x74595 shiftRegister;
        LedBarGraph ledBarGraph;
        RotaryEncoder rotaryEncoder;

        public override Task Initialize()
        {
            var onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            shiftRegister = new x74595(Device, Device.CreateSpiBus(), Device.Pins.D00, 8);
            shiftRegister.Clear();

            IDigitalOutputPort[] ports =
            {
                Device.CreateDigitalOutputPort(Device.Pins.D14),
                Device.CreateDigitalOutputPort(Device.Pins.D15),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP0, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP1, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP2, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP3, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP4, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP5, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP6, false, OutputType.PushPull),
                shiftRegister.CreateDigitalOutputPort(shiftRegister.Pins.GP7, false, OutputType.PushPull),
            };

            ledBarGraph = new LedBarGraph(ports);
            ledBarGraph.Percentage = 1;

            rotaryEncoder = new RotaryEncoder(Device, Device.Pins.D01, Device.Pins.D03);
            rotaryEncoder.Rotated += RotaryEncoderRotated;

            onboardLed.SetColor(Color.Green);

            return base.Initialize();
        }

        void RotaryEncoderRotated(object sender, RotaryChangeResult result)
        {
            Console.WriteLine("Hey");

            if (result.New == RotationDirection.Clockwise)
                percentage += 0.05f;
            else
                percentage -= 0.05f;

            if (percentage > 1f)
                percentage = 1f;
            else if (percentage < 0f)
                percentage = 0f;

            ledBarGraph.Percentage = percentage;
        }
    }
}