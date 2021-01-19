using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
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

            HttpResponseMessage response = await client.GetAsync($"{clockDataUri}");

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var values = JsonSerializer.Deserialize(json, typeof(TimeEntity));
                var reading = values as TimeEntity;

                stopwatch.Stop();

                return reading.Datetime.Add(stopwatch.Elapsed);
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
