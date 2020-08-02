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
        const string WeatherKindsOutputPath = "FFXIVWeather/Data/weatherKinds.json";
        const string WeatherRateIndicesOutputPath = "FFXIVWeather/Data/weatherRateIndices.json";
        const string TerriTypesOutputPath = "FFXIVWeather/Data/terriTypes.json";

        static void Main(string[] args)
        {
            var http = new HttpClient();

            // GarlandTools
            Console.WriteLine("Requesting data from Garland Tools...");
            var dataStoreRaw = http.GetStringAsync(new Uri("https://www.garlandtools.org/db/doc/core/en/3/data.json")).GetAwaiter().GetResult();

            Console.WriteLine("Processing...");
            var dataStore = JObject.Parse(dataStoreRaw);

            var weatherRateIndices = new List<WeatherRateIndex>();
            var wris = dataStore["skywatcher"]["weatherRateIndex"].Children()
                .Select(token => token.Children().First());
            foreach (var wri in wris)
            {
                weatherRateIndices.Add(new WeatherRateIndex
                {
                    Id = wri["id"].ToObject<int>(),
                    Rates = wri["rates"].Children()
                        .Select(rate => new WeatherRate
                        {
                            Id = rate["weather"].ToObject<int>(),
                            Rate = rate["rate"].ToObject<int>(),
                        })
                        .ToArray(),
                });
            }

            // Quick validation of a design assumption.
            var wriLastN = 0;
            foreach (var weatherRateIndex in weatherRateIndices)
            {
                if (weatherRateIndex.Id != wriLastN)
                    throw new InvalidDataException("Data is not continuous and/or sorted in ascending order.");
                wriLastN++;
            }
            File.WriteAllText(WeatherRateIndicesOutputPath, JsonConvert.SerializeObject(weatherRateIndices));

            // XIVAPI
            Console.WriteLine("Requesting data from XIVAPI and FFCafe...");
            var terriTypes = new List<TerriType>();

            {
                var page = 1;
                var pageTotal = 1;
                while (page <= pageTotal)
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

            // Quick validation of a design assumption.
            var ttLastN = 0;
            foreach (var terriType in terriTypes)
            {
                if (terriType.Id < ttLastN)
                    throw new InvalidDataException("Data is not sorted in ascending order.");
                ttLastN = terriType.Id;
            }
            
            File.WriteAllText(TerriTypesOutputPath, JsonConvert.SerializeObject(terriTypes));

            var weatherKinds = new List<Weather>();

            {
                var page = 1;
                var pageTotal = 1;
                while (page <= pageTotal)
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


            // Quick validation of a design assumption.
            var wkLastN = 1;
            foreach (var weatherKind in weatherKinds)
            {
                if (weatherKind.Id != wkLastN)
                    throw new InvalidDataException("Data is not continuous and/or sorted in ascending order.");
                wkLastN++;
            }
            File.WriteAllText(WeatherKindsOutputPath, JsonConvert.SerializeObject(weatherKinds));

            Console.WriteLine("Done!");
        }
    }
}
