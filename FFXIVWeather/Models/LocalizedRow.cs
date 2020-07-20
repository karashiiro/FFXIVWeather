using Newtonsoft.Json;
using System;

namespace FFXIVWeather.Models
{
    public class LocalizedRow
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name_en")]
        public string NameEn { get; set; }

        [JsonProperty("name_de")]
        public string NameDe { get; set; }

        [JsonProperty("name_fr")]
        public string NameFr { get; set; }

        [JsonProperty("name_ja")]
        public string NameJa { get; set; }

        [JsonProperty("name_zh")]
        public string NameZh { get; set; }

        public string GetName(LangKind lang = LangKind.En) => lang switch
        {
            LangKind.En => NameEn,
            LangKind.De => NameDe,
            LangKind.Fr => NameFr,
            LangKind.Ja => NameJa,
            LangKind.Zh => NameZh,
            _ => throw new NotImplementedException(),
        };

        public override string ToString() => GetName();
    }
}
