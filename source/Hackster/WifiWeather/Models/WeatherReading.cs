using System.Text.Json.Serialization;

namespace WifiWeather.Models
{
    public class WeatherReading
    {
        public WeatherReading() { }

        [JsonPropertyName("coord")]
        public Coordinates Coordinates { get; set; }

        [JsonPropertyName("weather")]
        public Weather[] Weather { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("main")]
        public WeatherValues WeatherValues { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }

        [JsonPropertyName("dt")]
        public int DateTime { get; set; }

        [JsonPropertyName("sys")]
        public System System { get; set; }

        [JsonPropertyName("timezone")]
        public long Timezone { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cod")]
        public int Code { get; set; }
    }

    public class Coordinates 
    {
        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }
    }

    public class Weather 
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public class WeatherValues
    {
        [JsonPropertyName("temp")]
        public decimal Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public decimal FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public decimal TemperatureMin { get; set; }

        [JsonPropertyName("temp_max")]
        public decimal TemperatureMax { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int humidity { get; set; }
    }

    public class Wind 
    {
        [JsonPropertyName("speed")]
        public decimal Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Degrees { get; set; }
    }

    public class Clouds 
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }

    public class System 
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }


}