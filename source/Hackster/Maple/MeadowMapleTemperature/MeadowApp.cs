using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using MeadowMapleTemperature.Models;
using MeadowMapleTemperature.Services;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        TemperatureAgent temperatureAgent;
        MapleServer mapleServer;

        public MeadowApp()
        {
            Initialize().Wait();

            GetDateTime().Wait();

            mapleServer.Start();
        }

        async Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            await GetDateTime();

            temperatureAgent = new TemperatureAgent();
            temperatureAgent.TemperatureUpdated += TemperatureAgentUpdated;

            mapleServer = new MapleServer(
                Device.WiFiAdapter.IpAddress, 5417, true
            );

            onboardLed.SetColor(Color.Green);
        }

        async Task GetDateTime()
        {
            var dateTime = await DateTimeService.GetTimeAsync();

            Device.SetClock(new DateTime(
                year: dateTime.Year,
                month: dateTime.Month,
                day: dateTime.Day,
                hour: dateTime.Hour,
                minute: dateTime.Minute,
                second: dateTime.Second));
        }

        void TemperatureAgentUpdated(object sender, TemperatureModel e)
        {
            Console.Write($"Saving ({e.Temperature.Value.Celsius},{e.DateTime.ToString()})...");
            SQLiteDatabaseManager.Instance.SaveReading(e);
            Console.WriteLine("done!");
        }
    }
}