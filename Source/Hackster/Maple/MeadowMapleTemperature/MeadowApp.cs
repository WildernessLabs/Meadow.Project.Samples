using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using MeadowMapleTemperature.Controller;
using System;
using System.Threading.Tasks;

namespace MeadowMapleTemperature
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        MapleServer mapleServer;

        public override async Task Initialize()
        {
            LedController.Instance.SetColor(Color.Red);

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            var connectionResult = await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            await DateTimeService.GetTimeAsync();

            TemperatureController.Instance.Initialize();

            mapleServer = new MapleServer(wifi.IpAddress, 5417, true);
            mapleServer.Start();

            LedController.Instance.SetColor(Color.Green);
        }
    }
}