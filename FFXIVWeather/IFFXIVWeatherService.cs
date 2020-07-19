using FFXIVWeather.Models;
using System;

namespace FFXIVWeather
{
    interface IFFXIVWeatherService
    {
        /// <summary>
        ///     Returns the next <paramref name="count"/> forecast entries for the provided territory type,
        ///     at a separation defined by <paramref name="secondIncrement"/> and from the provided
        ///     initial offset in seconds.
        /// </summary>
        /// <param name="terriTypeId">The territory type to calculate a forecast for.</param>
        /// <param name="count">The number of entries to return.</param>
        /// <param name="secondIncrement">The offset in seconds between forecasts.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <returns>An array of <see cref="Weather"/>/start time tuples for the specified teritory type.</returns>
        (Weather, DateTime)[] GetForecast(int terriTypeId, uint count, double secondIncrement, double initialOffset);

        /// <summary>
        ///     Returns the next <paramref name="count"/> forecast entries for the provided place,
        ///     at a separation defined by <paramref name="secondIncrement"/> and from the provided
        ///     initial offset in seconds.
        /// </summary>
        /// <param name="placeName">The place to calculate a forecast for.</param>
        /// <param name="count">The number of entries to return.</param>
        /// <param name="secondIncrement">The offset in seconds between forecasts.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <param name="lang">The language to read the place name in.</param>
        /// <returns>An array of <see cref="Weather"/>/start time tuples for the specified place.</returns>
        (Weather, DateTime)[] GetForecast(string placeName, uint count, double secondIncrement, double initialOffset, LangKind lang);

        /// <summary>
        ///     Returns the next <paramref name="count"/> forecast entries for the provided territory,
        ///     at a separation defined by <paramref name="secondIncrement"/> and from the provided
        ///     initial offset in seconds.
        /// </summary>
        /// <param name="terriType">The territory to calculate a forecast for.</param>
        /// <param name="count">The number of entries to return.</param>
        /// <param name="secondIncrement">The offset in seconds between forecasts.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <returns>An array of <see cref="Weather"/>/start time tuples for the specified territory.</returns>
        (Weather, DateTime)[] GetForecast(TerriType terriType, uint count, double secondIncrement, double initialOffset);

        /// <summary>
        ///     Returns the current <see cref="Weather"/> and its start time, relative to the provided offset in seconds,
        ///     for the specified territory type.
        /// </summary>
        /// <param name="terriTypeId">The teritory type to calculate a forecast for.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <returns>A <see cref="Weather"/>/start time tuple representing the current weather in the specified territory.</returns>
        (Weather, DateTime) GetCurrentWeather(int terriTypeId, double initialOffset);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placeName">The place to calculate a forecast for.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <param name="lang">The language to read the place name in.</param>
        /// <returns>A <see cref="Weather"/>/start time tuple representing the current weather in the specified place.</returns>
        (Weather, DateTime) GetCurrentWeather(string placeName, double initialOffset, LangKind lang);

        /// <summary>
        ///     Returns the current <see cref="Weather"/> and its start time, relative to the provided offset in seconds,
        ///     for the specified territory type.
        /// </summary>
        /// <param name="terriType">The teritory type to calculate a forecast for.</param>
        /// <param name="initialOffset">The offset in seconds from the current moment to begin forecasting for.</param>
        /// <returns>A <see cref="Weather"/>/start time tuple representing the current weather in the specified territory.</returns>
        (Weather, DateTime) GetCurrentWeather(TerriType terriType, double initialOffset);
    }
}
