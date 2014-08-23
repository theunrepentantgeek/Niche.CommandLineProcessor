using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        /// Gets or sets a description of this option
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the CommandLineOptionBase class
        /// </summary>
        protected CommandLineOptionBase(MemberInfo member)
        {
            Description = FindDescription(member);
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
        public abstract void AddOptionsTo(Dictionary<string, CommandLineOptionBase> dictionary);

        /// <summary>
        /// Add help text to the passed list
        /// </summary>
        /// <param name="help">List to capture the help text</param>
        public abstract void AddHelpTo(IList<string> help);

        /// <summary>
        /// Carry out any configuration that needs to happen when we've finished processing the command line
        /// </summary>
        /// <param name="errors">List used to gather any reported errors.</param>
        public abstract void Completed(IList<string> errors);

        protected internal static string FindDescription(MemberInfo info)
        {
            var attribute = info.GetCustomAttribute<DescriptionAttribute>();
            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Description;
        }
    }
}
