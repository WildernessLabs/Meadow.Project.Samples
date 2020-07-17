using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Hardware;

namespace MF
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        St7789 display;
        GraphicsLibrary graphics;

        Bmp180 sensor;

        IDigitalInputPort button;

        bool isMetric = true;

        public MeadowApp()
        {
            Initialize();
         //   CycleColors(1000);

         //   TestDraw();
        }

        void TestDraw()
        {
            Console.WriteLine("test draw");
            display.Clear();

            for( int i = 0; i < 40; i++)
            {
                display.DrawPixel(i, i, Color.LawnGreen);
            }
            display.Show();
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

            var config = new SpiClockConfiguration(3000, SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            button = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeRising, ResistorMode.Disabled);
            button.Changed += Button_Changed;

            //display
            display = new St7789(
                device: Device,
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240);

            graphics = new GraphicsLibrary(display);

            graphics.CurrentFont = new Font12x20();

            sensor = new Bmp180(Device.CreateI2cBus());
            sensor.Updated += Sensor_Updated;
            sensor.StartUpdating();
        }

        private void Button_Changed(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine("Button pressed");
            isMetric = !isMetric;
        }

        private void Sensor_Updated(object sender, Meadow.Peripherals.Sensors.Atmospheric.AtmosphericConditionChangeResult e)
        {
            Console.WriteLine($"{e.New.Temperature}");

            UpdateDisplayFancy(e.New.Temperature.Value);

            UpdateLed(e.New.Temperature.Value);
        }

        void UpdateDisplay ()
        {
            Console.WriteLine("Update display");

            graphics.Clear();

            Console.WriteLine("Draw");
            graphics.DrawText(0, 0, $"Temperature:", Color.Blue, GraphicsLibrary.ScaleFactor.X2);
            graphics.DrawText(0, 40, $"{sensor.Conditions.Temperature}°C", Color.Blue, GraphicsLibrary.ScaleFactor.X2);


            graphics.DrawText(0, 80, $"Pressure:", Color.Green, GraphicsLibrary.ScaleFactor.X2);
            graphics.DrawText(0, 120, $"{sensor.Conditions.Pressure / 101325f}atm", Color.Green, GraphicsLibrary.ScaleFactor.X2);

            Console.WriteLine("Show");
            graphics.Show();
        }

        void UpdateDisplayFancy(float temperature)
        {
            Console.WriteLine("Update display");

            graphics.Clear();

            Console.WriteLine("Draw");

            var primaryColor = Color.FromRgb(238, 243, 189);
            var accentColor = Color.FromRgb(26, 128, 170);

            var tempText = GetTemperatureString(temperature);

            var xTempPos = graphics.Width / 2 - tempText.Length * 12;


            graphics.DrawCircle(120, 84, 80, accentColor, true);
            graphics.DrawCircle(120, 84, 80, primaryColor);
            graphics.DrawText((int)xTempPos, 70, tempText, primaryColor, GraphicsLibrary.ScaleFactor.X2);

            //draw pressure bar 
            int barWidth = (int)(220.0 * sensor.Pressure / Conversions.StandardAtmInPa);
            graphics.DrawRectangle(10, 190, barWidth, 30, accentColor, true);
            graphics.DrawRectangle(10, 190, 220, 30, primaryColor);
            graphics.DrawText(20, 196, GetPressureString(sensor.Pressure), primaryColor);

            Console.WriteLine("Show");
            graphics.Show();
        }

        string GetPressureString(float pa)
        {
            if (isMetric)
            {
                return Conversions.PaToMmHg(pa) + " mm Hg";
            }
            else
            {
                return Conversions.PaToPsi(pa) + " psi";
            }
        }

        string GetTemperatureString(float celcius)
        {
            if(isMetric)
            {
                return celcius.ToString("n1") + "°C";
            }
            else
            {
                return Conversions.CeliusToFahrenheit(celcius).ToString("n1") + "°F";
            }


        }

        double oldTemp = 0;
        void UpdateLed(double newTemp)
        {
            if(Math.Abs(newTemp - oldTemp) < 0.01)
            {
                onboardLed.SetColor(Color.Black);
            }
            else if(oldTemp > newTemp)
            {   //we're cooling
                onboardLed.SetColor(Color.Blue);
            }
            else
            {   //warming
                onboardLed.SetColor(Color.Red);
            }
            oldTemp = newTemp;
        }
    }
}
