using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using System;
using System.Threading.Tasks;

namespace ConnectedLed.Meadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        MapleServer mapleServer;

        public MeadowApp()
        {
            Initialize().Wait();

            mapleServer.Start();
            Console.WriteLine("Maple Started!");

            LedController.Current.SetColor(Color.Green);
        }

        async Task Initialize()
        {
            Console.WriteLine("");
            Console.Write("Initialize hardware...");

            if (!Device.InitWiFiAdapter().Result)
            {
                throw new Exception("Could not initialize the WiFi adapter.");
            }

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            mapleServer = new MapleServer(Device.WiFiAdapter.IpAddress, 5417, true);

            LedController.Current.Initialize();

            Console.WriteLine("done.");
        }
    }
}