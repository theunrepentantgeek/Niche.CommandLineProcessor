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
    ///  Wrapper class that handles a simple switch - something without a value
    /// </summary>
    public class CommandLineSwitch : CommandLineOptionBase
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

        public CommandLineSwitch(object instance, MethodInfo method)
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

            ShortName = "-" + CamelCase.ToShortName(method.Name);
            AlternateShortName = "/" + CamelCase.ToShortName(method.Name);
            LongName = "--" + CamelCase.ToDashedName(method.Name);
        }

        /// <summary>
        /// Activate this switch when found
        /// </summary>
        public override void Activate(Queue<string> arguments)
        {
            mMethod.Invoke(mInstance, null);
        }

        /// <summary>
        /// Carry out any switch configuration that needs to happen when we've finished processing the command line
        /// </summary>
        /// <param name="errors">List used to gather any reported errors.</param>
        public override void Completed(IList<string> errors)
        {
            // Nothing
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
                    "{0}\t{1}\t{2}",
                    LongName,
                    ShortName,
                    Description);

            help.Add(text);
        }

        private readonly object mInstance;
        private readonly MethodInfo mMethod;
    }
}
