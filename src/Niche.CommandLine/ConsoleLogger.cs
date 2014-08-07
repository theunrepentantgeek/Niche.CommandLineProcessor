using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Display a heading
        /// </summary>
        /// <param name="message"></param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Niche.CommandLine.ConsoleLogger.WriteMessage(System.ConsoleColor,System.String)")]
        public void Heading(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            WriteMessage(ConsoleColor.DarkGray, new string('-', message.Length + 4));
            WriteMessage(ConsoleColor.White, "  " + message);
            WriteMessage(ConsoleColor.DarkGray, new string('-', message.Length + 4));
        }

        /// <summary>
        /// Write details of an action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Action(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            WriteMessage(ConsoleColor.White, ActionMarker, message);
        }

        /// <summary>
        /// Write details of a successful action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Success(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            WriteMessage(ConsoleColor.Green, SuccessMarker, message);
        }

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Failure(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            } 
            
            WriteMessage(ConsoleColor.Red, FailureMarker, message);
        }

        /// <summary>
        /// Write a warning
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Warning(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            } 
            
            WriteMessage(ConsoleColor.Yellow, WarningMarker, message);
        }

        /// <summary>
        /// Write information
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Information(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            } 
            
            WriteMessage(ConsoleColor.Gray, InformationMarker, message);
        }

        /// <summary>
        /// Write detailed information
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Detail(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            } 
            
            WriteMessage(ConsoleColor.DarkGray, DetailMarker, message);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Niche.CommandLine.ConsoleLogger.WriteMessage(System.ConsoleColor,System.String)")]
        private static void WriteMessage(ConsoleColor color, char prefix, string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            } 
            
            WriteMessage(color, string.Format(CultureInfo.CurrentCulture, "{0} {1}", prefix, message));
        }

        private static void WriteMessage(ConsoleColor color, string message)
        {
            var foreground = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
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
        private const char DetailMarker = ' ';
        private const char Space = ' ';
    }
}
