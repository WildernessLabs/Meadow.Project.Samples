using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Hardware;
using Meadow.Units;
using MeadowAzureIoTHub.Azure;
using MeadowAzureIoTHub.Views;
using System;
using System.Threading.Tasks;

namespace MeadowAzureIoTHub
{
    // Change F7MicroV2 to F7Micro for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;
        IotHubManager iotHubManager;
        Htu21d atmosphericSensor;

        public override Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                Device.Pins.OnboardLedRed,
                Device.Pins.OnboardLedGreen,
                Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            iotHubManager = new IotHubManager();

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            wifi.NetworkConnected += NetworkConnected;

            var config = new SpiClockConfiguration(
            speed: new Frequency(48000, Frequency.UnitType.Kilohertz),
            mode: SpiClockConfiguration.Mode.Mode3);
            var spiBus = Device.CreateSpiBus(
                clock: Device.Pins.SCK,
                copi: Device.Pins.MOSI,
                cipo: Device.Pins.MISO,
                config: config);
            var display = new St7789
            (
                spiBus: spiBus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 240, height: 240
            );

            DisplayController.Instance.Initialize(display);
            DisplayController.Instance.ShowSplashScreen();
            DisplayController.Instance.ShowConnectingAnimation();

            atmosphericSensor = new Htu21d(Device.CreateI2cBus());
            atmosphericSensor.Updated += AtmosphericSensorUpdated;

            return Task.CompletedTask;
        }

        private async void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            DisplayController.Instance.ShowConnected();

            await iotHubManager.Initialize();

            atmosphericSensor.StartUpdating(TimeSpan.FromSeconds(15));

            onboardLed.SetColor(Color.Green);
        }

        private async void AtmosphericSensorUpdated(object sender, IChangeResult<(Temperature? Temperature, RelativeHumidity? Humidity)> e)
        {
            await iotHubManager.SendEnvironmentalReading(e.New);
            await DisplayController.Instance.StartSyncCompletedAnimation(e.New);
        }
    }
}