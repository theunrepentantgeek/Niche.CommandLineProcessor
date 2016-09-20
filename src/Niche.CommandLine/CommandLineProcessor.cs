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
    /// <typeparam name="T">Type of the driver instances to support</typeparam>
    public class CommandLineProcessor<T>
        where T : class
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
            get
            {
                if (!mOptionHelp.Any())
                {
                    CreateHelp();
                }

                return mOptionHelp;
            }
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
        public CommandLineProcessor(IEnumerable<string> arguments, T driver)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var queue = new Queue<string>(arguments);
            var selectedDriver = driver;
            mModes = CommandLineOptionFactory.CreateModes<T>(selectedDriver);
            while (queue.Any())
            {
                var modeName = queue.Peek();
                var mode = mModes.SingleOrDefault(m => m.HasName(modeName));
                if (mode == null)
                {
                    break;
                }

                selectedDriver = (T)mode.Activate();
                mModes = CommandLineOptionFactory.CreateModes<T>(selectedDriver);
                queue.Dequeue();
            }

            mDriver = selectedDriver;

            // Default switches
            mSwitches.AddRange(CommandLineOptionFactory.CreateSwitches(this));

            // Create options for our driver
            mParameters.AddRange(CommandLineOptionFactory.CreateParameters(mDriver));
            mSwitches.AddRange(CommandLineOptionFactory.CreateSwitches(mDriver));

            mArguments.Clear();
            while (queue.Count > 0)
            {
                var activatedSwitch = mSwitches.FirstOrDefault(m => m.TryActivate(queue));
                if (activatedSwitch != null)
                {
                    continue;
                }

                var activatedParameter = mParameters.FirstOrDefault(p => p.TryActivate(queue));
                if (activatedParameter != null)
                {
                    continue;
                }

                var arg = queue.Dequeue();
                if (IsOption(arg))
                {
                    var message = string.Format(CultureInfo.CurrentCulture, "{0}\twas not expected.", arg);
                    mErrors.Add(message);
                    continue;
                }

                mArguments.Add(arg);
            }

            foreach (var o in mSwitches)
            {
                o.Completed(mErrors);
            }

            foreach (var o in mParameters)
            {
                o.Completed(mErrors);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool TryActivate(CommandLineOptionBase option, Queue<string> queue)
        {
            try
            {
                return option.TryActivate(queue);
            }
            // Shouldn't really catch "Exception", but the BCL throws it!
            // See http://www.nichesoftware.co.nz/2013/02/21/so-you-should-never-catch-exception.html 
            catch (Exception ex)
            {
                var message
                    = string.Format(CultureInfo.CurrentCulture, "{0}:\t{1}", queue.Peek(), ex.Message);
                mErrors.Add(message);
            }

            return false;
        }

        [Description("Show this help")]
        public void Help()
        {
            mShowHelp = true;
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

        /// <summary>
        /// Create help text
        /// </summary>
        private void CreateHelp()
        {
            var modeHelp = mModes.SelectMany(m => m.CreateHelp()).OrderBy(l => l).ToList();
            if (modeHelp.Any())
            {
                mOptionHelp.AddRange(modeHelp);
                mOptionHelp.Add(string.Empty);
            }

            var switchHelp = mSwitches.SelectMany(o => o.CreateHelp()).OrderBy(l => l).ToList();
            mOptionHelp.AddRange(switchHelp);

            var parameterHelp = mParameters.SelectMany(o => o.CreateHelp()).OrderBy(l => l).ToList();
            if (parameterHelp.Any())
            {
                mOptionHelp.Add(string.Empty);
                mOptionHelp.AddRange(parameterHelp);
            }
        }

        /// <summary>
        /// Storage for all our switches
        /// </summary>
        private readonly List<CommandLineOptionBase> mSwitches = new List<CommandLineOptionBase>();

        /// <summary>
        /// Storage for all our parameters
        /// </summary>
        private readonly List<CommandLineOptionBase> mParameters = new List<CommandLineOptionBase>();

        /// <summary>
        /// List of unprocessed arguments
        /// </summary>
        private readonly List<string> mArguments = new List<string>();

        private readonly List<string> mErrors = new List<string>();

        private readonly List<string> mOptionHelp = new List<string>();

        private readonly T mDriver;

        private bool mShowHelp;

        private IEnumerable<CommandLineMode> mModes;
    }
}
