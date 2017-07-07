using System.Collections.Generic;
using System.ComponentModel;

namespace Niche.CommandLine.Tests
{
    /// <summary>
    /// Driver used to test variable assignment functionality of the commandline processor
    /// </summary>
    public class AssignmentDriver
    {
        public string this[string name] => _variables[name];

        [Description("Verbose output for debugging.")]
        public void Verbose()
        {
            mVerbose = true;
        }

        [Description("Define a variable.")]
        public void Define(IEnumerable<KeyValuePair<string, string>> variables)
        {
            foreach (var p in variables)
            {
                mVariables[p.Key] = p.Value;
            }
        }

        private readonly Dictionary<string, string> mVariables = new Dictionary<string, string>();

        private bool mVerbose;
    }
}
