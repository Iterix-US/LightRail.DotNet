using System;

namespace LightRail.DotNet.Extensions
{
    /// <summary>
    /// Extension methods for the DateTime data type.
    /// </summary>
    public static class DateTimeExtensions
    {
        private const long AnnualTicks = 3153600000000;
        private const long MonthlyTicks = 259200000000;
        private const long DailyTicks = 8640000000;
        private const long HourlyTicks = 360000000;
        private const long MinuteTicks = 6000000;
        private const long SecondTicks = 100000;
        private const long MillisecondTicks = 10000;

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
