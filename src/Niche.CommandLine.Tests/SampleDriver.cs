using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    public class SampleDriver : BaseDriver
    {
        public string TextSearch { get; private set; }

        public IEnumerable<string> FilesToUpload { get; private set; }

        public int Repeats { get; private set; }

        public SampleDriver()
        {
        }

        [Description("This is the 'activate' mode")]
        public SampleDriver Activate()
        {
            return this;
        }

        /// <summary>
        /// This is a parameter method
        /// </summary>
        /// <param name="term"></param>
        [Required]
        [Description("This is the 'find' parameter")]
        public void Find(string term)
        {
            TextSearch = term;
        }

        [Description("Upload file")]    
        public void Upload(IEnumerable<string> files)
        {
            FilesToUpload = files.ToList();
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
