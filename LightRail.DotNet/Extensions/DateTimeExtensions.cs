using System;

namespace LightRail.DotNet.Extensions
{
    /// <summary>
    /// Extension methods for the DateTime data type.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Subtracts a specified number of years, months, days, hours, minutes, seconds, and milliseconds from the base DateTime.
        /// </summary>
        /// <param name="dateTime">The base DateTime to subtract from</param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime Minus(this DateTime dateTime, int years = 0, int months = 0, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            var result = dateTime
                .AddYears(-years)
                .AddMonths(-months)
                .AddDays(-days)
                .AddHours(-hours)
                .AddMinutes(-minutes)
                .AddSeconds(-seconds)
                .AddMilliseconds(-milliseconds);

            return result;
        }

        /// <summary>
        /// Subtracts a specified DateTime from the base DateTime.
        /// </summary>
        /// <param name="dateTime">The base DateTime to subtract from</param>
        /// <param name="subtractionDateTime">The DateTime being subtracted from the base</param>
        /// <returns></returns>
        public static DateTime Minus(this DateTime dateTime, DateTime subtractionDateTime)
        {
            return dateTime.Minus(
                subtractionDateTime.Year,
                subtractionDateTime.Month,
                subtractionDateTime.Day,
                subtractionDateTime.Hour,
                subtractionDateTime.Minute,
                subtractionDateTime.Second,
                subtractionDateTime.Millisecond
            );
        }

        /// <summary>
        /// Adds a specified number of years, months, days, hours, minutes, seconds, and milliseconds to the base DateTime.
        /// </summary>
        /// <param name="dateTime">The base DateTime to add to</param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime Plus(this DateTime dateTime, int years = 0, int months = 0, int days = 0, int hours = 0, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            var result = dateTime
                .AddYears(years)
                .AddMonths(months)
                .AddDays(days)
                .AddHours(hours)
                .AddMinutes(minutes)
                .AddSeconds(seconds)
                .AddMilliseconds(milliseconds);

            return result;
        }

        /// <summary>
        /// Adds a specified DateTime to the base DateTime.
        /// </summary>
        /// <param name="dateTime">The base DateTime to add to</param>
        /// <param name="additionDateTime">The DateTime being added to the base</param>
        /// <returns></returns>
        public static DateTime Plus(this DateTime dateTime, DateTime additionDateTime)
        {
            return dateTime.Plus(
                additionDateTime.Year,
                additionDateTime.Month,
                additionDateTime.Day,
                additionDateTime.Hour,
                additionDateTime.Minute,
                additionDateTime.Second,
                additionDateTime.Millisecond
            );
        }
    }
}
