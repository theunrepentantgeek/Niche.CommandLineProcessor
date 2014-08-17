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

        public List<string> Searches { get; private set; }

        public int Repeats { get; private set; }

        public SampleDriver()
        {
            Searches = new List<string>();
        }

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
        [Required]
        [Description("Find")]
        public void Find(string term)
        {
            Searches.Add(term);
        }

        [Description("Upload file")]
        public void Upload(IEnumerable<string> files)
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

        [Description("Specify how many repetitions")]
        public void Repeat(int count)
        {
            Repeats = count;
        }

    }
}
