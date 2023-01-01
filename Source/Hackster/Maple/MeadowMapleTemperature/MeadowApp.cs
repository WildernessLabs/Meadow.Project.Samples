﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Web.Maple;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using MeadowMapleTemperature.Controllers;
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

            await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            TemperatureController.Instance.Initialize();

            mapleServer = new MapleServer(wifi.IpAddress, 5417, true, logger: Resolver.Log);
            mapleServer.Start();

            LedController.Instance.SetColor(Color.Green);
        }
    }
}