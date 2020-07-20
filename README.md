# FFXIVWeather
FFXIV weather forecast library for C# applications.

Credit to [Garland Tools](https://www.garlandtools.org/) for crowdsourced weather data, and [XIVAPI](https://xivapi.com/) and [FFCafe](https://ffcafe.org/) for game data.

## Installation
`Install-Package FFXIVWeather` or other methods as described [here](https://www.nuget.org/packages/FFXIVWeather/).

## Example
Code:
```cs
var weatherService = new FFXIVWeatherService();
var zone = "Eureka Pyros";
var count = 15U;

var forecast = weatherService.GetForecast(zone, count);

Console.WriteLine($"Weather for {zone}:");
Console.WriteLine("|\tWeather\t\t|\tTime\t|");
Console.WriteLine("+-----------------------+---------------+");
foreach (var (weather, startTime) in forecast)
{
    Console.WriteLine($"|\t{(weather.ToString().Length < 8 ? weather.ToString() + '\t' : weather.ToString())}\t|\t{Math.Round((startTime - DateTime.UtcNow).TotalMinutes)}m\t|");
}
```

Output:
```
Weather for Eureka Pyros:
|       Weather         |       Time    |
+-----------------------+---------------+
|       Umbral Wind     |       -3m     |
|       Blizzards       |       20m     |
|       Thunder         |       43m     |
|       Umbral Wind     |       67m     |
|       Umbral Wind     |       90m     |
|       Blizzards       |       113m    |
|       Thunder         |       137m    |
|       Heat Waves      |       160m    |
|       Umbral Wind     |       183m    |
|       Blizzards       |       207m    |
|       Heat Waves      |       230m    |
|       Thunder         |       253m    |
|       Blizzards       |       277m    |
|       Umbral Wind     |       300m    |
|       Thunder         |       323m    |
```
## Maintaining
In the event that I cease to maintain this library and [ffxivweather-py](https://github.com/karashiiro/ffxivweather-py) before game content updates cease, here's the patch-update process:
1. Run the resource generator, which will output the 3 JSON resources into the library's data directory.
2. Compile and publish the library.
3. Copy the 3 JSON resources to ffxivweather-py's `store` directory.
4. Publish that library.
