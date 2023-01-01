using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using MeadowMapleServo.Controllers;
using System;
using System.Threading.Tasks;
using Meadow.Units;
using Meadow.Foundation.Servos;

namespace MeadowMapleServo
{
    // public class MeadowApp : App<F7FeatherV1> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7FeatherV2>
    {
        MapleServer mapleServer;

        public override async Task Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            ServoController.Instance.RotateTo(new Angle(NamedServoConfigs.SG90.MinimumAngle));

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            mapleServer = new MapleServer(wifi.IpAddress, 5417, true, logger: Resolver.Log);
            mapleServer.Start();

            onboardLed.SetColor(Color.Green);
        }
    }
}