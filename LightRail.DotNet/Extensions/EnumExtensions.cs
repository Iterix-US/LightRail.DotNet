using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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
    }
}
