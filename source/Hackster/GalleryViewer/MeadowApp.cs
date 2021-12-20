using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Units;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;

namespace GalleryViewer
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        RgbLed led;
        Gc9a01 display;
        MicroGraphics graphics;
        PushButton buttonUp;
        PushButton buttonDown;
        int selectedIndex;
        string[] images = new string[3] { "image1.jpg", "image2.jpg", "image3.jpg" };

        public MeadowApp()
        {
            led = new RgbLed(
                device: Device, 
                redPin: Device.Pins.OnboardLedRed, 
                greenPin: Device.Pins.OnboardLedGreen, 
                bluePin: Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            buttonUp = new PushButton(Device, Device.Pins.D03);
            buttonUp.Clicked += ButtonUpClicked;

            buttonDown = new PushButton(Device, Device.Pins.D04);
            buttonDown.Clicked += ButtonDownClicked;

            var config = new SpiClockConfiguration(
                speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
                mode: SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                clock: Device.Pins.SCK,
                copi: Device.Pins.MOSI,
                cipo: Device.Pins.MISO,
                config: config);
            display = new Gc9a01
            (
                device: Device,
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            graphics = new MicroGraphics(display);
            graphics.Rotation = RotationType._90Degrees;

            DisplayJPG();

            led.SetColor(RgbLed.Colors.Green);
        }

        void ButtonUpClicked(object sender, EventArgs e)
        {
            led.SetColor(RgbLed.Colors.Red);

            if (selectedIndex + 1 > 2)
                selectedIndex = 0;
            else
                selectedIndex++;

            DisplayJPG();

            led.SetColor(RgbLed.Colors.Green);
        }

        void ButtonDownClicked(object sender, EventArgs e)
        {
            led.SetColor(RgbLed.Colors.Red);

            if (selectedIndex - 1 < 0)
                selectedIndex = 2;
            else
                selectedIndex--;

            DisplayJPG();

            led.SetColor(RgbLed.Colors.Green);
        }

        void DisplayJPG()
        {
            var jpgData = LoadResource(images[selectedIndex]);
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            int x = 0;
            int y = 0;
            byte r, g, b;

            for (int i = 0; i < jpg.Length; i += 3)
            {
                r = jpg[i];
                g = jpg[i + 1];
                b = jpg[i + 2];

                graphics.DrawPixel(x, y, Color.FromRgb(r, g, b));

                x++;
                if (x % decoder.Width == 0)
                {
                    y++;
                    x = 0;
                }
            }

            display.Show();
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"GalleryViewer.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}