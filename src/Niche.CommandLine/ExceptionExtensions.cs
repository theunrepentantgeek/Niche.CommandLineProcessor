using System;
using System.Collections;
using System.Collections.Generic;

namespace Niche.CommandLine
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Turn a single exception into a sequence by flattening the nested inner exceptions
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A sequence of all the exceptions.</returns>
        public static IEnumerable<Exception> AsEnumerable(this Exception exception)
        {
            var e = exception;
            do
            {
                yield return e;
                e = e.InnerException;
            } while (e != null);
        }

        /// <summary>
        /// Turn a single exception into a sequence of strings for logging
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A sequence of strings</returns>
        public static IEnumerable<string> AsStrings(this Exception exception)
        {
            yield return string.Format("{0} ({1})", exception.Message, exception.GetType().Name);

            // Better safe than sorry
            // R# says this can never be null, but "CLR via C#" (Jeffrey Richter) says otherwise.
            if (exception.Data != null)
            {
                foreach (DictionaryEntry p in exception.Data)
                {
                    yield return string.Format("{0}: {1}", p.Key, p.Value);
                }
            }
        }

    }
}
