using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeatherClock.Models;

namespace WifiWeatherClock.Services
{
    public static class WeatherService
    {
        static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";
        static string city = $"q=[CITY]";
        static string apiKey = $"appid=[API KEY]";

        static WeatherService() { }

        public static async Task<WeatherReadingModel> GetWeatherForecast()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync($"{climateDataUri}?{city}&{apiKey}");

                try
                {
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var values = System.Text.Json.JsonSerializer.Deserialize(json, typeof(WeatherReadingModel));
                    var reading = values as WeatherReadingModel;
                    return reading;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                    return null;
                }
            }
        }
    }
}
