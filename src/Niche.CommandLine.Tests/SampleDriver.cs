using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    public class SampleDriver
    {
        public bool ShowHelp { get; private set; }

        /// <summary>
        /// This is a switch method
        /// </summary>
        [Description("Show Help")]
        public void Help()
        {
            ShowHelp = true;
        }

        /// <summary>
        /// This is a parameter method
        /// </summary>
        /// <param name="term"></param>
        [Description("Find")]
        public void Find(string term)
        {
            // Nothing
        }

        /// <summary>
        /// This is not a switch - no [Description]
        /// </summary>
        public void Verbose()
        {
            // Nothing
        }



    }
}
