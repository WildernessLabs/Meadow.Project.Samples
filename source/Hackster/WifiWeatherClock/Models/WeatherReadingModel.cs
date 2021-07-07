using Newtonsoft.Json;

namespace WifiWeatherClock.Models
{
    public class WeatherReadingModel 
    { 
        public WeatherReadingModel() { }

        [JsonProperty("coord")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("main")]
        public WeatherValues WeatherValues { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("dt")]
        public int DateTime { get; set; }

        [JsonProperty("sys")]
        public System System { get; set; }

        [JsonProperty("timezone")]
        public long Timezone { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public int Code { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }
    }

    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class WeatherValues
    {
        [JsonProperty("temp")]
        public decimal Temperature { get; set; }

        [JsonProperty("feels_like")]
        public decimal FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public decimal TemperatureMin { get; set; }

        [JsonProperty("temp_max")]
        public decimal TemperatureMax { get; set; }

        [JsonProperty("pressure")]
        public int Pressure { get; set; }

        [JsonProperty("humidity")]
        public int humidity { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public decimal Speed { get; set; }

        [JsonProperty("deg")]
        public int Degrees { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }
    }

    public class System
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }
}