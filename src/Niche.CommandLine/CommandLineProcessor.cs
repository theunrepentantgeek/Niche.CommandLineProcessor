using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility class used to parse command line parameters and populate a driver instance
    /// </summary>
    /// <typeparam name="T">Type of the driver instances to support</typeparam>
    public class CommandLineProcessor<T>
        where T : class
    {
        // List of unprocessed arguments
        private readonly List<string> _arguments = new List<string>();

        // List of errors encountered so far
        private readonly List<string> _errors = new List<string>();

        // Help text on available parameters
        private readonly List<string> _optionHelp = new List<string>();

        // The driver object we are configuring
        private readonly T _driver;

        // A flag for whether to show help
        private bool _showHelp;

        // A list of all the modes we support
        private readonly IEnumerable<CommandLineMode> _modes;

        /// <summary>
        /// Gets the list of arguments not already processed
        /// </summary>
        public IEnumerable<string> Arguments => _arguments;

        /// <summary>
        /// Gets a value indicating whether we have any errors
        /// </summary>
        public bool HasErrors => _errors.Any();

        /// <summary>
        /// Gets a value indicating whether we should should help
        /// </summary>
        public bool ShowHelp => _showHelp;

        /// <summary>
        /// Gets the sequence of the errors already encountered
        /// </summary>
        public IEnumerable<string> Errors => _errors;

        /// <summary>
        /// Gets a list of help text for display
        /// </summary>
        public IEnumerable<string> OptionHelp
        {
            get
            {
                if (!_optionHelp.Any())
                {
                    CreateHelp();
                }

                return _optionHelp;
            }
        }

        /// <summary>
        /// Gets a reference to the driver instance we've configured from the command line
        /// </summary>
        public T Driver => _driver;

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        /// <param name="arguments">Command line arguments to process</param>
        /// <param name="driver">Driver instance to configure from the command line</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public CommandLineProcessor(IEnumerable<string> arguments, T driver)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var queue = new Queue<string>(arguments);
            var selectedDriver = driver;
            _modes = CommandLineOptionFactory.CreateModes(selectedDriver);
            while (queue.Any())
            {
                var modeName = queue.Peek();
                var mode = _modes.SingleOrDefault(m => m.HasName(modeName));
                if (mode == null)
                {
                    break;
                }

                selectedDriver = (T)mode.Activate();
                _modes = CommandLineOptionFactory.CreateModes(selectedDriver);
                queue.Dequeue();
            }

            _driver = selectedDriver;

            // Default switches
            _switches.AddRange(CommandLineOptionFactory.CreateSwitches(this));

            // Create options for our driver
            _parameters.AddRange(CommandLineOptionFactory.CreateParameters(_driver));
            _switches.AddRange(CommandLineOptionFactory.CreateSwitches(_driver));

            _arguments.Clear();
            while (queue.Count > 0)
            {
                var activatedSwitch = _switches.FirstOrDefault(m => m.TryActivate(queue));
                if (activatedSwitch != null)
                {
                    continue;
                }

                var activatedParameter = _parameters.FirstOrDefault(p => p.TryActivate(queue));
                if (activatedParameter != null)
                {
                    continue;
                }

                var arg = queue.Dequeue();
                if (IsOption(arg))
                {
                    var message = string.Format(CultureInfo.CurrentCulture, "{0}\twas not expected.", arg);
                    _errors.Add(message);
                    continue;
                }

                _arguments.Add(arg);
            }

            foreach (var o in _switches)
            {
                o.Completed(_errors);
            }

            foreach (var o in _parameters)
            {
                o.Completed(_errors);
            }
        }

        [Description("Show this help")]
        public void Help()
        {
            _showHelp = true;
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
            var modeHelp = _modes.SelectMany(m => m.CreateHelp()).OrderBy(l => l).ToList();
            if (modeHelp.Any())
            {
                _optionHelp.AddRange(modeHelp);
                _optionHelp.Add(string.Empty);
            }

            var switchHelp = _switches.SelectMany(o => o.CreateHelp()).OrderBy(l => l).ToList();
            _optionHelp.AddRange(switchHelp);

            var parameterHelp = _parameters.SelectMany(o => o.CreateHelp()).OrderBy(l => l).ToList();
            if (parameterHelp.Any())
            {
                _optionHelp.Add(string.Empty);
                _optionHelp.AddRange(parameterHelp);
            }
        }

        /// <summary>
        /// Storage for all our switches
        /// </summary>
        private readonly List<CommandLineOptionBase> _switches = new List<CommandLineOptionBase>();

        /// <summary>
        /// Storage for all our parameters
        /// </summary>
        private readonly List<CommandLineOptionBase> _parameters = new List<CommandLineOptionBase>();
    }
}
