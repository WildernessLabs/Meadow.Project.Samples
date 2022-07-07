using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateway.WiFi;
using MeadowMapleTemperature.Controller;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature
{
    // public class MeadowApp : App<F7FeatherV1, MeadowApp> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        MapleServer mapleServer;

        public override async Task Initialize()
        {
            LedController.Instance.SetColor(Color.Red);

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            await DateTimeService.GetTimeAsync();

            TemperatureController.Instance.Initialize();

            mapleServer = new MapleServer(Device.WiFiAdapter.IpAddress, 5417, true);
            mapleServer.Start();

            LedController.Instance.SetColor(Color.Green);
        }
    }
}