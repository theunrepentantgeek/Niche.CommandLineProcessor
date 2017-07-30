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
        private readonly List<string> _arguments = new List<string>();

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


            }



        /// <summary>
        /// Configure some program options from the command line
        /// </summary>
        /// <typeparam name="T">Type of options instance to configure.</typeparam>
        /// <remarks>Any command line arguments that address the options will be consumed; the 
        /// remaining arguments will be retained in original order.</remarks>
        /// <param name="options">Driver instance to populate</param>
        /// <returns>Instance of <see cref="ICommandLineExecuteActionSyntax{T}"/>.</returns>
        public ICommandLineExecuteFuncSyntax<T> Parse<T>(T options)
        where T : class
        {
            var processor = FindLeafProcessor(options ?? throw new ArgumentNullException(nameof(options)));
            processor.Parse(_arguments, _errors);
            _processors.Add(processor);

            _arguments.Clear();
            while (queue.Count > 0)
            {


        public CommandLineProcessor<T> Process(T driver, Action<T, IEnumerable<string>> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var processor = FindLeafProcessor(driver ?? throw new ArgumentNullException(nameof(driver)));
            processor.Populate(_arguments, _errors);

            foreach (var a in _arguments.Where(IsOption))
            {
                _errors.Add($"Did not expect: {a}");
            }

            if (!_errors.Any())
            {
                action(processor.Instance, _arguments);
            }

            return this;
        }

        private InstanceProcessor<T> FindLeafProcessor(T driver)
        {
            var processor = new InstanceProcessor<T>(driver);
            while (_arguments.Any())
            {
                var modeName = _arguments.Peek();
                var mode = processor.Modes.SingleOrDefault(m => m.HasName(modeName));
                if (mode == null)
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
