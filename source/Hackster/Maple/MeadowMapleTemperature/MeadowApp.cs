using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using MeadowMapleTemperature.Controller;
using MeadowMapleTemperature.Services;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
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

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            await DateTimeService.GetTimeAsync();

            mapleServer = new MapleServer(Device.WiFiAdapter.IpAddress, 5417, false);

            TemperatureController.Instance.Initialize();

            onboardLed.SetColor(Color.Green);
        }
    }
}