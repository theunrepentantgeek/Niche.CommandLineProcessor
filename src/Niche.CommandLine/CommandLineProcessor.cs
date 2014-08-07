using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility class used to parse command line parameters and populate a driver instance
    /// </summary>
    /// <typeparam name="T">Type of the driver instance to use</typeparam>
    public class CommandLineProcessor<T>
        where T : new()
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
        /// Gets the sequence of the errors already encountered
        /// </summary>
        public IEnumerable<string> Errors
        {
            get { return mErrors; }
        }

        /// <summary>
        /// Gets a list of help text for display
        /// </summary>
        public IEnumerable<string> Help
        {
            get { return mHelp; }
        }

        /// <summary>
        /// Gets a reference to the driver instance we've configured from the command line
        /// </summary>
        public T Driver
        {
            get { return mDriver; }
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        /// <param name="arguments"></param>
        public CommandLineProcessor(IEnumerable<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            mArguments = arguments.ToList();
            mDriver = new T();

            var options = new Dictionary<string, CommandLineOptionBase>();

            CreateSwitches(options);
            CreateParameters(options);

            mHelp.Sort();

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

        private void CreateParameters(Dictionary<string, CommandLineOptionBase> options)
        {
            // Create Parameters
            foreach (var p in CommandLineParameter.CreateParameters(mDriver))
            {
                p.AddOptionsTo(options);
                p.AddHelpTo(mHelp);
            }
        }

        private void CreateSwitches(Dictionary<string, CommandLineOptionBase> options)
        {
            // Create Switches
            foreach (var s in CommandLineSwitch.CreateSwitches(mDriver))
            {
                s.AddOptionsTo(options);
                s.AddHelpTo(mHelp);
            }
        }

        /// <summary>
        /// Test to see if the passed argument is an option
        /// </summary>
        /// <param name="argument">Argument to test</param>
        /// <returns>True if the argument is an option, false otherwise.</returns>
        private static bool IsOption(string argument)
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

        private readonly T mDriver;
    }
}
