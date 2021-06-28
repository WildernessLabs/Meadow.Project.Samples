using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Gateway.WiFi;
using System;
using System.Threading.Tasks;
using WifiWeatherClock.Services;
using WifiWeatherClock.ViewModels;
using WifiWeatherClock.Views;

namespace WifiWeatherClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;
        DisplayView displayView;
        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            Initialize().Wait();

            GetData().Wait();

            Start().Wait();
        }

        async Task Initialize()
        {
            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            analogTemperature = new AnalogTemperature(Device, Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35);

            displayView = new DisplayView();

            Device.InitWiFiAdapter().Wait();

            onboardLed.SetColor(Color.Blue);

            var result = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }

            onboardLed.SetColor(Color.Green);
        }

        async Task GetData() 
        {
            await GetTime();
            await GetTemperature();
        }

        async Task GetTime() 
        {
            onboardLed.StartPulse(Color.Magenta);

            var dateTime = await DateTimeService.GetDateTime();

            Device.SetClock(new DateTime(
                year: dateTime.Year,
                month: dateTime.Month,
                day: dateTime.Day,
                hour: dateTime.Hour,
                minute: dateTime.Minute,
                second: dateTime.Second));

            await GetTemperature();

            onboardLed.StartPulse(Color.Green);
        }

        async Task GetTemperature()
        {
            onboardLed.StartPulse(Color.Orange);

            displayView.ClearLine(2);
            displayView.WriteLine($"{DateTime.Now.ToString("Temperature...")}", 2);
            displayView.ClearLine(3);
            displayView.WriteLine($"{DateTime.Now.ToString("Weather...")}", 3);

            // Get indoor conditions
            var roomTemperature = await analogTemperature.Read();

            // Get outdoor conditions
            var outdoorConditions = await WeatherService.GetWeatherForecast();

            // Format indoor/outdoor conditions data
            var model = new WeatherViewModel(outdoorConditions, roomTemperature);

            // Send formatted data to display to render
            displayView.UpdateDisplay(model);

            onboardLed.StartPulse(Color.Green);
        }

        async Task Start() 
        {
            while (true) 
            {
                var datetime = DateTime.Now;

                if (datetime.Minute == 0 && datetime.Second == 0)
                {
                    await GetTemperature();
                }

                displayView.WriteLine($"{DateTime.Now.ToString("ddd, MMM dd, yyyy")}", 0);
                displayView.WriteLine($"{DateTime.Now.ToString("hh:mm:ss tt")}", 1);
                await Task.Delay(1000);
            }
        }
    }
}