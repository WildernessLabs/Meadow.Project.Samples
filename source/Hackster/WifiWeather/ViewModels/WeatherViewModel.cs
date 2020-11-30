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

        public WeatherViewModel(WeatherReading outdoorConditions, AtmosphericConditions indoorConditions)
        {
            int TIME_ZONE = -8; // Note: Adjust time zone value here

            DateTime = DateTimeOffset.FromUnixTimeSeconds(outdoorConditions.DateTime).LocalDateTime.AddHours(TIME_ZONE);

            WeatherCode = outdoorConditions.Weather[0].Id;

            OutdoorTemperature = (int) (outdoorConditions.WeatherValues.Temperature - 273);

            IndoorTemperature = (int) indoorConditions.Temperature;
        }
    }
}
