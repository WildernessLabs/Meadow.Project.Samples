using Newtonsoft.Json;
using System;

namespace WifiWeatherClock.Models
{
    public class DateTimeModel
    {
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("client_ip")]
        public string ClientIp { get; set; }

        [JsonProperty("datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("day_of_week")]
        public long DayOfWeek { get; set; }

        [JsonProperty("day_of_year")]
        public long DayOfYear { get; set; }

        [JsonProperty("dst")]
        public bool Dst { get; set; }

        [JsonProperty("dst_from")]
        public object DstFrom { get; set; }

        [JsonProperty("dst_offset")]
        public long DstOffset { get; set; }

        [JsonProperty("dst_until")]
        public object DstUntil { get; set; }

        [JsonProperty("raw_offset")]
        public long RawOffset { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("unixtime")]
        public long Unixtime { get; set; }

        [JsonProperty("utc_datetime")]
        public DateTimeOffset UtcDatetime { get; set; }

        [JsonProperty("utc_offset")]
        public string UtcOffset { get; set; }

        [JsonProperty("week_number")]
        public long WeekNumber { get; set; }
    }
}