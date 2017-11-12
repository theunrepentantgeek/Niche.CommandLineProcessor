using System.ComponentModel;

namespace Niche.CommandLine
{
    /// <summary>
    /// Standard options supported by every application
    /// </summary>
    public class StandardOptions
    {
        // A flag for whether to show help
        private bool _showHelp;

        /// <summary>
        /// Gets a value indicating whether we should should display help
        /// </summary>
        public bool ShowHelp => _showHelp;

        /// <summary>
        /// Command line switch used to request the display of help
        /// </summary>
        [Description("Show this help")]
        public void Help()
        {
            _showHelp = true;
        }
    }
}
