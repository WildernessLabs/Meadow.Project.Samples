using System;
using System.Text.Json.Serialization;

namespace WifiWeatherClock.Models
{
    public class DateTimeModel
    {
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonPropertyName("client_ip")]
        public string ClientIp { get; set; }

        [JsonPropertyName("datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonPropertyName("day_of_week")]
        public long DayOfWeek { get; set; }

        [JsonPropertyName("day_of_year")]
        public long DayOfYear { get; set; }

        [JsonPropertyName("dst")]
        public bool Dst { get; set; }

        [JsonPropertyName("dst_from")]
        public object DstFrom { get; set; }

        [JsonPropertyName("dst_offset")]
        public long DstOffset { get; set; }

        [JsonPropertyName("dst_until")]
        public object DstUntil { get; set; }

        [JsonPropertyName("raw_offset")]
        public long RawOffset { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("unixtime")]
        public long Unixtime { get; set; }

        [JsonPropertyName("utc_datetime")]
        public DateTimeOffset UtcDatetime { get; set; }

        [JsonPropertyName("utc_offset")]
        public string UtcOffset { get; set; }

        [JsonPropertyName("week_number")]
        public long WeekNumber { get; set; }
    }
}