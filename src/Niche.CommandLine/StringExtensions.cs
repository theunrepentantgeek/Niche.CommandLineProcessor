using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility conversion extensions to String
    /// </summary>
    /// Some custom functionality specific to the CommandLineProcessor's needs.
    /// Reuse with care.
    internal static class StringExtensions
    {
        /// <summary>
        /// Return the part of a string after a given separator.
        /// </summary>
        /// <param name="original">Original string to split.</param>
        /// <param name="separator">Separator to look for.</param>
        /// <returns>Part of the string after the separator.</returns>
        public static string After(this string original, string separator)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException("Expect to have a separator defined", nameof(separator));
            }

            var index = original.IndexOf(separator, StringComparison.Ordinal);

            if (index == -1)
            {
                return string.Empty;
            }

            return original.Substring(index + separator.Length);
        }

        /// <summary>
        /// Return the part of the string before a given separator.
        /// </summary>
        /// <param name="original">Original string to split.</param>
        /// <param name="separator">Separator to look for.</param>
        /// <returns>Part of the string before the separator.</returns>
        public static string Before(this string original, string separator)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentException("Expect to have a separator defined", nameof(separator));
            }

            var index = original.IndexOf(separator, StringComparison.Ordinal);

            if (index == -1)
            {
                return original;
            }

            return original.Substring(0, index);
        }

        public static T As<T>(this string value)
        {
            return (T)As(value, typeof(T));
        }

        private static object As(string value, Type convertTo)
        {
            // If a string, just return it
            if (convertTo == typeof(string))
            {
                return value;
            }

            if (TryConvention(convertTo, value, out object result))
            {
                return result;
            }

            if (TryConstructor(convertTo, value, out result))
            {
                return result;
            }

            if (TryConvert(convertTo, value, out result))
            {
                return result;
            }

            var message = string.Format(CultureInfo.CurrentCulture, "Cannot convert {0} into {1}", value, convertTo.Name);
            throw new InvalidOperationException(message);
        }

        private static bool TryConvert(Type convertTo, string value, out object result)
        {
            // Use a TypeConverter if one is available
            var converter = TypeDescriptor.GetConverter(convertTo);
            if (converter == null || !converter.CanConvertFrom(typeof(string)))
            {
                result = null;
                return false;
            }

            try
            {
                result = converter.ConvertFromString(value);
                return true;
            }
            // Yeah, capturing Exception is bad
            // Except that the BCL throws it, so can't write a more specific catch
            // see http://www.nichesoftware.co.nz/2013/02/21/so-you-should-never-catch-exception.html
            catch (Exception ex)
            {
                var failureMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    "Failed to convert \"{0}\" to {1}: {2}",
                    value,
                    convertTo.Name,
                    ex.Message);
                throw new InvalidOperationException(failureMessage);
            }
        }

        private static bool TryConstructor(Type convertTo, string value, out object result)
        {
            // Look for a constructor that takes a single string parameter
            var constructor = convertTo.GetTypeInfo().GetConstructor(new[] { typeof(string) });
            if (constructor != null)
            {
                result = constructor.Invoke(new[] { value });
                return true;
            }

            result = null;
            return false;
        }

        private static bool TryConvention(Type convertTo, string value, out object result)
        {
            MethodInfo conversionMethod;
            if (convertTo.GetTypeInfo().IsGenericType)
            {
                var baseType = convertTo.GetGenericTypeDefinition();
                var methodName = "As" + baseType.Name.Before("`");
                conversionMethod
                    = typeof(StringExtensions)
                        .GetTypeInfo().GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
                if (conversionMethod != null)
                {
                    conversionMethod
                        = conversionMethod.MakeGenericMethod(convertTo.GetTypeInfo().GetGenericArguments());
                }
            }
            else
            {
                conversionMethod
                    = typeof(StringExtensions)
                        .GetTypeInfo().GetMethod("As" + convertTo.Name, BindingFlags.Static | BindingFlags.NonPublic);
            }

            if (conversionMethod == null)
            {
                result = null;
                return false;
            }

            result = conversionMethod.Invoke(null, new object[] { value });
            return true;
        }

        private static KeyValuePair<K, V> AsKeyValuePair<K,V>(string value)
        {
            var k = value.Before("=").As<K>();
            var v = value.After("=").As<V>();
            return new KeyValuePair<K, V>(k, v);
        }
    }
}
