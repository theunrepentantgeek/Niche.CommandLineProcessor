using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine
{
    public interface ILogger
    {
        /// <summary>
        /// Display a heading
        /// </summary>
        /// <param name="heading"></param>
        void Heading(string heading);

        /// <summary>
        /// Write details of an action
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Action(string message);

        /// <summary>
        /// Write details of a successful action
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Success(string message);

        /// <summary>
        /// Write details of a failed action
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Failure(string message);

        /// <summary>
        /// Write a warning
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Warning(string message);

        /// <summary>
        /// Write information
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Information(string message);

        /// <summary>
        /// Write detailed information
        /// </summary>
        /// <param name="message">The message to write.</param>
        void Detail(string message);
    }
}
