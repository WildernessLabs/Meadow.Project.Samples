using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using MeadowMapleLed.Controller;
using System;
using System.Threading.Tasks;

namespace MeadowMapleLed
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7 v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        MapleServer mapleServer;

        public MeadowApp()
        {
            Initialize().Wait();

            mapleServer.Start();

            LedController.Current.SetColor(Color.Green);
        }

        async Task Initialize()
        {
            LedController.Current.Initialize();

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            mapleServer = new MapleServer(
                Device.WiFiAdapter.IpAddress, 5417, true
            );
        }
    }
}