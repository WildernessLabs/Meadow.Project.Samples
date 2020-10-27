using Meadow;
using Meadow.Devices;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Rotary;
using System;

namespace RotaryLedBar
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        float percentage;
        x74595 shiftRegister;
        LedBarGraph ledBarGraph;
        RotaryEncoder rotaryEncoder;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

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

            rotaryEncoder = new RotaryEncoder(Device, Device.Pins.D02, Device.Pins.D03);
                //Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeRising, ResistorMode.PullUp, 0, 5),
                //Device.CreateDigitalInputPort(Device.Pins.D03, InterruptMode.EdgeRising, ResistorMode.PullUp, 0, 5));
            rotaryEncoder.Rotated += RotaryEncoderRotated;

            led.SetColor(RgbLed.Colors.Green);
        }

        void RotaryEncoderRotated(object sender, RotaryTurnedEventArgs e)
        {
            Console.WriteLine("Hey");

            if (e.Direction == RotationDirection.Clockwise)
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