using FFXIVWeather;
using System;
using System.Diagnostics;

namespace FFXIVWeatherConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
