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
        private readonly List<string> _searchTerms = new List<string>();

        public bool IsVerbose { get; private set; }

        public IList<string> SearchTerms => _searchTerms;

        public ConsoleColor ForegroundColor { get; private set; }
        
        [Description("Verbose output")]
        public void Verbose()
        {
            IsVerbose = true;
        }

        [Description("Find items by keyword")]
        public void Find(string keyword)
        {
            _searchTerms.Add(keyword);
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
    }
}
