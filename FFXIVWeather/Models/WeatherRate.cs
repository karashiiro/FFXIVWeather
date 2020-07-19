using Newtonsoft.Json;

namespace FFXIVWeather.Models
{
    public class WeatherRate
    {
        [JsonProperty("weather")]
        public int Id { get; private set; }

        [JsonProperty("rate")]

        public int Rate { get; private set; }
    }
}
