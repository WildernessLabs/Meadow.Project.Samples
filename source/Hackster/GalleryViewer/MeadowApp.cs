using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;

namespace GalleryViewer
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Led red, green;
        St7789 display;
        GraphicsLibrary graphics;
        PushButton buttonUp;
        PushButton buttonDown;
        int selectedIndex;
        string[] images = new string[3] { "image1.jpg", "image2.jpg", "image3.jpg" };

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            red = new Led(Device.CreateDigitalOutputPort(Device.Pins.OnboardLedRed));
            green = new Led(Device.CreateDigitalOutputPort(Device.Pins.OnboardLedGreen));

            buttonUp = new PushButton(Device, Device.Pins.D03);
            buttonUp.Clicked += ButtonUpClicked;

            buttonDown = new PushButton(Device, Device.Pins.D04);
            buttonDown.Clicked += ButtonDownClicked;

            var config = new SpiClockConfiguration(
                 speedKHz: 6000,
                 mode: SpiClockConfiguration.Mode.Mode3);

            display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(
                    clock: Device.Pins.SCK, 
                    mosi: Device.Pins.MOSI, 
                    miso: Device.Pins.MISO, 
                    config: config),
                chipSelectPin: null,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(display);
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            DisplayJPG();

            green.IsOn = true;
        }

        void ButtonUpClicked(object sender, EventArgs e)
        {
            red.IsOn = true;
            green.IsOn = false;

            if (selectedIndex + 1 > 2)
                selectedIndex = 0;
            else
                selectedIndex++;

            DisplayJPG();

            red.IsOn = false;
            green.IsOn = true;
        }

        void ButtonDownClicked(object sender, EventArgs e)
        {
            red.IsOn = true;
            green.IsOn = false;

            if (selectedIndex - 1 < 0)
                selectedIndex = 2;
            else
                selectedIndex--;

            DisplayJPG();

            red.IsOn = false;
            green.IsOn = true;
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