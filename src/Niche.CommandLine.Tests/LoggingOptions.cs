using System.ComponentModel;

namespace Niche.CommandLine.Tests
{
    public class LoggingOptions
    {
        public bool IsQuiet { get; private set; }

        public bool IsVerbose { get; private set; }

        public bool IsDebug { get; private set; }

        [Description("Use minimal logging")]
        public void Quiet()
        {
            IsQuiet = true;
        }

        [Description("Use verbose logging")]
        public void Verbose()
        {
            IsVerbose = true;
        }

        [Description("Use debug logging")]
        public void Debug()
        {
            IsDebug = true;
        }
    }
}
