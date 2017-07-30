using System.ComponentModel;

namespace Niche.CommandLine
{
    public class StandardOptions
    {
        // A flag for whether to show help
        private bool _showHelp;

        /// <summary>
        /// Gets a value indicating whether we should should help
        /// </summary>
        public bool ShowHelp => _showHelp;

        [Description("Show this help")]
        public void Help()
        {
            _showHelp = true;
        }
    }
}