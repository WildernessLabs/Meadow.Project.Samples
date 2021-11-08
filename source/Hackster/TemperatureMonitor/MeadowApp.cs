using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using System;

namespace TemperatureMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Color[] colors = new Color[4] 
        { 
            Color.FromHex("#008500"), 
            Color.FromHex("#269926"), 
            Color.FromHex("#00CC00"), 
            Color.FromHex("#67E667") 
        };

        St7789 st7789;
        GraphicsLibrary graphics;
        AnalogTemperature analogTemperature;               
        int displayWidth, displayHeight;

        public MeadowApp()
        {
            var led = new RgbLed(Device, Device.Pins.OnboardLedRed, Device.Pins.OnboardLedGreen, Device.Pins.OnboardLedBlue);
            led.SetColor(RgbLed.Colors.Red);

            analogTemperature = new AnalogTemperature(
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );
            analogTemperature.TemperatureUpdated += AnalogTemperatureTemperatureUpdated; //+= AnalogTemperatureUpdated;

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
            displayWidth = Convert.ToInt32(st7789.Width);
            displayHeight = Convert.ToInt32(st7789.Height);

            graphics = new GraphicsLibrary(st7789);
            graphics.Rotation = RotationType._270Degrees;

            led.SetColor(RgbLed.Colors.Green);

            LoadScreen();
            analogTemperature.StartUpdating(TimeSpan.FromSeconds(5));
        }

        void AnalogTemperatureTemperatureUpdated(object sender, IChangeResult<Meadow.Units.Temperature> e)
        {
            graphics.DrawRectangle(
                x: 48,
                y: 160,
                width: 144,
                height: 40,
                color: colors[colors.Length - 1],
                filled: true);

            graphics.DrawText(
                x: 48, y: 160,
                text: $"{e.New.Celsius:00.0}°C",
                color: Color.White,
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }

        void LoadScreen()
        {
            Console.WriteLine("LoadScreen...");

            graphics.Clear();

            int radius = 225;
            int originX = displayWidth / 2;
            int originY = displayHeight / 2 + 130;

            graphics.Stroke = 3;
            for (int i = 1; i < 5; i++)
            {
                graphics.DrawCircle
                (
                    centerX: originX,
                    centerY: originY,
                    radius: radius,
                    color: colors[i - 1],
                    filled: true
                );

                graphics.Show();
                radius -= 20;
            }

            graphics.DrawLine(0, 220, 240, 220, Color.White);
            graphics.DrawLine(0, 230, 240, 230, Color.White);

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText(54, 130, "TEMPERATURE", Color.White);

            graphics.Show();
        }
    }
}