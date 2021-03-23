using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;
using WifiWeather.Models;
using WifiWeather.ViewModels;
using static Meadow.Foundation.Displays.DisplayBase;

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
            Initialize();
        }

        void Initialize()
        {
            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode3);
            var spiBus = MeadowApp.Device.CreateSpiBus(MeadowApp.Device.Pins.SCK, MeadowApp.Device.Pins.MOSI, MeadowApp.Device.Pins.MISO, config);

            display = new St7789
            (
                device: MeadowApp.Device,
                spiBus: spiBus,
                chipSelectPin: null,
                dcPin: MeadowApp.Device.Pins.D01,
                resetPin: MeadowApp.Device.Pins.D00,
                width: 240, height: 240,
                displayColorMode: DisplayColorMode.Format16bppRgb565
            );

            graphics = new GraphicsLibrary(display)
            {   
                CurrentFont = new Font12x20(),
                Rotation = GraphicsLibrary.RotationType._270Degrees
            };

            graphics.Clear();
        }

        public void UpdateDisplay(WeatherViewModel model)
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

            graphics.Clear();

            graphics.Stroke = 1;
            graphics.DrawRectangle(0, 0, display.Width, display.Height, Color.White, true);

            DisplayJPG(model.WeatherCode, 5, 5);

            string date = model.DateTime.ToString("MM/dd/yy"); // $"11/29/20";
            graphics.DrawText(
                x: 128,
                y: 24,
                text: date,
                color: Color.Black);

            string time = model.DateTime.AddHours(1).ToString("hh:mm"); // $"12:16 AM";
            graphics.DrawText(
                x: 116,
                y: 66,
                text: time,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string outdoor = $"Outdoor";
            graphics.DrawText(
                x: 134,
                y: 143,
                text: outdoor,
                color: Color.Black);

            string outdoorTemp = model.OutdoorTemperature.ToString("00°C");
            graphics.DrawText(
                x: 128,
                y: 178,
                text: outdoorTemp,
                color: Color.Black,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            string indoor = $"Indoor";
            graphics.DrawText(
                x: 23,
                y: 143,
                text: indoor,
                color: Color.Black);

            string indoorTemp = model.IndoorTemperature.ToString("00°C");
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
        }

        byte[] LoadResource(int weatherCode)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName;

            switch(weatherCode)
            {
                case int n when (n >= WeatherConstants.THUNDERSTORM_LIGHT_RAIN && n <= WeatherConstants.THUNDERSTORM_HEAVY_DRIZZLE):
                    resourceName = $"WifiWeather.w_storm.jpg";
                    break;
                case int n when (n >= WeatherConstants.DRIZZLE_LIGHT && n <= WeatherConstants.DRIZZLE_SHOWER):
                    resourceName = $"WifiWeather.w_drizzle.jpg";
                    break;
                case int n when (n >= WeatherConstants.RAIN_LIGHT && n <= WeatherConstants.RAIN_SHOWER_RAGGED):
                    resourceName = $"WifiWeather.w_rain.jpg";
                    break;
                case int n when (n >= WeatherConstants.SNOW_LIGHT && n <= WeatherConstants.SNOW_SHOWER_HEAVY):
                    resourceName = $"WifiWeather.w_snow.jpg";
                    break;                                    
                case WeatherConstants.CLOUDS_CLEAR:
                    resourceName = $"WifiWeather.w_clear.jpg";
                    break;
                case int n when (n >= WeatherConstants.CLOUDS_FEW && n <= WeatherConstants.CLOUDS_OVERCAST):
                    resourceName = $"WifiWeather.w_cloudy.jpg";
                    break;
                default:
                    resourceName = $"WifiWeather.w_misc.jpg";
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