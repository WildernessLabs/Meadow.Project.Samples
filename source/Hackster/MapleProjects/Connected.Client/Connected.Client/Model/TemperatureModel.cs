using Newtonsoft.Json;

namespace Connected.Client.Model
{
    public class TemperatureModel
    {
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("temperature")]
        public float Temperature { get; set; }
    }
}