using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Demo
{
    /// <summary>
    /// Command line interface for the demo
    /// </summary>
    public class Driver
    {
        public bool IsVerbose { get; private set; }

        public IList<string> SearchTerms { get { return mSearchTerms; } }

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

        private readonly List<string> mSearchTerms = new List<string>();
    }
}
