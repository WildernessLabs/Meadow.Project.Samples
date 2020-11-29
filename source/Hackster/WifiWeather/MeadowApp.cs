using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Gateway.WiFi;
using System;
using Meadow.Foundation;
using System.Threading.Tasks;
using WifiWeather.Controllers;
using WifiWeather.Models;
using WifiWeather.ServiceAccessLayer;

namespace WifiWeather
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        WeatherReading reading;
        DisplayController displayController;

        public MeadowApp()
        {
            Initialize();

            Start();
        }

        void Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            onboardLed.StartPulse(Color.Red);

            displayController = new DisplayController();

            Device.InitWiFiAdapter().Wait();

            onboardLed.StartPulse(Color.Blue);

            var result = Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.StartPulse(Color.Green);
        }

        async Task Start()
        {
            onboardLed.StartPulse(Color.Magenta);

            WeatherReading reading = await ClientServiceFacade.FetchReadings();
            //displayController.UpdateDisplay(reading);

            onboardLed.StartPulse(Color.Green);
        }
    }
}
