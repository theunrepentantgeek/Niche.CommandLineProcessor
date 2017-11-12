using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

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
        public string Description { get; }

        /// <summary>
        /// Gets a reference to the method used to implement this option
        /// </summary>
        /// Mostly useful for debugging
        public MethodInfo Method { get; }

        /// <summary>
        /// Initializes a new instance of the CommandLineOptionBase class
        /// </summary>
        /// <param name="method">Method to use for this option.</param>
        protected CommandLineOptionBase(MethodInfo method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            Description = FindDescription(method);
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

        /// <summary>
        /// Find the configured description for a specified method
        /// </summary>
        /// <param name="info">Method for which a description is requested.</param>
        /// <returns>Description for the method, if configured; empty string otherwise.</returns>
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
