using MeadowServerTemperature.Controllers;
using MeadowServerTemperature.Services;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using System;
using System.Threading.Tasks;

namespace MeadowServerTemperature
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        MapleServer mapleServer;

        public MeadowApp()
        {
            Initialize().Wait();

            mapleServer.Start();
        }

        async Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);            

            if (!Device.InitWiFiAdapter().Result)
            {
                throw new Exception("Could not initialize the WiFi adapter.");
            }

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            var dateTime = await DateTimeService.GetTimeAsync();

            Device.SetClock(new DateTime(
                year: dateTime.Year,
                month: dateTime.Month,
                day: dateTime.Day,
                hour: dateTime.Hour,
                minute: dateTime.Minute,
                second: dateTime.Second));

            AnalogTemperatureController.Current.Initialize(Device, Device.Pins.A00);

            mapleServer = new MapleServer(
                Device.WiFiAdapter.IpAddress, 5417, true
            );

            onboardLed.SetColor(Color.Green);
        }
    }
}