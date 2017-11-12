using System;
using System.Collections.Generic;

namespace Niche.CommandLine
{
    /// <summary>
    /// Syntax for invoking an action after an options instance has been parsed
    /// </summary>
    /// <typeparam name="T">Type of options instance just configured.</typeparam>
    public interface ICommandLineExecuteFuncSyntax<out T>
    {
        /// <summary>
        /// Do something useful with a properly configured option
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <remarks>Returns the int returned by <paramref name="func"/>.</remarks>
        int Execute(Func<T, int> func);

        /// <summary>
        /// Do something useful with a properly configured option and any extra arguments provided
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <remarks>Returns the int returned by <paramref name="func"/>.</remarks>
        int Execute(Func<T, IEnumerable<string>, int> func);
    }

    /// <summary>
    /// An implementation of <see cref="ICommandLineExecuteFuncSyntax{T}"/> that does nothing
    /// </summary>
    /// <typeparam name="T">Type of instance that was configured.</typeparam>
    public class NullCommandLineExecuteFuncSyntax<T> : ICommandLineExecuteFuncSyntax<T>
    {
        private readonly int _exitCode;

        /// <summary>
        /// Initializes a new instance of <see cref="NullCommandLineExecuteFuncSyntax{T}"/>
        /// </summary>
        /// <param name="exitCode">Exit code to return.</param>
        public NullCommandLineExecuteFuncSyntax(int exitCode)
        {
            _exitCode = exitCode;
        }

        /// <summary>
        /// Do nothing at all with a properly configured option
        /// </summary>
        /// <param name="func">Function to ignore.</param>
        /// <remarks>Returns the int returned by <see cref="_exitCode"/>.</remarks>
        public int Execute(Func<T, int> func)
        {
            return _exitCode;
        }

        /// <summary>
        /// Do nothing useful with a properly configured option and any extra arguments provided
        /// </summary>
        /// <param name="func">Function to not invoke.</param>
        /// <remarks>Returns the int returned by <paramref name="func"/>.</remarks>
        public int Execute(Func<T, IEnumerable<string>, int> func)
        {
            return _exitCode;
        }
    }
}
