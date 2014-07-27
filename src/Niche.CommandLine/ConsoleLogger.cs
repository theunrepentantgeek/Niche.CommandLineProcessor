using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class ConsoleLogger : IConsoleLogger
    {
        /// <summary>
        /// Write an action message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteActionLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.White, ActionMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write a success message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteSuccessLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Green, SuccessMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write an error to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteErrorLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Red, FailureMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write a warning to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteWarningLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Yellow, WarningMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write an information message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteInformationLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.White, InformationMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write a detail message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteDetailLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Gray, DetailMarker, messageTemplate, parameters);
        }

        /// <summary>
        /// Write a debug message to the console
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteDebugLine(string messageTemplate, params string[] parameters)
        {
            WriteMessage(ConsoleColor.DarkGray, Space, messageTemplate, parameters);
        }

        private static void WriteMessage(ConsoleColor color, char prefix, string messageTemplate, string[] parameters)
        {
            var foreground = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.Write(prefix);
                Console.Write(Space);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
            }
            finally
            {
                Console.ForegroundColor = foreground;
            }
        }

        private const char ActionMarker = '#';
        private const char SuccessMarker = '+';
        private const char FailureMarker = 'x';
        private const char WarningMarker = '!';
        private const char InformationMarker = '-';
        private const char DetailMarker = '.';
        private const char Space = ' ';
    }
}
