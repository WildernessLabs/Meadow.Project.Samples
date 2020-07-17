using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Atmospheric;
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

        AnalogTemperature analogTemperature;
        St7789 st7789;
        GraphicsLibrary graphics;
        int displayWidth, displayHeight;

        public MeadowApp()
        {
            Console.WriteLine("Initializing...");

            analogTemperature = new AnalogTemperature(
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );
            analogTemperature.Updated += AnalogTemperatureUpdated;

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
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;

            LoadScreen();
            analogTemperature.StartUpdating();
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

        void AnalogTemperatureUpdated(object sender, AtmosphericConditionChangeResult e)
        {
            float oldTemp = (float)(e.Old.Temperature / 1000);
            float newTemp = (float)(e.New.Temperature / 1000);

            graphics.DrawText(
                x: 48, y: 160, 
                text: $"{oldTemp.ToString("##.#")}°C", 
                color: colors[colors.Length - 1], 
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);
            graphics.DrawText(
                x: 48, y: 160, 
                text: $"{newTemp.ToString("##.#")}°C", 
                color: Color.White, 
                scaleFactor: GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }
    }
}