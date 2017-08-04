using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Niche.CommandLine
{
    public class ConsoleLogger : ILogger
    {
        private readonly ConsoleLoggerOptions _options;

        private readonly string _successMarker = "+";
        private readonly string _failureMarker = "x";

        private readonly string _actionMarker = ">";
        private readonly string _warningMarker = "!";

        private readonly string _informationMarker = "-";
        private readonly string _detailMarker = " ";
        private readonly string _debugMarker = ".";

        /// <summary>
        /// Initializes a new instance of the ConsoleLogger class
        /// </summary>
        /// <param name="options"></param>
        public ConsoleLogger(ConsoleLoggerOptions options = ConsoleLoggerOptions.None)
        {
            _options = options;

            if (_options.HasFlag(ConsoleLoggerOptions.UseLabels))
            {
                _successMarker = "[succ]";
                _failureMarker = "[fail]";
                _actionMarker = "[actn]";
                _warningMarker = "[warn]";
                _informationMarker = "[info]";
                _detailMarker = "[detl]";
                _debugMarker = "[dbug]";
            }

            if (options.HasFlag(ConsoleLoggerOptions.DisplayBanner))
            {
                this.ConsoleBanner();
            }
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

            WriteMessage(ConsoleColor.White, _actionMarker, message);
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

            WriteMessage(ConsoleColor.DarkGreen, _successMarker, message);
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

            WriteMessage(ConsoleColor.Red, _failureMarker, message);
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

            WriteMessage(ConsoleColor.Yellow, _warningMarker, message);
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

            WriteMessage(ConsoleColor.White, _informationMarker, message);
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

            WriteMessage(ConsoleColor.Gray, _detailMarker, message);
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

            WriteMessage(ConsoleColor.DarkGray, _debugMarker, message);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Niche.CommandLine.ConsoleLogger.WriteMessage(System.ConsoleColor,System.String)")]
        private void WriteMessage(ConsoleColor color, string prefix, string message)
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
    }

    /// <summary>
    /// Options for configuration of a ConsoleLogger
    /// </summary>
    [Flags]
    public enum ConsoleLoggerOptions
    {
        // Do nothing special
        None = 0,

        // Show a timestamp at the start of each line
        ShowTime = 1,

        // Use labels to show the severity of each line instead of symbols
        UseLabels = 2,

        // Show a banner on startup
        DisplayBanner = 4
    }
}
