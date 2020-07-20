using Newtonsoft.Json;

namespace FFXIVWeather.Models
{
    public class WeatherRate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("rate")]

        public int Rate { get; set; }
    }
}
