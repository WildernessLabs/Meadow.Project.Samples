using System;

namespace MeadowServerTemperature.Entities
{
    public class DateTimeEntity
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