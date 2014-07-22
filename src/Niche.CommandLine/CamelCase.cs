using System;
using System.Globalization;
using System.Text;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility methods for dealing with CamelCase names.
    /// </summary>
    public static class CamelCase
    {
        /// <summary>
        /// Convert a camelCaseName into a dashed-format-name.
        /// </summary>
        /// <param name="camelCase">Name to convert</param>
        /// <returns>Converted name.</returns>
        public static string ToDashedName(string camelCase)      // --to-dashed-name  
        {
            if (camelCase == null)
            {
                throw new ArgumentNullException("camelCase");
            }

            var result = new StringBuilder();

            for (int i = 0; i < camelCase.Length; i++)
            {
                var thisChar = camelCase[i];
                if (i > 0 && i < camelCase.Length - 1)
                {
                    var nextChar = camelCase[i + 1];
                    var lastChar = camelCase[i - 1];
                    if (Char.IsUpper(thisChar))
                    {
                        if (Char.IsLower(lastChar) || Char.IsLower(nextChar))
                        {
                            result.Append("-");
                        }
                    }
                }

                result.Append(Char.ToLower(thisChar, CultureInfo.InvariantCulture));
            }

            return result.ToString();
        }

        /// <summary>
        /// Convert a camelCaseName into a short format name (ccn)
        /// </summary>
        /// <param name="camelCase">Name to convert.</param>
        /// <returns>Converted name.</returns>
        public static string ToShortName(string camelCase)  // -tsn
        {
            if (camelCase == null)
            {
                throw new ArgumentNullException("camelCase");
            }

            var result = new StringBuilder();

            for (int i = 0; i < camelCase.Length; i++)
            {
                var thisChar = camelCase[i];
                if (i > 0 && i < camelCase.Length - 1)
                {
                    var nextChar = camelCase[i + 1];
                    var lastChar = camelCase[i - 1];
                    if (Char.IsUpper(thisChar))
                    {
                        if (Char.IsLower(lastChar)
                            || Char.IsLower(nextChar))
                        {
                            result.Append(Char.ToLower(thisChar, CultureInfo.InvariantCulture));
                        }
                    }
                }
                else if (i == 0)
                {
                    result.Append(Char.ToLower(thisChar, CultureInfo.InvariantCulture));
                }
            }

            return result.ToString();
        }
    }
}
