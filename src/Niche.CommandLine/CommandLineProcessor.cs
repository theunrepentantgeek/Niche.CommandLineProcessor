using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// Gets a value indicating whether we have any errors
        /// </summary>
        public bool HasErrors
        {
            get { return mErrors.Any(); }
        }

        /// <summary>
        /// Gets a list of errors already encountered
        /// </summary>
        public IEnumerable<string> Errors
        {
            get { return mErrors; }
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
            
            // Create Switches
            foreach(var s in CommandLineSwitch.CreateSwitches(driver))
            {
                s.AddOptionsTo(options);
            }

            // Create Parameters
            foreach(var p in CommandLineParameter.CreateParameters(driver))
            {
                p.AddOptionsTo(options);
            }

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

                if (IsOption(arg))
                {
                    var message = string.Format(CultureInfo.CurrentCulture, "{0}\twas not expected.", arg);
                    mErrors.Add(message);
                    continue;
                }

                mArguments.Add(arg);
            }
        }

        /// <summary>
        /// Test to see if the passed argument is an option
        /// </summary>
        /// <param name="argument">Argument to test</param>
        /// <returns>True if the argument is an option, false otherwise.</returns>
        private bool IsOption(string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }

            return argument.StartsWith("-", StringComparison.Ordinal)
                || argument.StartsWith("/", StringComparison.Ordinal);
        }

        private readonly List<string> mArguments = new List<string>();

        private readonly List<string> mErrors = new List<string>();

        private readonly List<string> mHelp = new List<string>();
    }
}
