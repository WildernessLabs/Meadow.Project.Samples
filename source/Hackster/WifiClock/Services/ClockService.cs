using System;
using System.Net.Http;
using System.Threading.Tasks;
using WifiClock.Models;

namespace WifiClock.Services
{
    public static class WeatherService
    {
        static string clockDataUri = "http://worldtimeapi.org/api/timezone/America/Vancouver/";        

        static WeatherService() { }

        public static async Task<DateTimeOffset> GetTimeAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                Console.WriteLine($"Sending request...");

                HttpResponseMessage response = await client.GetAsync($"{clockDataUri}");

                try
                {
                    Console.WriteLine($"Back, Serializing...");

                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var values = System.Text.Json.JsonSerializer.Deserialize(json, typeof(TimeEntity));
                    var reading = values as TimeEntity;

                    Console.WriteLine($"Time: {reading.Datetime.ToString("hh:mm:ss")}");
                    Console.WriteLine($"Time: {reading.Datetime.ToString("dd:MM:yyyy")}");



                    return reading.Datetime;
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");
                    return DateTimeOffset.MinValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                    return DateTimeOffset.MinValue;
                }
            }
        }
    }
}
