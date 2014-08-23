using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
        /// Gets a value indicating whether we should should help
        /// </summary>
        public bool ShowHelp
        {
            get { return mShowHelp; }
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
        public IEnumerable<string> OptionHelp
        {
            get { return mOptionHelp; }
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
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public CommandLineProcessor(IEnumerable<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            mArguments = arguments.ToList();
            mDriver = new T();

            var options = new Dictionary<string, CommandLineOptionBase>();

            LoadOptions(options);

            mOptionHelp.Sort();

            CommandLineOptionBase option;
            var queue = new Queue<string>(mArguments);
            mArguments.Clear();
            while (queue.Count > 0)
            {
                var arg = queue.Dequeue();
                if (options.TryGetValue(arg, out option))
                {
                    Activate(option, queue, arg);
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

            foreach(var o in mOptions)
            {
                o.Completed(mErrors);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Activate(CommandLineOptionBase option, Queue<string> queue, string arg)
        {
            try
            {
                option.Activate(queue);
            }
            // Shouldn't really catch exception, but the BCL throws it!
            // See http://www.nichesoftware.co.nz/2013/02/21/so-you-should-never-catch-exception.html 
            catch (Exception ex)
            {
                var message
                    = string.Format(CultureInfo.CurrentCulture, "{0}:\t{1}", arg, ex.Message);
                mErrors.Add(message);
            }
        }

        [Description("Show help")]
        public void Help()
        {
            mShowHelp = true;
        }

        private void LoadOptions(Dictionary<string, CommandLineOptionBase> options)
        {
            // Default switches
            mOptions.AddRange(CommandLineOptionFactory.CreateSwitches(this));

            // Create options for our driver
            mOptions.AddRange(CommandLineOptionFactory.CreateParameters(mDriver));
            mOptions.AddRange(CommandLineOptionFactory.CreateSwitches(mDriver));

            // Populate as flags
            foreach (var s in mOptions)
            {
                s.AddOptionsTo(options);
                s.AddHelpTo(mOptionHelp);
            }
        }

        /// <summary>
        /// Test to see if the passed argument is an option
        /// </summary>
        /// <param name="argument">Argument to test</param>
        /// <returns>True if the argument is an option, false otherwise.</returns>
        private static bool IsOption(string argument)
        {
            return argument.StartsWith("-", StringComparison.Ordinal)
                || argument.StartsWith("/", StringComparison.Ordinal);
        }

        private readonly List<CommandLineOptionBase> mOptions = new List<CommandLineOptionBase>();

        private readonly List<string> mArguments = new List<string>();

        private readonly List<string> mErrors = new List<string>();

        private readonly List<string> mOptionHelp = new List<string>();

        private readonly T mDriver;

        private bool mShowHelp;
    }
}
