﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
        /// Add triggers to activate this option to the passed dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary that collects our triggers</param>
        public abstract void AddTo(Dictionary<string, CommandLineOptionBase> dictionary);

        /// <summary>
        /// Convert a value from a string into the desired type
        /// </summary>
        /// <param name="value">String value to convert</param>
        /// <param name="desiredType">Desired type to return</param>
        /// <returns>Converted value</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static object ConvertValue(string value, Type desiredType)
        {
            if (desiredType==null)
            {
                throw new ArgumentNullException("desiredType");
            }

            if (value == null)
            {
                return null;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(desiredType);
                var result
                    = converter != null
                          ? converter.ConvertFromString(value)
                          : Convert.ChangeType(value, desiredType, CultureInfo.InvariantCulture);
                return result;
            }
            // Have to catch Exception 'cause that's what is thrown!
            catch (Exception ex)
            {
                string message
               = string.Format(
                   CultureInfo.InvariantCulture,
                   "Failed to convert \"{0}\" to {1}: {2}",
                   value,
                   desiredType.Name,
                   ex.Message);
                throw new InvalidOperationException(message);
            }
        }
    }
}
