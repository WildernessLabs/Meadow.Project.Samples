using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Moisture;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Hardware;
using SimpleJpegDecoder;
using System;
using System.IO;
using System.Reflection;

namespace PlantMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const float MINIMUM_VOLTAGE_CALIBRATION = 2.81f;
        const float MAXIMUM_VOLTAGE_CALIBRATION = 1.50f;

        RgbPwmLed onboardLed;
        PushButton button;
        Capacitive capacitive;
        GraphicsLibrary graphics;
        AnalogTemperature analogTemperature;

        string[] images = new string[4]
        {
            "level_0.jpg",
            "level_1.jpg",
            "level_2.jpg",
            "level_3.jpg"
        };

        int selectedImageIndex = 0;

        public MeadowApp()
        {
            Initialize();

            Start();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.SetColor(Color.Red);

            button = new PushButton(Device, Device.Pins.D04, ResistorMode.InternalPullUp);
            button.Clicked += ButtonClicked;

            var config = new SpiClockConfiguration
            (
                speedKHz: 6000,
                mode: SpiClockConfiguration.Mode.Mode3
            );
            var display = new St7789
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            graphics = new GraphicsLibrary(display);            
            graphics.CurrentFont = new Font12x20();
            graphics.Stroke = 3;

            capacitive = new Capacitive(
                device: Device,
                analogPin: Device.Pins.A01,
                minimumVoltageCalibration: MINIMUM_VOLTAGE_CALIBRATION,
                maximumVoltageCalibration: MAXIMUM_VOLTAGE_CALIBRATION);

            analogTemperature = new AnalogTemperature(Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35);

            onboardLed.SetColor(Color.Green);
        }

        async void ButtonClicked(object sender, EventArgs e)
        {
            onboardLed.SetColor(Color.Orange);

            float moisture = await capacitive.Read();

            if (moisture > 1)
                moisture = 1f;
            else
            if (moisture < 0)
                moisture = 0f;

            if (moisture > 0 && moisture <= 0.25)
                UpdateImage(0, 42, 20);
            else if (moisture > 0.25 && moisture <= 0.50)
                UpdateImage(1, 28, 14);
            else if (moisture > 0.50 && moisture <= 0.75)
                UpdateImage(2, 21, 15);
            else if (moisture > 0.75 && moisture <= 1.0)
                UpdateImage(3, 45, 15);

            graphics.DrawRectangle(0, 0, 36, 20, Color.White, true);
            graphics.DrawText(0, 0, $"{(int)(moisture * 100)}%", Color.Black);

            graphics.Show();

            onboardLed.SetColor(Color.Green);
        }

        void Start()
        {
            graphics.Clear();
           
            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);

            string plant = "Plant";
            string monitor = "Monitor";

            graphics.CurrentFont = new Font12x20();
            graphics.DrawText((240 - (plant.Length * 24))/2, 80, plant, Color.Black, GraphicsLibrary.ScaleFactor.X2);
            graphics.DrawText((240 - (monitor.Length * 24)) / 2, 130, monitor, Color.Black, GraphicsLibrary.ScaleFactor.X2);

            graphics.Show();
        }

        void UpdateImage(int index, int xOffSet, int yOffSet)
        {
            var jpgData = LoadResource(images[index]);
            var decoder = new JpegDecoder();
            var jpg = decoder.DecodeJpeg(jpgData);

            int x = 0;
            int y = 0;
            byte r, g, b;

            graphics.DrawRectangle(0, 0, 240, 240, Color.White, true);

            for (int i = 0; i < jpg.Length; i += 3)
            {
                r = jpg[i];
                g = jpg[i + 1];
                b = jpg[i + 2];

                graphics.DrawPixel(x + xOffSet, y + yOffSet, Color.FromRgb(r, g, b));

                x++;
                if (x % decoder.Width == 0)
                {
                    y++;
                    x = 0;
                }
            }

            graphics.Show();
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"PlantMonitor.{filename}";

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