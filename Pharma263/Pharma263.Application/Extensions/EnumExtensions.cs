using System;
using System.ComponentModel;
using System.Reflection;

namespace Pharma263.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }

        public static TEnum GetValueFromDescription<TEnum>(string description) where TEnum : Enum
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException($"No enum value found with the description '{description}'", nameof(description));
        }
    }
}
