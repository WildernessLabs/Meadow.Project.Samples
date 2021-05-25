using Meadow.Units;
using System.Globalization;
using WifiWeatherClock.Models;

namespace WifiWeatherClock.ViewModels
{
    public class WeatherViewModel
    {
        public int OutdoorTemperature { get; set; }

        public int IndoorTemperature { get; set; }

        public string Weather { get; set; }

        public WeatherViewModel(WeatherReadingModel outdoorConditions, Temperature indoorTemperature)
        {
            var textCase = new CultureInfo("en-US", false).TextInfo;
            Weather = textCase.ToTitleCase(outdoorConditions.Weather[0].Description);

            OutdoorTemperature = (int)(outdoorConditions.WeatherValues.Temperature - 273);

            IndoorTemperature = (int)indoorTemperature.Celsius;
        }
    }
}