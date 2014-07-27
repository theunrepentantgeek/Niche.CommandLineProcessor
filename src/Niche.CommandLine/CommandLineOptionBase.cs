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
    /// Base class for CommandLine argument handlers
    /// </summary>
    public abstract class CommandLineOptionBase
    {
        /// <summary>
        /// Initializes a new instance of the CommandLineOptionBase class
        /// </summary>
        protected CommandLineOptionBase()
        {
            // Nothing
        }

        /// <summary>
        /// Activate this option when found
        /// </summary>
        /// <param name="arguments">Available arguments for handling.</param>
        public abstract void Activate(Queue<string> arguments);

        /// <summary>
        /// Convert a value from a string into the desired type
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <param name="desiredType">Desired type to return</param>
        /// <returns>Converted value</returns>
        protected object ConvertValue(string value, Type desiredType)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(desiredType);
                var result
                    = converter != null
                          ? converter.ConvertFromString(value)
                          : Convert.ChangeType(value, desiredType, CultureInfo.InvariantCulture);
                return result;
            }
            catch (NotSupportedException ex)
            {
                ReportConversionError(value, desiredType, ex);
            }
            catch (InvalidCastException ex)
            {
                ReportConversionError(value, desiredType, ex);
            }
            catch (FormatException ex)
            {
                ReportConversionError(value, desiredType, ex);
            }
            catch (OverflowException ex)
            {
                ReportConversionError(value, desiredType, ex);
            }
            catch (ArgumentNullException ex)
            {
                ReportConversionError(value, desiredType, ex);
            }

            return null;
        }

        /// <summary>
        /// Report a problem converting a value to the desired type
        /// </summary>
        /// <param name="value">Value we tried to convert.</param>
        /// <param name="desiredType">Type we try to convert to.</param>
        /// <param name="exception">Conversion exception.</param>
        private void ReportConversionError(string value, Type desiredType, Exception exception)
        {
            string message
                = string.Format(
                    CultureInfo.InvariantCulture,
                    "Failed to convert \"{0}\" to {1}: {2}",
                    value,
                    desiredType.Name,
                    exception.Message);
            throw new InvalidOperationException(message);
        }
    }
}
