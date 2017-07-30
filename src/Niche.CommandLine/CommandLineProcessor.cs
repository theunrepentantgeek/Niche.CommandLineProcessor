using System;
using System.Collections.Generic;
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
        private readonly Queue<string> _arguments;

        // List of errors encountered so far
        private readonly List<string> _errors = new List<string>();

        // Help text on available parameters
        private readonly List<string> _optionHelp = new List<string>();

        // A list of all the processors we've used
        private readonly List<IInstanceProcessor> _processors = new List<IInstanceProcessor>();

        // Standard options supplied for all programs
        private readonly StandardOptions _standardOptions = new StandardOptions();

        /// <summary>
        /// Gets the list of arguments not already processed
        /// </summary>
        public IEnumerable<string> Arguments => _arguments;

        /// <summary>
        /// Gets a value indicating whether we have any errors
        /// </summary>
        public bool HasErrors => _errors.Any();

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
        /// Gets a value indicating whether we should should help
        /// </summary>
        public bool ShowHelp => _standardOptions.ShowHelp;

        /// <summary>
        /// Initializes a new instance of the CommandLineProcessor class
        /// </summary>
        /// <param name="arguments">Command line arguments to process</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public CommandLineProcessor(IEnumerable<string> arguments)
        {
            _arguments = new Queue<string>(arguments ?? throw new ArgumentNullException(nameof(arguments)));
            var instanceProcessor = new InstanceProcessor<StandardOptions>(_standardOptions);
            instanceProcessor.Populate(_arguments, _errors);
            _processors.Add(instanceProcessor);
        }

        public CommandLineProcessor<T> Process(T driver)
        {
            var processor = FindLeafProcessor(driver ?? throw new ArgumentNullException(nameof(driver)));
            processor.Populate(_arguments, _errors);
            return this;
        }

        /// <summary>
        /// Populate a driver from the command line and then invoke an action with the result
        /// </summary>
        /// <remarks>The action won't be called if there are any extra arguments or any errors 
        /// during processing.</remarks>
        /// <param name="driver">Driver instance to populate.</param>
        /// <param name="action">Action to trigger if there are no errors.</param>
        /// <returns></returns>
        public CommandLineProcessor<T> Process(T driver, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var processor = FindLeafProcessor(driver ?? throw new ArgumentNullException(nameof(driver)));
            processor.Populate(_arguments, _errors);

            foreach (var a in _arguments)
            {
                _errors.Add($"Did not expect: {a}");
            }

            if (!_errors.Any())
            {
                action(processor.Instance);
            }

            return this;
        }

                {
                    break;
                }

                var newDriver = (T)mode.Activate();
                processor = new InstanceProcessor<T>(newDriver);
                _arguments.Dequeue();
            }

            return processor;
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
    }
}
