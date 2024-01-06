using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Hardware;
using Meadow.Units;
using MeadowAzureIoTHub.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub
{
    internal class MainController
    {
        Htu21d atmosphericSensor;
        DisplayController displayController;

        IWiFiNetworkAdapter network;
        IIoTHubController iotHubController;

        public MainController(IWiFiNetworkAdapter network)
        {
            this.network = network;
        }

        public async Task Initialize()
        {
            var display = new St7789
            (
                spiBus: MeadowApp.Device.CreateSpiBus(),
                chipSelectPin: MeadowApp.Device.Pins.D02,
                dcPin: MeadowApp.Device.Pins.D01,
                resetPin: MeadowApp.Device.Pins.D00,
                width: 240, height: 240
            );

            displayController = new DisplayController(display);
            displayController.ShowSplashScreen();
            Thread.Sleep(3000);
            _ = displayController.ShowConnectingAnimation();

            //iotHubController = new IoTHubAmqpController();

            iotHubController = new IoTHubMqttController();

            await InitializeIoTHub();

            atmosphericSensor = new Htu21d(MeadowApp.Device.CreateI2cBus());
            atmosphericSensor.Updated += AtmosphericSensorUpdated;
        }

        private async Task InitializeIoTHub()
        {
            while (!iotHubController.isAuthenticated)
            {
                if (network.IsConnected)
                {
                    bool authenticated = await iotHubController.Initialize();

                    if (authenticated)
                    {
                        Resolver.Log.Info("Authenticated");
                        displayController.ShowConnected();
                    }
                    else
                    {
                        Resolver.Log.Info("Not Authenticated");
                    }
                }
                else
                {
                    Resolver.Log.Info("Offline");
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private async void AtmosphericSensorUpdated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity)> e)
        {
            await SendDataToIoTHub(e.New);
            await displayController.StartSyncCompletedAnimation(e.New);
        }

        private async Task SendDataToIoTHub((Temperature? Temperature, RelativeHumidity? Humidity) data)
        {
            if (network.IsConnected && iotHubController.isAuthenticated)
            {
                await iotHubController.SendEnvironmentalReading(data);
            }
        }

        public Task Run()
        {
            atmosphericSensor.StartUpdating(TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }
    }
}