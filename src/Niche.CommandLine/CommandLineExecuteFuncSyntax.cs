using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Implementation of <see cref="ICommandLineExecuteFuncSyntax{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the configured options instance.</typeparam>
    public class CommandLineExecuteFuncSyntax<T> : ICommandLineExecuteFuncSyntax<T>
        where T : class
    {
        // Reference to the instance we've populated, ready for use
        private readonly T _options;

        // A (possibly empty) set of arguments available for use
        private readonly ICollection<string> _arguments;

        // Reference to a set of errors to populate if an exception occurs
        private readonly ICollection<string> _errorsReference;

        private readonly int _errorExitCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecuteFuncSyntax{T}"/> class
        /// </summary>
        /// <param name="options">Instance that has been configured</param>
        /// <param name="arguments"></param>
        /// <param name="errorsReference">Reference to a collection of errors to populate if something goes wrong.</param>
        /// <param name="errorExitCode">Exit code to use if something goes wrong.</param>
        public CommandLineExecuteFuncSyntax(
            T options,
            ICollection<string> arguments,
            ICollection<string> errorsReference,
            int errorExitCode)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            _errorsReference = errorsReference ?? throw new ArgumentNullException(nameof(errorsReference));
            _errorExitCode = errorExitCode;
        }

        /// <summary>
        /// Do something useful with a properly configured option
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <returns>Returns the int returned by <paramref name="func"/>.</returns>
        public int Execute(Func<T, int> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            // We may have extra arguments but the user doesn't want to know
            foreach (var a in _arguments)
            {
                _errorsReference.Add($"Unexpected argument: {a}");
            }

            if (_arguments.Any())
            {
                return _errorExitCode;
            }

            try
            {
                return func(_options);
            }
            catch (Exception e)
            {
                foreach (var message in e.AsStrings())
                {
                    _errorsReference.Add(message);
                }

                return _errorExitCode;
            }
        }

        /// <summary>
        /// Do something useful with a properly configured option
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <returns>Returns the int returned by <paramref name="func"/>.</returns>
        public async Task<int> ExecuteAsync(Func<T, Task<int>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            // We may have extra arguments but the user doesn't want to know
            foreach (var a in _arguments)
            {
                _errorsReference.Add($"Unexpected argument: {a}");
            }

            if (_arguments.Any())
            {
                return _errorExitCode;
            }

            try
            {
                return await func(_options);
            }
            catch (Exception e)
            {
                foreach (var message in e.AsStrings())
                {
                    _errorsReference.Add(message);
                }

                return _errorExitCode;
            }
        }

        /// <summary>
        /// Do something useful with a properly configured option and any extra arguments provided
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <remarks>Returns the int returned by <paramref name="func"/>.</remarks>
        public int Execute(Func<T, IEnumerable<string>, int> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            // Create errors for any extra options
            foreach (var a in _arguments.Where(IsOption))
            {
                _errorsReference.Add($"Unexpected option: {a}");
            }

            if (_errorsReference.Any())
            {
                return _errorExitCode;
            }

            try
            {
                return func(_options, _arguments);
            }
            catch (Exception e)
            {
                foreach (var message in e.AsStrings())
                {
                    _errorsReference.Add(message);
                }

                return _errorExitCode;
            }
        }

        /// <summary>
        /// Do something useful with a properly configured option and any extra arguments provided
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <remarks>Returns the int returned by <paramref name="func"/>.</remarks>
        public async Task<int> ExecuteAsync(Func<T, IEnumerable<string>, Task<int>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            // Create errors for any extra options
            foreach (var a in _arguments.Where(IsOption))
            {
                _errorsReference.Add($"Unexpected option: {a}");
            }

            if (_errorsReference.Any())
            {
                return _errorExitCode;
            }

            try
            {
                return await func(_options, _arguments);
            }
            catch (Exception e)
            {
                foreach (var message in e.AsStrings())
                {
                    _errorsReference.Add(message);
                }

                return _errorExitCode;
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
    }
}
