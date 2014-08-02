using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    /// <summary>
    /// Extensions for ILogger
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Write details of an action
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Action(this ILogger logger, string messageTemplate, params string[] parameters)
        {
            logger.Action(string.Format(messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several actions, using tabs to create a table
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        public static void Action(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (var line in Tablefy(messages))
            {
                logger.Action(line);
            }
        }

        /// <summary>
        /// Write details of a successful action
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Success(this ILogger logger, string messageTemplate, params string[] parameters)
        {
            logger.Success(string.Format(messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several successful actions, using tabs to create a table
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        public static void Success(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (var line in Tablefy(messages))
            {
                logger.Success(line);
            }
        }

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Failure(this ILogger logger, string messageTemplate, params string[] parameters)
        {
            logger.Failure(string.Format(messageTemplate, parameters));
        }

        /// <summary>
        /// Write details of several failed actions, using tabs to create a table
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        public static void Failure(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (var line in Tablefy(messages))
            {
                logger.Failure(line);
            }
        }

        /// <summary>
        /// Write a warning
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Warning(this ILogger logger, string messageTemplate, params string[] parameters)
        {
            logger.Warning(string.Format(messageTemplate, parameters));
        }

        /// <summary>
        /// Write several warnings, using tabs to create a table
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        public static void Warning(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (var line in Tablefy(messages))
            {
                logger.Warning(line);
            }
        }

        /// <summary>
        /// Write information (detail about an action)
        /// </summary>
        /// <param name="messageTemplate">Template for the message to write.</param>
        /// <param name="parameters">Parameters to substitute in the template.</param>
        public static void Information(this ILogger logger, string messageTemplate, params string[] parameters)
        {
            logger.Information(string.Format(messageTemplate, parameters));
        }

        /// <summary>
        /// Write pieces of information
        /// </summary>
        /// <param name="messages">The messages to write.</param>
        public static void Information(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (var line in Tablefy(messages))
            {
                logger.Information(line);
            }
        }

        /// <summary>
        /// Format a series of lines as a "table", based on use of tabs to separate columns
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static IEnumerable<string> Tablefy(IEnumerable<string> lines)
        {
            var parts = lines.Select(l => l.Split('\t')).ToList();
            var columns = parts.Max(c => c.Length);
            
            // Calculate Widths
            var widths = new int[columns];
            foreach(var line in parts)
            {
                int index = 0;
                foreach(var cell in line)
                {
                    widths[index] = Math.Max(widths[index], cell.Length+1);
                    index++;
                }
            }

            // Create a format string
            var f = new StringBuilder();
            int column = 0;
            foreach(var w in widths)
            {
                f.AppendFormat("{{{0},-{1}}}", column++, w);
            }
            var format = f.ToString();

            // Create the result
            var result = new List<string>();
            foreach(var line in parts)
            {
                result.Add(string.Format(format, line).TrimEnd());
            }

            return result;
        }

    }
}
