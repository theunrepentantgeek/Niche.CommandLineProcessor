using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Write details of an action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Action(string message)
        {
            WriteMessage(ConsoleColor.White, ActionMarker, message);
        }

        /// <summary>
        /// Write details of a successful action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Success(string message)
        {
            WriteMessage(ConsoleColor.Green, SuccessMarker, message);
        }

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Failure(string message)
        {
            WriteMessage(ConsoleColor.Red, FailureMarker, message);
        }

        /// <summary>
        /// Write a warning
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Warning(string message)
        {
            WriteMessage(ConsoleColor.Yellow, WarningMarker, message);
        }

        /// <summary>
        /// Write information
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Information(string message)
        {
            WriteMessage(ConsoleColor.Gray, InformationMarker, message);
        }

        /// <summary>
        /// Write detailed information
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Detail(string message)
        {
            WriteMessage(ConsoleColor.DarkGray, DetailMarker, message);
        }

        private static void WriteMessage(ConsoleColor color, char prefix, string message)
        {
            var foreground = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.Write(prefix);
                Console.Write(Space);
                Console.WriteLine(message);
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
