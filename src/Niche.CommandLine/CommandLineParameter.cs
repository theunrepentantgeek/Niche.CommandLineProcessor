using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Niche.CommandLine
{
    /// <summary>
    /// Wrapper class that handles a simple parameter - something that takes a value
    /// </summary>
    /// <typeparam name="V">Type of value expected by the parameter</typeparam>
    [DebuggerDisplay("Parameter: {" + nameof(LongName) + "}")]
    public sealed class CommandLineParameter<V> : CommandLineOptionBase
    {
        // Information about the method we call to set this paraemeter
        private readonly MethodInfo _method;

        // Information about the single parameter to _method
        private readonly ParameterInfo _parameterInfo;

        // The instance we configure
        private readonly object _instance;

        // List of values passed for this parameter
        private readonly List<V> _values = new List<V>();

        /// <summary>
        /// Gets the short form of this switch
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Gets the other short form of this switch
        /// </summary>
        public string AlternateShortName { get; }

        /// <summary>
        /// Gets the long form of this switch
        /// </summary>
        public string LongName { get; }

        /// <summary>
        /// Gets a value indicating whether this parameter is required
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets a value indicating whether this parameter is multivalued
        /// </summary>
        public bool IsMultivalued { get; }

        /// <summary>
        /// Gets the sequence of values handled by this parameter
        /// </summary>
        public IEnumerable<V> Values => _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParameter{V}"/> class
        /// </summary>
        /// <param name="instance">Instance that we're configuring with this parameter.</param>
        /// <param name="method">Method to invoke if this parameter is used.</param>
        public CommandLineParameter(object instance, MethodInfo method)
            : base(method)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));

            if (!method.DeclaringType.IsInstanceOfType(instance))
            {
                throw new ArgumentException(
                    "Expect method to be callable on instance", nameof(method));
            }

            _method = method;
            _parameterInfo = method.GetParameters().Single();

            var name = method.Name;
            ShortName = "-" + CamelCase.ToShortName(name);
            AlternateShortName = "/" + CamelCase.ToShortName(name);
            LongName = "--" + CamelCase.ToDashedName(name);

            IsRequired = method.GetCustomAttribute<RequiredAttribute>() != null;
            IsMultivalued = _parameterInfo.ParameterType.IsIEnumerable();
        }

        /// <summary>
        /// Activate this parameter when found
        /// </summary>
        /// <param name="arguments">Arguments to process.</param>
        public override bool TryActivate(Queue<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (arguments.Count == 0)
            {
                return false;
            }

            var arg = arguments.Peek();
            if (ShortName.Equals(arg, StringComparison.CurrentCultureIgnoreCase)
                || AlternateShortName.Equals(arg, StringComparison.CurrentCultureIgnoreCase)
                || LongName.Equals(arg, StringComparison.CurrentCultureIgnoreCase))
            {
                arguments.Dequeue();
                var value = arguments.Dequeue().As<V>();
                _values.Add(value);
                return true;
            }

            if (arg.StartsWith(ShortName + ":", StringComparison.CurrentCultureIgnoreCase)
                || arg.StartsWith(AlternateShortName + ":", StringComparison.CurrentCultureIgnoreCase)
                || arg.StartsWith(LongName + ":", StringComparison.CurrentCultureIgnoreCase))
            {
                arguments.Dequeue();
                var value = arg.After(":").As<V>();
                _values.Add(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create help text for this option
        /// </summary>
        public override IEnumerable<string> CreateHelp()
        {
            var text
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0} <{3}>\t{1} <{3}>\t{2}",
                    LongName,
                    ShortName,
                    Description,
                    _parameterInfo.Name.ToLower(CultureInfo.CurrentCulture));

            yield return text;
        }

        /// <summary>
        /// Carry out any parameter configuration that needs to happen when we've finished processing the command line
        /// </summary>
        /// <param name="errors">List used to gather any reported errors.</param>
        public override void Completed(IList<string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            if (IsRequired && !_values.Any())
            {
                // Mandatory but not provided: create error
                var message
                    = string.Format(CultureInfo.CurrentCulture, "{0}:\t{1}", LongName, "Required parameter not supplied.");
                errors.Add(message);
                return;
            }

            if (_values.Count == 0)
            {
                // Nothing provided: return
                return;
            }

            if (IsMultivalued)
            {
                // Use all the values we have
                _method.Invoke(_instance, new object[] { _values });
                return;
            }

            if (_values.Count > 1)
            {
                // Single valued, but we have too many values
                var message
                    = string.Format(CultureInfo.CurrentCulture, "--{0}:\t{1}", LongName, "Parameter may only be specified once.");
                errors.Add(message);
                return;
            }

            // Single valued: one value provided
            _method.Invoke(_instance, new object[] { _values[0] });
        }
    }
}
