using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class ConsoleLogger : IConsoleLogger
    {
        /// <summary>
        /// Write a success message to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteSuccessLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Green, "+", template, parameters);
        }

        /// <summary>
        /// Write an error to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteErrorLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Red, "x", template, parameters);
        }

        /// <summary>
        /// Write a warning to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteWarningLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Yellow, "!", template, parameters);
        }

        /// <summary>
        /// Write an information message to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteInformationLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.White, "-", template, parameters);
        }

        /// <summary>
        /// Write a detail message to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteDetailLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.Gray, " ", template, parameters);
        }

        /// <summary>
        /// Write a debug message to the console
        /// </summary>
        /// <param name="template">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public void WriteDebugLine(string template, params string[] parameters)
        {
            WriteMessage(ConsoleColor.DarkGray, " ", template, parameters);
        }

        private void WriteMessage(ConsoleColor color, string prefix, string template, string[] parameters)
        {
            var foreground = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                Console.Write(prefix);
                Console.Write(" ");
            }

            Console.WriteLine(string.Format(template, parameters));
        }
    }
}
