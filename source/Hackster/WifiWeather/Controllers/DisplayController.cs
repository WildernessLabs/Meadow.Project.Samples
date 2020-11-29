using System;
using System.IO;
using System.Reflection;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Atmospheric;
using SimpleJpegDecoder;
using WifiWeather.Models;

namespace WifiWeather.Controllers
{
    public class DisplayController
    {
        protected St7789 display;
        protected GraphicsLibrary graphics;
        protected WeatherReading weatherReading;

        protected bool isRendering = false;
        protected object renderLock = new object();

        public DisplayController()
        {
            InitializeDisplay();
        }

        /// <summary>
        /// intializes the physical display peripheral, as well as the backing
        /// graphics library.
        /// </summary>
        protected void InitializeDisplay()
        {
            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);

            display = new St7789
            (
                device: MeadowApp.Device,
                spiBus: MeadowApp.Device.CreateSpiBus(MeadowApp.Device.Pins.SCK, MeadowApp.Device.Pins.MOSI, MeadowApp.Device.Pins.MISO, config),
                chipSelectPin: null,
                dcPin: MeadowApp.Device.Pins.D01,
                resetPin: MeadowApp.Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(display)
            {   
                CurrentFont = new Font12x20(),
                Rotation = GraphicsLibrary.RotationType._270Degrees
            };

            Console.WriteLine("Clear display");

            graphics.Clear(true);

            Render();
        }

        public void UpdateDisplay(WeatherReading reading)
        {
            weatherReading = reading;
            Render();
        }

        /// <summary>
        /// Does the actual rendering. If it's already rendering, it'll bail out,
        /// so render requests don't stack up.
        /// </summary>
        protected void Render()
        {
            Console.WriteLine($"Render() - is rendering: {isRendering}");

            lock (renderLock)
            {
                if (isRendering)
                {
                    Console.WriteLine("Already in a rendering loop, bailing out.");
                    return;
                }

                isRendering = true;
            }

            graphics.Clear(true);

            graphics.Stroke = 1;
            graphics.DrawRectangle(0, 0, (int)display.Width, (int)display.Height, Color.White, true);
            //graphics.DrawRectangle(5, 5, (int)display.Width - 10, (int)display.Height - 10, Color.White);

            //graphics.DrawCircle((int)display.Width / 2, (int)display.Height / 2, (int)(display.Width / 2) - 10, Color.FromHex("#23abe3"), true);

            DisplayJPG(5,5);

            string date = $"11/29/20";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 111,
                y: 20,
                text: date,
                color: Color.Black);

            string time = $"12:16 AM"; 
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 111,
                y: 50,
                text: time,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string outdoor = $"Outdoor:";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 5,
                y: 138,
                text: outdoor,
                color: Color.Black);

            string outdoorTemp = $"07°C";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 130,
                y: 128,
                text: outdoorTemp,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string indoor = $"Indoor:";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 5,
                y: 197,
                text: indoor,
                color: Color.Black);

            string indoorTemp = $"22°C"; 
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 130,
                y: 187,
                text: indoorTemp,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();

            Console.WriteLine("Show complete");

            isRendering = false;

        }

        protected void DisplayJPG(int xOffset, int yOffset)
        {
            Console.WriteLine("DisplayJPG - BEGIN");

            var jpgData = LoadResource("w_rain.jpg");
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

                graphics.DrawPixel(x + xOffset, y + yOffset, Color.FromRgb(r, g, b));

                x++;
                if (x % decoder.Width == 0)
                {
                    y++;
                    x = 0;
                }
            }

            display.Show();
            Console.WriteLine("DisplayJPG - END");
        }

        protected byte[] LoadResource(string filename)
        {
            Console.WriteLine("LoadResource - END");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"WifiWeather.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }

            Console.WriteLine("LoadResource - END");
        }
    }
}
