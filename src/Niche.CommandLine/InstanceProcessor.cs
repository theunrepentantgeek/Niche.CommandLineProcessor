using System;
using System.Collections.Generic;
using System.Linq;

namespace Niche.CommandLine
{
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

        public InstanceProcessor(T instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Switches = CommandLineOptionFactory.CreateSwitches(Instance);
            Parameters = CommandLineOptionFactory.CreateParameters(Instance);
            Modes = CommandLineOptionFactory.CreateModes(Instance);
        }

        public void Populate(Queue<string> arguments, IList<string> errors)
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
