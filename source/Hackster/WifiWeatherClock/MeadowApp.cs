using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading.Tasks;
using WifiWeatherClock.Services;
using WifiWeatherClock.ViewModels;
using WifiWeatherClock.Views;

namespace WifiWeatherClock
{
    // public class MeadowApp : App<F7Micro, MeadowApp> <- If you have a Meadow F7v1.*
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        RgbPwmLed onboardLed;
        DisplayView displayView;
        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            Device.WiFiAdapter.WiFiConnected += WiFiConnected;
        }

        void WiFiConnected(object sender, EventArgs e)
        {
            Initialize();

            GetTemperature().Wait();

            Start().Wait();
        }

        void Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);
            onboardLed.SetColor(Color.Red);

            onboardLed.SetColor(Color.Blue);

            analogTemperature = new AnalogTemperature(Device, Device.Pins.A00,
                sensorType: AnalogTemperature.KnownSensorType.LM35);
            analogTemperature.StartUpdating(TimeSpan.FromMinutes(5));

            displayView = new DisplayView();

            onboardLed.SetColor(Color.Green);
        }

        async Task GetTemperature()
        {
            onboardLed.StartPulse(Color.Orange);

            // Get indoor conditions
            var roomTemperature = await analogTemperature.Read();

            // Get outdoor conditions
            var outdoorConditions = await WeatherService.GetWeatherForecast();

            // Format indoor/outdoor conditions data
            var model = new WeatherViewModel(outdoorConditions, roomTemperature);

            // Update Temperature values and weather description
            displayView.WriteLine($"In: {model.IndoorTemperature.ToString("00")}C | Out: {model.OutdoorTemperature.ToString("00")}C", 2);
            displayView.WriteLine($"{model.Weather}", 3);

            onboardLed.StartPulse(Color.Green);
        }

        async Task Start() 
        {
            while (true) 
            {
                var datetime = DateTime.Now.AddHours(-8);

                if (datetime.Minute == 0 && datetime.Second == 0)
                {
                    await GetTemperature();
                }

                displayView.WriteLine($"{datetime.ToString("ddd, MMM dd, yyyy")}", 0);
                displayView.WriteLine($"{datetime.ToString("hh:mm:ss tt")}", 1);
                await Task.Delay(1000);
            }
        }
    }
}