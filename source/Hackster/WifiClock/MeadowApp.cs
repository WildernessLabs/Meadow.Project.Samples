using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Gateway.WiFi;
using System;
using System.Threading;
using System.Threading.Tasks;
using WifiClock.Services;

namespace WifiClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        Max7219 display;
        GraphicsLibrary graphics;

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

            onboardLed.StartPulse(Color.Red);

            display = new Max7219(Device, Device.CreateSpiBus(), Device.Pins.D01, 4, Max7219.Max7219Type.Display);
            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._180Degrees;

            Device.InitWiFiAdapter().Wait();

            onboardLed.StartPulse(Color.Blue);

            var result = Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                onboardLed.StartPulse(Color.Magenta);
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.StartPulse(Color.Green);
        }

        async Task Start() 
        {
            var dateTime = await WeatherService.GetTimeAsync();

            Device.SetClock(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second));

            while (true)
            {
                DateTime clock = DateTime.Now;
                graphics.Clear();
                graphics.DrawText(0, 1, $"{clock:hh}");
                graphics.DrawText(0, 9, $"{clock:mm}");
                graphics.DrawText(0, 17, $"{clock:ss}");
                graphics.DrawText(0, 25, $"{clock:tt}");
                graphics.Show();
                Thread.Sleep(1000);
            }
        }

    }
}
