using ConnectedServo.Meadow.Controllers;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple.Server;
using Meadow.Gateway.WiFi;
using System;
using System.Threading.Tasks;
using Meadow.Units;
using AU = Meadow.Units.Angle.UnitType;

namespace ConnectedServo.Meadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        MapleServer mapleServer;

        public MeadowApp()
        {
            Initialize().Wait();

            mapleServer.Start();

            ServoController.Current.RotateTo(new Angle(0, AU.Degrees));
        }

        async Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            ServoController.Current.Initialize(Device, Device.Pins.D03);

            if (!Device.InitWiFiAdapter().Result)
            {
                throw new Exception("Could not initialize the WiFi adapter.");
            }

            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }

            mapleServer = new MapleServer(
                Device.WiFiAdapter.IpAddress, 5417, true
            );

            onboardLed.SetColor(Color.Green);
        }
    }
}