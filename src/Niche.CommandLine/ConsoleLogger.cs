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
        private readonly ConsoleLoggerOptions _options;

        /// <summary>
        /// Initializes a new instance of the ConsoleLogger class
        /// </summary>
        /// <param name="options"></param>
        public ConsoleLogger(ConsoleLoggerOptions options = ConsoleLoggerOptions.None)
        {
            _options = options;
        }

        /// <summary>
        /// Display a heading
        /// </summary>
        /// <param name="message"></param>
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "Niche.CommandLine.ConsoleLogger.WriteMessage(System.ConsoleColor,System.String)")]
        public void Heading(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
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
                throw new ArgumentNullException(nameof(message));
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
                throw new ArgumentNullException(nameof(message));
            }

            WriteMessage(ConsoleColor.DarkGreen, SuccessMarker, message);
        }

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Failure(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
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
                throw new ArgumentNullException(nameof(message));
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
                throw new ArgumentNullException(nameof(message));
            }

            WriteMessage(ConsoleColor.White, InformationMarker, message);
        }

        /// <summary>
        /// Write detailed information
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Detail(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            WriteMessage(ConsoleColor.Gray, DetailMarker, message);
        }

        /// <summary>
        /// Write debug information
        /// </summary>
        /// <param name="message">The message to write</param>
        public void Debug(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            WriteMessage(ConsoleColor.DarkGray, DebugMarker, message);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Niche.CommandLine.ConsoleLogger.WriteMessage(System.ConsoleColor,System.String)")]
        private void WriteMessage(ConsoleColor color, char prefix, string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            WriteMessage(color, string.Format(CultureInfo.CurrentCulture, "{0} {1}", prefix, message));
        }

        private void WriteMessage(ConsoleColor color, string message)
        {
            var foreground = Console.ForegroundColor;
            try
            {
                if (_options.HasFlag(ConsoleLoggerOptions.ShowTime))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"[{DateTimeOffset.Now:HH:mm:ss.fff}] ");
                }

                Console.ForegroundColor = color;
                Console.WriteLine(message);
            }
            finally
            {
                Console.ForegroundColor = foreground;
            }
        }

        private const char SuccessMarker = '+';
        private const char FailureMarker = 'x';

        private const char ActionMarker = '>';
        private const char WarningMarker = '!';

        private const char InformationMarker = '-';
        private const char DetailMarker = ' ';
        private const char DebugMarker = '.';
        private const char Space = ' ';
    }

    /// <summary>
    /// Options for configuration of a ConsoleLogger
    /// </summary>
    public enum ConsoleLoggerOptions
    {
        // Do nothing special
        None = 0,

        // Show a timestamp at the start of each line
        ShowTime = 1
    }
}
