using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeather.Models;

namespace WifiWeather.ServiceAccessLayer
{
    public static class WeatherService
    {
        static string climateDataUri = "http://api.openweathermap.org/data/2.5/weather";
        static string city = $"q=[CITY NAME HERE]";
        static string apiKey = $"appid=[API KEY HERE]";

        static WeatherService() { }

        public static async Task<WeatherReading> GetWeatherForecast()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync($"{climateDataUri}?{city}&{apiKey}");

                try
                {
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var values = System.Text.Json.JsonSerializer.Deserialize(json, typeof(WeatherReading));
                    var reading = values as WeatherReading;
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