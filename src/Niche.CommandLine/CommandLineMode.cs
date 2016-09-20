using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class CommandLineMode
    {
        /// <summary>
        /// Gets the name of this mode
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a description of this option
        /// </summary>
        public string Description { get; private set; }

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

            if (!method.DeclaringType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Expect method to be callable on instance");
            }

            if (!driverType.IsAssignableFrom(method.ReturnType))
            {
                var message
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        "Expected method return type to be compatible with {0}",
                        driverType.Name);
                throw new ArgumentException(message);
            }

            mInstance = instance;
            mMethod = method;
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
            return mMethod.Invoke(mInstance, null);
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

        private readonly object mInstance;
        private readonly MethodInfo mMethod;
    }
}
