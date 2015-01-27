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
        /// Gets the short form of this switch
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Gets the other short form of this switch
        /// </summary>
        public string AlternateShortName { get; private set; }

        /// <summary>
        /// Gets the long form of this switch
        /// </summary>
        public string LongName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this parameter is required
        /// </summary>
        public bool IsRequired { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this parameter is multivalued
        /// </summary>
        public bool IsMultivalued { get; private set; }

        /// <summary>
        /// Gets the sequence of values handled by this parameter
        /// </summary>
        public IEnumerable<TValue> Values { get { return mValues; } }

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

            var name = method.Name;
            ShortName = "-" + CamelCase.ToShortName(name);
            AlternateShortName = "/" + CamelCase.ToShortName(name);
            LongName = "--" + CamelCase.ToDashedName(name);

            IsRequired = method.GetCustomAttribute<RequiredAttribute>() != null;
            IsMultivalued = mParameterInfo.ParameterType.IsIEnumerable();
        }

        /// <summary>
        /// Activate this parameter when found
        /// </summary>
        public override bool TryActivate(Queue<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
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
                var value = arguments.Dequeue().As<TValue>();
                mValues.Add(value);
                return true;
            }

            if (arg.StartsWith(ShortName + ":", StringComparison.CurrentCultureIgnoreCase)
                || arg.StartsWith(AlternateShortName + ":", StringComparison.CurrentCultureIgnoreCase)
                || arg.StartsWith(LongName + ":", StringComparison.CurrentCultureIgnoreCase))
            {
                arguments.Dequeue();
                var value = arg.After(":").As<TValue>();
                mValues.Add(value);
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
                    mParameterInfo.Name.ToLower(CultureInfo.CurrentCulture));

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
                throw new ArgumentNullException("errors");
            }

            if (IsRequired && !mValues.Any())
            {
                // Mandatory but not provided: create error
                var message
                    = string.Format(CultureInfo.CurrentCulture, "--{0}:\t{1}", LongName, "Required parameter not supplied.");
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
                    = string.Format(CultureInfo.CurrentCulture, "--{0}:\t{1}", LongName, "Parameter may only be specified once.");
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
