using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Hardware;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MotionDetector
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        St7789 display;
        GraphicsLibrary graphics;
        ParallaxPir motionSensor;     

        public MeadowApp()
        {
            var rgbLed = new RgbLed(
                Device,
                Device.Pins.OnboardLedRed,
                Device.Pins.OnboardLedGreen,
                Device.Pins.OnboardLedBlue
            );
            rgbLed.SetColor(RgbLed.Colors.Red);

            var config = new SpiClockConfiguration(
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3);
            display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(
                    Device.Pins.SCK,
                    Device.Pins.MOSI,
                    Device.Pins.MISO,
                    config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(display);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            motionSensor = new ParallaxPir(Device, Device.Pins.D08, InterruptMode.EdgeFalling, ResistorMode.Disabled, 5, 0);
            motionSensor.OnMotionStart += MotionSensorMotionStart;
            motionSensor.OnMotionEnd += MotionSensorMotionEnd;

            rgbLed.SetColor(RgbLed.Colors.Green);

            LoadScreen();
        }

        void LoadScreen()
        {
            Console.WriteLine("LoadScreen...");

            graphics.Clear(true);

            graphics.Stroke = 1;
            graphics.DrawRectangle(
                x: 0,
                y: 0,
                width: (int)display.Width,
                height: (int)display.Height,
                color: Color.White);
            graphics.DrawRectangle(
                x: 5,
                y: 5,
                width: (int)display.Width - 10,
                height: (int)display.Height - 10,
                color: Color.White);

            graphics.DrawCircle(
                centerX: (int)display.Width / 2,
                centerY: (int)display.Height / 2,
                radius: (int)(display.Width / 2) - 10,
                color: Color.FromHex("#EF7D3B"),
                filled: true);

            DisplayJPG(55, 40);

            graphics.CurrentFont = new Font8x12();
            
            string textMotion = "MOTION";
            graphics.DrawText(
                (int)(display.Width - textMotion.Length * 16) / 2, 
                140, 
                textMotion, 
                Color.Black, 
                GraphicsLibrary.ScaleFactor.X2);

            string textDetector = "DETECTOR";
            graphics.DrawText(
                (int)(display.Width - textDetector.Length * 16) / 2,
                165,
                textDetector, 
                Color.Black,
                GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }

        void DisplayJPG(int x, int y)
        {
            var jpgData = LoadResource("meadow.jpg");
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            int imageX = 0;
            int imageY = 0;
            byte r, g, b;

            for (int i = 0; i < jpg.Length; i += 3)
            {
                r = jpg[i];
                g = jpg[i + 1];
                b = jpg[i + 2];

                graphics.DrawPixel(imageX + x, imageY + y, Color.FromRgb(r, g, b));

                imageX++;
                if (imageX % decoder.Width == 0)
                {
                    imageY++;
                    imageX = 0;
                }
            }

            display.Show();
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MotionDetector.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private void MotionSensorMotionEnd(object sender)
        {
            Console.WriteLine("End");
            //onboardLed.SetColor(Color.Cyan);
        }

        private void MotionSensorMotionStart(object sender)
        {
            Console.WriteLine("Start");
            //displayonboardLed.SetColor(Color.Magenta);
            Thread.Sleep(1000);
        }
    }
}
