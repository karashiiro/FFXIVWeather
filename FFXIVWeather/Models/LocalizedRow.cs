using System;

namespace FFXIVWeather.Models
{
    public class LocalizedRow
    {
        public int Id { get; set; }

        public string NameEn { get; set; }

        public string NameDe { get; set; }

        public string NameFr { get; set; }

        public string NameJa { get; set; }

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
