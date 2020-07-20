using Newtonsoft.Json;

namespace FFXIVWeather.Models
{
    public class TerriType : LocalizedRow
    {
        [JsonProperty("weather_rate")]
        public int WeatherRate { get; set; }
    }
}
