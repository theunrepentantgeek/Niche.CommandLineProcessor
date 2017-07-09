using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Niche.CommandLine
{
    /// <summary>
    /// Extensions for ILogger
    /// </summary>
    public static class LoggerExtensions
    {
        public static void ConsoleBanner(this ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var assembly = Assembly.GetEntryAssembly();

            var heading = new StringBuilder();

            var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (title != null && !string.IsNullOrWhiteSpace(title.Title))
            {
                heading.Append(title.Title);
            }
            else
            {
                heading.Append(assembly.GetName().Name);
            }

            var version = assembly.GetName().Version;
            if (version != null)
            {
                heading.Append(" v");
                heading.Append(version.ToString(4));
            }

            logger.Heading(heading.ToString());
        }

        /// <summary>
        /// Write details of an action
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Action(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Action(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several actions, using tabs to create a table
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Action(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Action(line);
            }
        }

        /// <summary>
        /// Write details of a successful action
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Success(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Success(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several successful actions, using tabs to create a table
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Success(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Success(line);
            }
        }

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Failure(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Failure(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several failed actions, using tabs to create a table
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Failure(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Failure(line);
            }
        }

        /// <summary>
        /// Write details of an exception
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="exception">Exception with the details to log.</param>
        public static void Failure(this ILogger logger, Exception exception)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            logger.Failure(exception.Message);
            if (exception.Data != null && exception.Data.Count > 0)
            {
                logger.Failure(
                    exception.Data.Cast<DictionaryEntry>()
                    .Select(de => string.Format(CultureInfo.CurrentCulture, "{0}\t{1}", de.Key, de.Value)));
            }

            if (exception.InnerException != null)
            {
                Failure(logger, exception.InnerException);
            }

            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (var e in aggregateException.InnerExceptions)
                {
                    Failure(logger, e);
                }
            }
        }

        /// <summary>
        /// Write a warning
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Warning(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Warning(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write several warnings, using tabs to create a table
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Warning(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Warning(line);
            }
        }

        /// <summary>
        /// Write information (detail about an action)
        /// </summary>
        /// <param name="logger">Logger to which we write our message.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Information(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Information(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write several information messages, using tabs to create a table.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Information(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Information(line);
            }
        }

        /// <summary>
        /// Write detailed information
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Detail(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Detail(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write several detail messages, using tabs to create a table.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Detail(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Detail(line);
            }
        }

        /// <summary>
        /// Write debug information
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Debug(this ILogger logger, string messageTemplate, params object[] parameters)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Debug(string.Format(CultureInfo.CurrentCulture, messageTemplate, parameters));
        }

        /// <summary>
        /// Write several debug messages, using tabs to create a table.
        /// </summary>
        /// <param name="logger">Logger to use for logging.</param>
        /// <param name="messages">The messages to write.</param>
        public static void Debug(this ILogger logger, IEnumerable<string> messages)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            foreach (var line in Tablefy(messages))
            {
                logger.Debug(line);
            }
        }

        /// <summary>
        /// Format a series of lines as a "table", based on use of tabs to separate columns
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tablefy")]
        public static IEnumerable<string> Tablefy(IEnumerable<string> lines)
        {
            var parts = lines.Select(l => l.Split('\t')).ToList();
            var columns = parts.Max(c => c.Length);

            var formats = new List<string>();

            // Create our set of format strings
            for (int c = 0; c < columns; c++)
            {
                var width = parts.Where(p => p.Length > c).Max(p => p[c].Length);
                var f = string.Format(CultureInfo.CurrentCulture, "{{0,-{0}}}{1}", width, "   ");
                formats.Add(f);
            }

            // Create the result
            var result = new List<string>();
            foreach (var line in parts)
            {
                var builder = new StringBuilder();
                var index = 0;
                foreach (var c in line)
                {
                    builder.Append(string.Format(CultureInfo.CurrentCulture, formats[index++], c));
                }

                result.Add(builder.ToString().TrimEnd());
            }

            return result;
        }

    }
}
