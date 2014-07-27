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
        public void Configure(object driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }

            var options = new Dictionary<string, CommandLineOptionBase>();
            CommandLineSwitch.ConfigureSwitches(driver, options);
            CommandLineParameter.ConfigureParameters(driver, options);

            CommandLineOptionBase option;
            var queue = new Queue<string>(mArguments);
            while (queue.Count > 0)
            {
                var a = queue.Dequeue();
                if (options.TryGetValue(a, out option))
                {
                    option.Activate(queue);
                    continue;
                }
            }
        }

        private readonly List<string> mArguments = new List<string>();
    }
}
