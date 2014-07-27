using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public interface IConsoleLogger
    {
        /// <summary>
        /// Write an action message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteActionLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write a success message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteSuccessLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write an error to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteErrorLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write a warning to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteWarningLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write an information message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteInformationLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write a detail message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteDetailLine(string messageTemplate, params string[] parameters);

        /// <summary>
        /// Write a debug message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        void WriteDebugLine(string messageTemplate, params string[] parameters);
    }
}
