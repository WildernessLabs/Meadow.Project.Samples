using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using WifiWeather.ViewModels;

namespace WifiWeather.Views
{
    public class WeatherView
    {
        St7789 display;
        GraphicsLibrary graphics;

        bool isRendering = false;
        object renderLock = new object();

        public WeatherView()
        {
            InitializeDisplay();
        }

        void InitializeDisplay()
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

            graphics.Clear(true);
        }

        public void UpdateDisplay(WeatherViewModel model)
        {
            Render(model);
        }

        void Render(WeatherViewModel model)
        {
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

            DisplayJPG(model.WeatherCode, 5, 5);

            string date = model.DateTime.ToString("MM/dd/yy"); // $"11/29/20";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 128,
                y: 24,
                text: date,
                color: Color.Black);

            string time = model.DateTime.ToString("hh:mm"); // $"12:16 AM"; 
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 116,
                y: 66,
                text: time,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string outdoor = $"Outdoor";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 134,
                y: 143,
                text: outdoor,
                color: Color.Black);

            string outdoorTemp = model.OutdoorTemperature.ToString("00°C");
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 128,
                y: 178,
                text: outdoorTemp,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string indoor = $"Indoor";
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 23,
                y: 143,
                text: indoor,
                color: Color.Black);

            string indoorTemp = model.IndoorTemperature.ToString("00°C");
            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(
                x: 11,
                y: 178,
                text: indoorTemp,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();

            isRendering = false;
        }

        void DisplayJPG(int weatherCode, int xOffset, int yOffset)
        {
            var jpgData = LoadResource(weatherCode);
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
        }

        byte[] LoadResource(int weatherCode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = string.Empty;

            switch(weatherCode)
            {
                case int n when (n >= 200 && n <= 299):
                    resourceName = $"WifiWeather.w_storm.jpg";
                    break;
                case int n when (n >= 300 && n <= 399):
                    resourceName = $"WifiWeather.w_drizzle.jpg";
                    break;
                case int n when (n >= 500 && n <= 599):
                    resourceName = $"WifiWeather.w_rain.jpg";
                    break;
                case int n when (n >= 600 && n <= 699):
                    resourceName = $"WifiWeather.w_snow.jpg";
                    break;
                case int n when (n >= 700 && n <= 799):
                    resourceName = $"WifiWeather.w_misc.jpg";
                    break;
                case int n when (n >= 800 && n <= 899):
                    resourceName = $"WifiWeather.w_cloudy.jpg";
                    break;
            }

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