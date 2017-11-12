using System;
using System.Collections.Generic;
using System.Linq;

namespace Niche.CommandLine
{
    /// <summary>
    /// Processor for configuring a specific instance from commandline arguments
    /// </summary>
    /// <typeparam name="T">Type of instance we're configuring.</typeparam>
    public class InstanceProcessor<T> : IInstanceProcessor
        where T : class
    {
        /// <summary>
        /// Gets the instance to configure from the command line
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Gets all the modes defined on <see cref="Instance"/>
        /// </summary>
        public IReadOnlyList<CommandLineMode> Modes { get; }

        /// <summary>
        /// Gets all the switches defined on <see cref="Instance"/>
        /// </summary>
        public IReadOnlyList<CommandLineSwitch> Switches { get; }

        /// <summary>
        /// Gets all the parameters defined on <see cref="Instance"/>
        /// </summary>
        public IReadOnlyList<CommandLineOptionBase> Parameters { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InstanceProcessor{T}"/>
        /// </summary>
        /// <param name="instance">Type of instance we're configuring.</param>
        public InstanceProcessor(T instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Switches = CommandLineOptionFactory.CreateSwitches(Instance);
            Parameters = CommandLineOptionFactory.CreateParameters(Instance);
            Modes = CommandLineOptionFactory.CreateModes(Instance);
        }

        /// <summary>
        /// Parse the arguments in the provided queue and configure our instance, leaving any unprocessed arguments in place
        /// </summary>
        /// <param name="arguments">Queue of command line arguments to process.</param>
        /// <param name="errors">List to collect any errors that occur.</param>
        public void Parse(Queue<string> arguments, IList<string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            var args = new Queue<string>(arguments ?? throw new ArgumentNullException(nameof(arguments)));
            arguments.Clear();
            while (args.Count > 0)
            {
                var activated =
                    Switches.FirstOrDefault(s => s.TryActivate(args))
                    ?? Parameters.FirstOrDefault(p => p.TryActivate(args));
                if (activated == null)
                {
                    arguments.Enqueue(args.Dequeue());
                }
            }

            Completed(errors);
        }

        private void Completed(IList<string> errors)
        {
            foreach (var option in Switches.Concat(Parameters))
            {
                option.Completed(errors);
            }
        }
    }
}
