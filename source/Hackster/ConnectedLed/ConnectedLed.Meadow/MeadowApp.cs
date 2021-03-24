using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectedLed.Meadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        MapleServer mapleServer;
        RgbPwmLed onboardLed;

        public MeadowApp()
        {
            Initialize().Wait();
            
        }

        async Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f);
            onboardLed.SetColor(Color.Red);

            // initialize the wifi adpater
            if (!Device.InitWiFiAdapter().Result)
            {
                throw new Exception("Could not initialize the WiFi adapter.");
            }

            // connnect to the wifi network.
            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");
            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }
            Console.WriteLine($"Connected. IP: {Device.WiFiAdapter.IpAddress}");

            // create our maple web server
            mapleServer = new MapleServer(Device.WiFiAdapter.IpAddress, 5417, true);
            mapleServer.Start();

            onboardLed.SetColor(Color.Green);
        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");

            while (true)
            {
                ShowColorPulse(Color.Blue, duration);
                ShowColorPulse(Color.Cyan, duration);
                ShowColorPulse(Color.Green, duration);
                ShowColorPulse(Color.GreenYellow, duration);
                ShowColorPulse(Color.Yellow, duration);
                ShowColorPulse(Color.Orange, duration);
                ShowColorPulse(Color.OrangeRed, duration);
                ShowColorPulse(Color.Red, duration);
                ShowColorPulse(Color.MediumVioletRed, duration);
                ShowColorPulse(Color.Purple, duration);
                ShowColorPulse(Color.Magenta, duration);
                ShowColorPulse(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            onboardLed.StartPulse(color, (duration / 2));
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}
