using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using System;
using System.Threading.Tasks;
using WifiWeather.Views;
using WifiWeather.Services;
using WifiWeather.ViewModels;

namespace WifiWeather
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        WeatherView displayController;
        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            Initialize().Wait();

            Start().Wait();
        }

        async Task Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.StartPulse(Color.Red);

            analogTemperature = new AnalogTemperature(
                device: Device,
                analogPin: Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35
            );

            displayController = new WeatherView();

            Device.InitWiFiAdapter().Wait();

            onboardLed.StartPulse(Color.Blue);

            var result = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.StartPulse(Color.Green);
        }

        async Task Start()
        {
            onboardLed.StartPulse(Color.Magenta);

            // Get indoor conditions
            var roomTemperature = await analogTemperature.Read();

            // Get outdoor conditions
            var outdoorConditions = await WeatherService.GetWeatherForecast();

            onboardLed.StartPulse(Color.Orange);

            // Format indoor/outdoor conditions data
            var model = new WeatherViewModel(outdoorConditions, roomTemperature);

            // Send formatted data to display to render
            displayController.UpdateDisplay(model);

            onboardLed.StartPulse(Color.Green);
        }
    }
}