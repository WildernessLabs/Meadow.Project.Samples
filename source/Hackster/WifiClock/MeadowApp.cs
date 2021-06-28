using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;
using WifiClock.Services;
using Meadow.Units;
using TU = Meadow.Units.Temperature.UnitType;

namespace WifiClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PushButton pushButton;
        Max7219 display;
        GraphicsLibrary graphics;
        AnalogTemperature analogTemperature;

        bool showDate;

        public MeadowApp()
        {
            Initialize();

            Start();
        }

        async Task Initialize()
        {
            RgbPwmLed onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            onboardLed.StartPulse(Color.Red);

            display = new Max7219(
                device: Device, 
                spiBus: Device.CreateSpiBus(), 
                csPin: Device.Pins.D01, 
                deviceCount: 4, 
                maxMode: Max7219.Max7219Type.Display);
            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._180Degrees;

            graphics.Clear();
            graphics.DrawText(0, 1,  "WI");
            graphics.DrawText(0, 9,  "FI");
            graphics.DrawText(0, 17, "TI");
            graphics.DrawText(0, 25, "ME");
            graphics.Show();

            pushButton = new PushButton(Device, Device.Pins.D04, ResistorMode.InternalPullUp);
            pushButton.Clicked += PushButtonClicked;

            analogTemperature = new AnalogTemperature(
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );            

            Device.InitWiFiAdapter().Wait();

            onboardLed.StartPulse(Color.Blue);

            var result = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                onboardLed.StartPulse(Color.Magenta);
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.StartPulse(Color.Green);
        }

        void PushButtonClicked(object sender, EventArgs e)
        {
            showDate = true;
            Thread.Sleep(5000);
            showDate = false;
        }

        async Task Start() 
        {
            var dateTime = await ClockService.GetTimeAsync();            

            Device.SetClock(new DateTime(
                year: dateTime.Year, 
                month: dateTime.Month, 
                day: dateTime.Day, 
                hour: dateTime.Hour, 
                minute: dateTime.Minute, 
                second: dateTime.Second));

            while (true)
            {
                DateTime clock = DateTime.Now;

                graphics.Clear();

                graphics.DrawText(0, 1, $"{clock:hh}");
                graphics.DrawText(0, 9, $"{clock:mm}");
                graphics.DrawText(0, 17, $"{clock:ss}");
                graphics.DrawText(0, 25, $"{clock:tt}");

                if (showDate)
                {
                    graphics.Clear();

                    graphics.DrawText(0, 1, $"{clock:dd}");
                    graphics.DrawText(0, 9, $"{clock:MM}");
                    graphics.DrawText(0, 17, $"{clock:yy}");

                    graphics.DrawHorizontalLine(0, 24, 7, true);

                    var temperature = await analogTemperature.Read();

                    graphics.DrawText(0, 26, $"{(int) temperature.Celsius}");
                }
                
                graphics.Show();
                Thread.Sleep(1000);
            }
        }
    }
}