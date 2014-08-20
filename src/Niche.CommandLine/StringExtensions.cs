using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility conversion extensions to String
    /// </summary>
    internal static class StringExtensions
    {
        public static T As<T>(this string value)
        {
            object v = value;

            // If a string, just return it
            if (typeof(T) == typeof(string))
            {
                return (T)v;
            }

            // Use a TypeConverter if one is available
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    return (T)converter.ConvertFromString(value);
                }
                catch (Exception ex)
                {
                    string failureMessage
                   = string.Format(
                       CultureInfo.InvariantCulture,
                       "Failed to convert \"{0}\" to {1}: {2}",
                       value,
                       typeof(T).Name,
                       ex.Message);
                    throw new InvalidOperationException(failureMessage);
                }
            }

            // Look for a constructor that takes a string
            var constructor = typeof(T).GetConstructor(new[] { typeof(string) });
            if (constructor != null)
            {
                return (T)constructor.Invoke(new[] { value });
            }

            var message = string.Format(CultureInfo.CurrentCulture, "Cannot convert {0} into {1}", value, typeof(T).Name);
            throw new InvalidOperationException(message);
        }
    }
}
