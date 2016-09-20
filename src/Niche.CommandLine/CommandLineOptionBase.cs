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
        /// Gets a reference to the method used to implement this option
        /// </summary>
        /// Mostly useful for debugging
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// Initializes a new instance of the CommandLineOptionBase class
        /// </summary>
        protected CommandLineOptionBase(MethodInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            Method = member;
            Description = FindDescription(member);
        }

        /// <summary>
        /// Try to activate this option
        /// </summary>
        /// <param name="arguments">Available arguments for handling.</param>
        /// <returns>True if activated, false otherwise.</returns>
        public abstract bool TryActivate(Queue<string> arguments);

        /// <summary>
        /// Create help text for this option
        /// </summary>
        public abstract IEnumerable<string> CreateHelp();

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
