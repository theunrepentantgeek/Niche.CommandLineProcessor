using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

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
        public string ShortName { get; }

        /// <summary>
        /// Gets the other short form of this switch
        /// </summary>
        public string AlternateShortName { get; }

        /// <summary>
        /// Gets the long form of this switch
        /// </summary>
        public string LongName { get; }

        public CommandLineSwitch(object instance, MethodInfo method)
            : base(method)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!method.DeclaringType.IsInstanceOfType(instance))
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
        public override bool TryActivate(Queue<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (arguments.Count==0)
            {
                return false;
            }

            var arg = arguments.Peek();
            if (ShortName.Equals(arg, StringComparison.CurrentCultureIgnoreCase)
                || AlternateShortName.Equals(arg, StringComparison.CurrentCultureIgnoreCase)
                || LongName.Equals(arg, StringComparison.CurrentCultureIgnoreCase))
            {
                arguments.Dequeue();
                mMethod.Invoke(mInstance, null);
                return true;
            }

            return false;
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
        /// Create help text for this option
        /// </summary>
        public override IEnumerable<string> CreateHelp()
        {
            var text
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}\t{1}\t{2}",
                    LongName,
                    ShortName,
                    Description);

            yield return text;
        }

        private readonly object mInstance;
        private readonly MethodInfo mMethod;
    }
}
