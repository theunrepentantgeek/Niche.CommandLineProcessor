using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class CommandLineProcessor
    {
        /// <summary>
        /// Gets the list of arguments not already processed
        /// </summary>
        public IEnumerable<string> Arguments
        {
            get { return mArguments; }
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        /// <param name="arguments"></pparam>
        public CommandLineProcessor(IEnumerable<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

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
            mArguments.Clear();
            while (queue.Count > 0)
            {
                var arg = queue.Dequeue();
                if (options.TryGetValue(arg, out option))
                {
                    option.Activate(queue);
                    continue;
                }

                mArguments.Add(arg);
            }
        }

        private readonly List<string> mArguments = new List<string>();
    }
}
