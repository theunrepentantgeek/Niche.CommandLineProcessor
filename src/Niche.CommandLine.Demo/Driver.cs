using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Niche.CommandLine.Demo
{
    /// <summary>
    /// Command line interface for the demo
    /// </summary>
    public class Driver
    {
        public bool IsVerbose { get; private set; }

        public IList<string> SearchTerms { get { return mSearchTerms; } }

        public ConsoleColor ForegroundColor { get; private set; }
        
        [Description("Verbose output")]
        public void Verbose()
        {
            IsVerbose = true;
        }

        [Description("Find items by keyword")]
        public void Find(string keyword)
        {
            mSearchTerms.Add(keyword);
        }

        [Description("Specify the color of output (and test automatic conversion)")]
        public void ForeGround(ConsoleColor color)
        {
            ForegroundColor = color;
        }

        [Description("Sample Mode (recursive!)")]
        public Driver RecursiveMode()
        {
            return this;
        }

        private readonly List<string> mSearchTerms = new List<string>();
    }
}
