using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    /// <summary>
    /// Driver used to test variable assignment functionality of the commanline processor
    /// </summary>
    public class AssignmentDriver
    {
        public string this[string name]
        {
            get { return mVariables[name]; }
        }

        [Description("Verbose output for debugging.")]
        public void Verbose()
        {
            mVerbose = true;
        }

        [Description("Define a variable.")]
        public void Define(string name, string value)
        {
            mVariables[name] = value;
        }

        private readonly Dictionary<string, string> mVariables = new Dictionary<string, string>();

        private bool mVerbose;
    }
}
