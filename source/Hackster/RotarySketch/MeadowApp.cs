using System;
using System.Drawing;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Rotary;
using Meadow.Hardware;

namespace RotarySketch
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        int X = 0;
        int Y = 0;
        St7789 st7789;
        GraphicsLibrary graphics;
        RotaryEncoder rotaryHorizontal;
        RotaryEncoder rotaryVertical;

        public MeadowApp() {
            Console.Write("Initializing...");

            var config = new SpiClockConfiguration(
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3);
            st7789 = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );
            graphics = new GraphicsLibrary(st7789);
            //graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            IDigitalInputPort horizontalPinA = Device.CreateDigitalInputPort(Device.Pins.D13, InterruptMode.EdgeBoth, ResistorMode.PullUp, 50, 10);
            IDigitalInputPort horizontalPinB = Device.CreateDigitalInputPort(Device.Pins.D14, InterruptMode.EdgeBoth, ResistorMode.PullUp, 50, 10);
            rotaryHorizontal = new RotaryEncoder(horizontalPinA, horizontalPinB);
            rotaryHorizontal.Rotated += (s, e) => {
                if (e.Direction == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
                { X++; }
                else
                { X--; }

                Console.WriteLine("X = {0}", X);
            };

            //IDigitalInputPort verticalPinA = Device.CreateDigitalInputPort(Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.PullUp, 200, 50);
            //IDigitalInputPort verticalPinB = Device.CreateDigitalInputPort(Device.Pins.D03, InterruptMode.EdgeBoth, ResistorMode.PullUp, 200, 50);
            //rotaryVertical = new RotaryEncoder(verticalPinA, verticalPinB);
            //rotaryVertical.Rotated += (s, e) => {
            //    if (e.Direction == Meadow.Peripherals.Sensors.Rotary.RotationDirection.Clockwise)
            //    { Y++; }
            //    else
            //    { Y--; }

            //    Console.WriteLine("Y = {0}", Y);
            //};

            Console.WriteLine("done");

            LoadScreen();
        }

        void LoadScreen() {
            Console.WriteLine("LoadScreen...");

            graphics.Clear();

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(10, 10, "Hello!", Meadow.Foundation.Color.Red);

            graphics.Show();
        }
    }
}