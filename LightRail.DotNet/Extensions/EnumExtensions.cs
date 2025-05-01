using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Serilog;
using Serilog.Events;

namespace LightRail.DotNet.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            return field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
        }

        public static List<T> GetAllValues<T>(this T enumValue) where T : Enum
        {
            return Enum.GetValues(enumValue.GetType()).Cast<T>().ToList();
        }

        public static LogEventLevel ToSerilogLevel(this LoggingLevel level)
        {
            switch (level)
            {
                case LoggingLevel.Debug:
                    return LogEventLevel.Debug;
                case LoggingLevel.Information:
                    return LogEventLevel.Information;
                case LoggingLevel.Warning:
                    return LogEventLevel.Warning;
                case LoggingLevel.Error:
                    return LogEventLevel.Error;
                case LoggingLevel.Fatal:
                    return LogEventLevel.Fatal;
                case LoggingLevel.Verbose:
                    return LogEventLevel.Verbose;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "Invalid logging level specified.");
            }
        }

        public static RollingInterval ToSerilogRollingInterval(this LogRollInterval logRollInterval)
        {
            switch (logRollInterval)
            {
                case LogRollInterval.Daily:
                    return RollingInterval.Day;
                case LogRollInterval.Hourly:
                    return RollingInterval.Hour;
                case LogRollInterval.Monthly:
                    return RollingInterval.Month;
                case LogRollInterval.Yearly:
                    return RollingInterval.Year;
                case LogRollInterval.Indefinite:
                    return RollingInterval.Infinite;
                default:
                case LogRollInterval.Weekly:
                    throw new ArgumentOutOfRangeException(nameof(logRollInterval), logRollInterval, "Serilog does not support this rolling log interval selection.");
            }
        }
    }
}
