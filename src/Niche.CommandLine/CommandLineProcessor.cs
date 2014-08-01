using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class CommandLineProcessor
    {
        public IConsoleLogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        public CommandLineProcessor(IEnumerable<string> arguments)
            : this(arguments, new ConsoleLogger())
       { 
            // Nothing
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        public CommandLineProcessor(IEnumerable<string> arguments, IConsoleLogger logger)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            Logger = logger;
            mArguments = arguments.ToList();
        }

        /// <summary>
        /// Configure the passed driver instance using available arguments
        /// </summary>
        /// <param name="driver">Instance to configure.</param>
        /// <returns>List of unprocessed arguments</returns>
        public IList<string> Configure(object driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }

            var result = new List<string>();
            Configure(driver, a => result.Add(a));
            return result;
        }

        /// <summary>
        /// Configure the passed driver instance using available arguments
        /// </summary>
        /// <param name="driver">Instance to configure.</param>
        /// <param name="handler">Handler for unrecognised arguments</param>
        public void Configure(object driver, Action<string> handler)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            var options = new Dictionary<string, CommandLineOptionBase>();
            CommandLineSwitch.ConfigureSwitches(driver, options);
            CommandLineParameter.ConfigureParameters(driver, options);

            CommandLineOptionBase option;
            var queue = new Queue<string>(mArguments);
            while (queue.Count > 0)
            {
                var arg = queue.Dequeue();
                if (options.TryGetValue(arg, out option))
                {
                    option.Activate(queue);
                    continue;
                }

                handler(arg);
            }
        }

        private readonly List<string> mArguments = new List<string>();
    }
}
