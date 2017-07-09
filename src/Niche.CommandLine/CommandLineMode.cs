using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Niche.CommandLine
{
    public class CommandLineMode
    {
        private readonly object _instance;

        private readonly MethodInfo _method;

        /// <summary>
        /// Gets the name of this mode
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a description of this option
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the CommandLineMode class
        /// </summary>
        /// <param name="driverType">Type of the driver</param>
        /// <param name="instance">Instance on which the mode was declared.</param>
        /// <param name="method">Method to invoke to activate the mode.</param>
        public CommandLineMode(Type driverType, object instance, MethodInfo method)
        {
            if (driverType == null)
            {
                throw new ArgumentNullException(nameof(driverType));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (!method.DeclaringType.IsInstanceOfType(instance))
            {
                throw new ArgumentException("Expect method to be callable on instance", nameof(method));
            }

            if (!driverType.IsAssignableFrom(method.ReturnType))
            {
                var message
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        "Expected method return type to be compatible with {0}",
                        driverType.Name);
                throw new ArgumentException(message, nameof(method));
            }

            _instance = instance;
            _method = method;
            Description = CommandLineOptionBase.FindDescription(method);

            Name = CamelCase.ToDashedName(method.Name);
        }

        public bool HasName(string name)
        {
            return string.Equals(Name, name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Activate this switch when found
        /// </summary>
        public object Activate()
        {
            return _method.Invoke(_instance, null);
        }

        /// <summary>
        /// Create help text for this option
        /// </summary>
        public IEnumerable<string> CreateHelp()
        {
            var text
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}\t\t{1}",
                    Name,
                    Description);

            yield return text;
        }
    }
}
