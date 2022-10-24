using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using MeadowMapleLed.Controller;
using System;
using System.Threading.Tasks;

namespace MeadowMapleLed
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        MapleServer mapleServer;

        public override async Task Initialize()
        {
            LedController.Current.Initialize();
            LedController.Current.SetColor(Color.Red);

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            var connectionResult = await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            mapleServer = new MapleServer(wifi.IpAddress, 5417, true);
            mapleServer.Start();

            LedController.Current.SetColor(Color.Green);
        }
    }
}