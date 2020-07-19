using CsvHelper;
using FFXIVWeather.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace FFXIVWeatherResourceGenerator
{
    class Program
    {
        const string WeatherKindsOutputPath = "../../../../FFXIVWeather/Data/weatherKinds.json";
        const string WeatherRateIndicesOutputPath = "../../../../FFXIVWeather/Data/weatherRateIndices.json";
        const string TerriTypesOutputPath = "../../../../FFXIVWeather/Data/terriTypes.json";

        static void Main(string[] args)
        {
            var http = new HttpClient();

            // GarlandTools
            Console.WriteLine("Requesting data from Garland Tools...");
            var dataStoreRaw = http.GetStringAsync(new Uri("https://www.garlandtools.org/db/doc/core/en/3/data.json")).GetAwaiter().GetResult();

            Console.WriteLine("Processing...");
            var dataStore = JObject.Parse(dataStoreRaw);
            
            var regionData = dataStore["skywatcher"]["weatherRateIndex"]
                .ToObject<IDictionary<string, WeatherRateIndex>>()
                .Select(kvp => kvp.Value);
            File.WriteAllText(WeatherRateIndicesOutputPath, JsonConvert.SerializeObject(regionData));

            // XIVAPI
            Console.WriteLine("Requesting data from XIVAPI and FFCafe...");
            var terriTypes = new List<TerriType>();

            {
                var page = 1;
                var pageTotal = 0;
                do
                {
                    var dataStore2Raw = http.GetStringAsync(new Uri($"https://xivapi.com/TerritoryType?columns=ID,WeatherRate,PlaceName&Page={page}")).GetAwaiter().GetResult();
                    var dataStore2 = JObject.Parse(dataStore2Raw);

                    pageTotal = dataStore2["Pagination"]["PageTotal"].ToObject<int>();

                    foreach (var child in dataStore2["Results"].Children())
                    {
                        if (!child["PlaceName"].Children().Any()) continue;

                        terriTypes.Add(new TerriType
                        {
                            Id = child["ID"].ToObject<int>(),
                            WeatherRate = child["WeatherRate"].ToObject<int>(),
                            NameEn = child["PlaceName"]["Name_en"].ToObject<string>(),
                            NameDe = child["PlaceName"]["Name_de"].ToObject<string>(),
                            NameFr = child["PlaceName"]["Name_fr"].ToObject<string>(),
                            NameJa = child["PlaceName"]["Name_ja"].ToObject<string>(),
                        });
                    }

                    page++;
                }
                while (page < pageTotal);

                var cafeCsvRaw = http.GetStreamAsync(new Uri("https://raw.githubusercontent.com/thewakingsands/ffxiv-datamining-cn/master/PlaceName.csv")).GetAwaiter().GetResult();
                using var cafeSr = new StreamReader(cafeCsvRaw);
                using var cafeCsv = new CsvReader(cafeSr, CultureInfo.InvariantCulture);
                for (var i = 0; i < 3; i++) cafeCsv.Read();
                while (cafeCsv.Read())
                {
                    var id = cafeCsv.GetField<int>(0);
                    var terriType = terriTypes.FirstOrDefault(tt => tt.Id == id);
                    if (terriType == null)
                        continue;
                    terriType.NameZh = cafeCsv.GetField<string>(1);
                }
            }
            
            File.WriteAllText(TerriTypesOutputPath, JsonConvert.SerializeObject(terriTypes));

            var weatherKinds = new List<Weather>();

            {
                var page = 1;
                var pageTotal = 0;
                do
                {
                    var dataStore2Raw = http.GetStringAsync(new Uri($"https://xivapi.com/Weather?columns=ID,Name_en,Name_de,Name_fr,Name_ja&Page={page}")).GetAwaiter().GetResult();
                    var dataStore2 = JObject.Parse(dataStore2Raw);

                    pageTotal = dataStore2["Pagination"]["PageTotal"].ToObject<int>();

                    foreach (var child in dataStore2["Results"].Children())
                    {
                        var id = child["ID"].ToObject<int>();

                        weatherKinds.Add(new Weather
                        {
                            Id = id,
                            NameEn = child["Name_en"].ToObject<string>(),
                            NameDe = child["Name_de"].ToObject<string>(),
                            NameFr = child["Name_fr"].ToObject<string>(),
                            NameJa = child["Name_ja"].ToObject<string>(),
                        });
                    }

                    page++;
                }
                while (page < pageTotal);

                var cafeCsvRaw = http.GetStreamAsync(new Uri("https://raw.githubusercontent.com/thewakingsands/ffxiv-datamining-cn/master/Weather.csv")).GetAwaiter().GetResult();
                using var cafeSr = new StreamReader(cafeCsvRaw);
                using var cafeCsv = new CsvReader(cafeSr, CultureInfo.InvariantCulture);
                for (var i = 0; i < 3; i++) cafeCsv.Read();
                while (cafeCsv.Read())
                {
                    var id = cafeCsv.GetField<int>(0);
                    var weatherKind = weatherKinds.FirstOrDefault(wk => wk.Id == id);
                    if (weatherKind == null)
                        continue;
                    weatherKind.NameZh = cafeCsv.GetField<string>(2);
                }
            }

            File.WriteAllText(WeatherKindsOutputPath, JsonConvert.SerializeObject(weatherKinds));

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
