using Meadow.Peripherals.Sensors.Atmospheric;
using System;
using WifiWeather.Models;

namespace WifiWeather.ViewModels
{
    public class WeatherViewModel
    {
        public DateTime DateTime { get; set; }

        public int WeatherCode { get; set; }

        public int OutdoorTemperature { get; set; }

        public int IndoorTemperature { get; set; }

        public WeatherViewModel(WeatherReading weatherReading, AtmosphericConditions indoorReadings)
        {
            DateTime = DateTimeOffset.FromUnixTimeSeconds(weatherReading.DateTime).LocalDateTime.AddHours(-8);

            WeatherCode = weatherReading.Weather[0].Id;

            OutdoorTemperature = (int) (weatherReading.WeatherValues.Temperature - 273);

            IndoorTemperature = (int) indoorReadings.Temperature;
        }
    }
}
