using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeadowMapleTemperature
{
    public static class DateTimeService
    {
        static string City = "Vancouver";
        static string clockDataUri = $"http://worldtimeapi.org/api/timezone/America/{City}/";

        static DateTimeService() { }

        public static async Task GetTimeAsync()
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
                    var values = JsonSerializer.Deserialize<CommonEntities>(json);

                    stopwatch.Stop();

                    var dateTime = values.datetime.Add(stopwatch.Elapsed);
                    MeadowApp.Device.SetClock(new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        dateTime.Hour,
                        dateTime.Minute,
                        dateTime.Second));
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request timed out.");                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }
    }

    public class CommonEntities
    {
        public string abbreviation { get; set; }
        public string client_ip { get; set; }
        public DateTimeOffset datetime { get; set; }
        public long day_of_week { get; set; }
        public long day_of_year { get; set; }
        public bool dst { get; set; }
        public object dst_from { get; set; }
        public long dst_offset { get; set; }
        public object dst_until { get; set; }
        public long raw_offset { get; set; }
        public string timezone { get; set; }
        public long unixtime { get; set; }
        public DateTimeOffset utc_datetime { get; set; }
        public string utc_offset { get; set; }
        public long week_number { get; set; }
    }
}