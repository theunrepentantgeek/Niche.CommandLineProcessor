using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Niche.CommandLine
{
    /// <summary>
    /// Utility class used to parse command line parameters and populate a options instance
    /// </summary>
    public class CommandLineProcessor
    {
        // List of unprocessed arguments
        private readonly Queue<string> _arguments;

        // List of errors encountered so far
        private readonly List<string> _errors = new List<string>();

        // Help text on available parameters
        private readonly Lazy<List<string>> _optionHelp;

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
        public IEnumerable<string> OptionHelp => _optionHelp.Value;

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
            instanceProcessor.Parse(_arguments, _errors);
            _processors.Add(instanceProcessor);
            _optionHelp = new Lazy<List<string>>(CreateHelp, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Configure a options instance, populating it from the command line
        /// </summary>
        /// <typeparam name="T">Type of options instance to configure.</typeparam>
        /// <remarks>Any command line arguments that address the options will be consumed; the 
        /// remaining arguments will be retained in original order.</remarks>
        /// <returns>Instance of <see cref="ICommandLineExecuteActionSyntax{T}"/>.</returns>
        public ICommandLineExecuteActionSyntax<T> ParseGlobal<T>()
            where T : class, new()
        {
            return ParseGlobal(new T());
        }

        /// <summary>
        /// Configure some global options from the command line
        /// </summary>
        /// <typeparam name="T">Type of options instance to configure.</typeparam>
        /// <remarks>Any command line arguments that address the options will be consumed; the 
        /// remaining arguments will be retained in original order.</remarks>
        /// <param name="options">Driver instance to populate</param>
        /// <returns>Instance of <see cref="ICommandLineExecuteActionSyntax{T}"/>.</returns>
        public ICommandLineExecuteActionSyntax<T> ParseGlobal<T>(T options)
            where T : class
        {
            var processor = FindLeafProcessor(options ?? throw new ArgumentNullException(nameof(options)));
            processor.Parse(_arguments, _errors);
            _processors.Add(processor);

            if (_errors.Any())
            {
                return new NullCommandLineExecuteActionSyntax<T>();
            }

            return new CommandLineExecuteActionSyntax<T>(options, _errors);
        }

        /// <summary>
        /// Configure some program options from the command line
        /// </summary>
        /// <typeparam name="T">Type of options instance to configure.</typeparam>
        /// <remarks>Any command line arguments that address the options will be consumed; the 
        /// remaining arguments will be retained in original order.</remarks>
        /// <returns>Instance of <see cref="ICommandLineExecuteActionSyntax{T}"/>.</returns>
        public ICommandLineExecuteFuncSyntax<T> Parse<T>()
            where T : class, new()
        {
            return Parse(new T());
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

            if (_errors.Any())
            {
                return new NullCommandLineExecuteFuncSyntax<T>(-1);
            }

            return new CommandLineExecuteFuncSyntax<T>(options, _arguments.ToList(), _errors, -1);
        }

        /// <summary>
        /// Display help if it's requested
        /// </summary>
        /// <param name="displayAction"></param>
        /// <returns></returns>
        public CommandLineProcessor WithHelpAction(Action<IEnumerable<string>> displayAction)
        {
            var action = displayAction ?? throw new ArgumentNullException(nameof(displayAction));
            if (ShowHelp)
            {
                action(OptionHelp);
            }

            return this;
        }

        /// <summary>
        /// Display help if it's requested
        /// </summary>
        /// <param name="displayAction"></param>
        /// <returns></returns>
        public CommandLineProcessor WithErrorAction(Action<IEnumerable<string>> displayAction)
        {
            var action = displayAction ?? throw new ArgumentNullException(nameof(displayAction));
            if (Errors.Any())
            {
                action(OptionHelp);
            }

            return this;
        }

        private InstanceProcessor<T> FindLeafProcessor<T>(T driver)
            where T : class
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
        /// Create help text
        /// </summary>
        private List<string> CreateHelp()
        {
            var helpText = new List<string>();

            var modeHelp =
                from processor in _processors
                from commandMode in processor.Modes
                from line in commandMode.CreateHelp()
                orderby line
                select line;
            AddHelp(modeHelp);

            var switchHelp = (
                from processor in _processors
                from commandSwitch in processor.Switches
                from line in commandSwitch.CreateHelp()
                orderby line
                select line).ToList();
            AddHelp(switchHelp);

            var parameterHelp = (
                from processor in _processors
                from commandParameter in processor.Parameters
                from line in commandParameter.CreateHelp()
                orderby line
                select line).ToList();
            AddHelp(parameterHelp);

            return helpText;

            void AddHelp(IEnumerable<string> lines)
            {
                var text = lines.ToList();
                if (helpText.Any() && text.Any())
                {
                    helpText.Add(string.Empty);
                }

                helpText.AddRange(text);
            }
        }
    }
}
