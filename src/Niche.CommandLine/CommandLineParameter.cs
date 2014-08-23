using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Wrapper class that handles a simple parameter - something that takes a value
    /// </summary>
    /// <typeparam name="TValue">Type of value expected by the parameter</typeparam>
    public sealed class CommandLineParameter<TValue> : CommandLineOptionBase
    {
        /// <summary>
        /// Gets the name of this parameter
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this parameter is required
        /// </summary>
        public bool IsRequired { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this parameter is multivalued
        /// </summary>
        public bool IsMultivalued { get; private set; }

        public CommandLineParameter(object instance, MethodInfo method)
            : base(method)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!method.DeclaringType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Expect method to be callable on instance");
            }

            mInstance = instance;
            mMethod = method;
            mParameterInfo = method.GetParameters().Single();

            Name = method.Name;

            IsRequired = method.GetCustomAttribute<RequiredAttribute>() != null;
            IsMultivalued = mParameterInfo.ParameterType.IsIEnumerable();
        }

        /// <summary>
        /// Activate this parameter when found
        /// </summary>
        public override void Activate(Queue<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            var value = arguments.Dequeue().As<TValue>();
            mValues.Add(value);
        }

        /// <summary>
        /// Add triggers to activate this option to the passed dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary that collects our triggers</param>
        public override void AddOptionsTo(Dictionary<string, CommandLineOptionBase> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            var shortForm = "-" + CamelCase.ToShortName(Name);
            var alternateForm = "/" + CamelCase.ToShortName(Name);
            var longForm = "--" + CamelCase.ToDashedName(Name);

            dictionary[shortForm] = this;
            dictionary[alternateForm] = this;
            dictionary[longForm] = this;
        }

        /// <summary>
        /// Add help text to the passed list
        /// </summary>
        /// <param name="help">List to capture the help text</param>
        public override void AddHelpTo(IList<string> help)
        {
            if (help == null)
            {
                throw new ArgumentNullException("help");
            }

            var text
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "--{0} <{3}>\t-{1} <{3}>\t{2}",
                    CamelCase.ToDashedName(Name),
                    CamelCase.ToShortName(Name),
                    Description,
                    mParameterInfo.Name.ToLower(CultureInfo.CurrentCulture));

            help.Add(text);
        }

        /// <summary>
        /// Carry out any parameter configuration that needs to happen when we've finished processing the command line
        /// </summary>
        /// <param name="errors">List used to gather any reported errors.</param>
        public override void Completed(IList<string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            if (IsRequired && !mValues.Any())
            {
                // Mandatory but not provided: create error
                var message
                    = string.Format(CultureInfo.CurrentCulture, "--{0}:\t{1}", CamelCase.ToDashedName(Name), "Required parameter not supplied.");
                errors.Add(message);
                return;
            }

            if (mValues.Count == 0)
            {
                // Nothing provided: return
                return;
            }

            if (IsMultivalued)
            {
                // Use all the values we have
                mMethod.Invoke(mInstance, new object[] { mValues });
                return;
            }

            if (mValues.Count > 1)
            {
                // Single valued, but we have too many values
                var message
                    = string.Format(CultureInfo.CurrentCulture, "--{0}:\t{1}", CamelCase.ToDashedName(Name), "Parameter may only be specified once.");
                errors.Add(message);
                return;
            }

            // Single valued: one value provided
            mMethod.Invoke(mInstance, new object[] { mValues[0] });
        }

        private readonly MethodInfo mMethod;
        private readonly ParameterInfo mParameterInfo;
        private readonly object mInstance;

        /// <summary>
        /// List of values passed for this parameter
        /// </summary>
        private readonly List<TValue> mValues = new List<TValue>();
    }
}
