using Newtonsoft.Json;

namespace MobileMaple.Model
{    
    [JsonObject]
    public class TemperatureModel
    {
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }
        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }
}