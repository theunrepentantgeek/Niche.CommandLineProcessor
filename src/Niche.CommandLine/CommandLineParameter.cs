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
    public class CommandLineParameter<TValue> : CommandLineOptionBase
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

        public CommandLineParameter(object instance, MethodInfo method)
            : base(method)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (!method.DeclaringType.IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException("Expect method to be callable on instance");
            }

            mInstance = instance;
            mMethod = method;
            mParameterInfo = method.GetParameters().Single();
            IsRequired = method.GetCustomAttribute<RequiredAttribute>() != null;

            ShortName = "-" + CamelCase.ToShortName(method.Name);
            AlternateShortName = "/" + CamelCase.ToShortName(method.Name);
            LongName = "--" + CamelCase.ToDashedName(method.Name);
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

            mUsed = true;
            var parameter = arguments.Dequeue();
            var parameters = new object[] { parameter.As<TValue>() };
            mMethod.Invoke(mInstance, parameters);
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

            dictionary[ShortName] = this;
            dictionary[AlternateShortName] = this;
            dictionary[LongName] = this;
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
                    "{0} <{3}>\t{1} <{3}>\t{2}",
                    LongName,
                    ShortName,
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

            if (!mUsed && IsRequired)
            {
                var message
                    = string.Format(CultureInfo.CurrentCulture, "{0}:\t{1}", LongName, "Required parameter not supplied.");
                errors.Add(message);
            }
        }

        private readonly MethodInfo mMethod;
        private readonly ParameterInfo mParameterInfo;
        private readonly object mInstance;
        
        /// <summary>
        /// Record whether this parameter has been used
        /// </summary>
        private bool mUsed;
    }
}
