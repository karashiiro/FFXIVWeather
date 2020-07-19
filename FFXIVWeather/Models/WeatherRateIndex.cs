using Newtonsoft.Json;

namespace FFXIVWeather.Models
{
    public class WeatherRateIndex
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("rates")]
        public WeatherRate[] Rates { get; private set; }
    }
}
