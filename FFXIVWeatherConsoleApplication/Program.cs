using FFXIVWeather;
using FFXIVWeather.Models;
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

            var stopwatch = new Stopwatch();
            for (var i = 0; i < 100000; i++)
            {
                stopwatch.Start();
                weatherService.GetForecast(zone, count);
                stopwatch.Stop();
            }

            Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds}ms.");

            /*var forecast = weatherService.GetForecast(zone, count);

            Console.WriteLine($"Weather for {zone}:");
            Console.WriteLine("|\tWeather\t\t|\tTime\t|");
            Console.WriteLine("+-----------------------+---------------+");
            foreach (var (weather, startTime) in forecast)
            {
                Console.WriteLine($"|\t{(weather.ToString().Length < 8 ? weather.ToString() + '\t' : weather.ToString())}\t|\t{Math.Round((startTime - DateTime.UtcNow).TotalMinutes)}m\t|");
            }*/
        }
    }
}
