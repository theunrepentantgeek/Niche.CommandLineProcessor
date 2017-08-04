using System;
using System.Collections.Generic;

namespace Niche.CommandLine
{
    internal class CommandLineExecuteActionSyntax<T> : ICommandLineExecuteActionSyntax<T>
        where T : class
    {
        // Reference to the instance we've populated, ready for use
        private readonly T _options;

        // Reference to a set of errors to popu late if an exception occurs
        private readonly ICollection<string> _errorsReference;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecuteActionSyntax{T}"/> class
        /// </summary>
        /// <remarks>
        /// This class provides an implementation of <see cref="ICommandLineExecuteActionSyntax{T}"/> allowing the user 
        /// to do something with a freshly configured instance after calling <see cref="CommandLineProcessor.ParseGlobal{T}(T)"/>.
        /// </remarks>
        /// <param name="options">Instance that has been configured</param>
        /// <param name="errorsReference">Reference to a collection of errors to populate if something goes wrong.</param>
        public CommandLineExecuteActionSyntax(T options, ICollection<string> errorsReference)
        {
            _errorsReference = errorsReference ?? throw new ArgumentNullException(nameof(errorsReference));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Do something useful with a properly configured option
        /// </summary>
        /// <param name="action">Action to invoke.</param>
        public void Execute(Action<T> action)
        {
            if(action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action(_options);
            }
            catch (Exception e)
            {
                foreach (var message in e.AsStrings())
                {
                    _errorsReference.Add(message);
                }
            }
        }
    }
}