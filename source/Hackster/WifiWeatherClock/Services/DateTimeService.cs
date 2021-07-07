using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WifiWeatherClock.Models;

namespace WifiWeatherClock.Services
{
    public static class DateTimeService
    {
        static string clockDataUri = "http://worldtimeapi.org/api/timezone/America/[CITY]/";

        static DateTimeService() { }

        public static async Task<DateTimeOffset> GetDateTime()
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
                    var values = JsonConvert.DeserializeObject<DateTimeModel>(json);                    
                    stopwatch.Stop();

                    return values.Datetime.Add(stopwatch.Elapsed);
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