using Json.Net;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WifiClock.Models;

namespace WifiClock.Services
{
    public static class DateTimeService
    {
        static string clockDataUri = "http://worldtimeapi.org/api/timezone/America/[CITY]/";        

        static DateTimeService() { }

        public static async Task<DateTimeOffset> GetTimeAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = new TimeSpan(0, 5, 0);

                    HttpResponseMessage response = await client.GetAsync($"{clockDataUri}");

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();
                    var values = JsonNet.Deserialize<DateTimeEntity>(json);                    

                    stopwatch.Stop();

                    return values.datetime.Add(stopwatch.Elapsed);
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