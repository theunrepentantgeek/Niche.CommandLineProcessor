using System.Collections.Generic;
using System.ComponentModel;

namespace Niche.CommandLine.Tests
{
    /// <summary>
    /// Driver used to test variable assignment functionality of the commandline processor
    /// </summary>
    public class AssignmentDriver
    {
        private readonly Dictionary<string, string> _variables = new Dictionary<string, string>();

        public bool IsVerbose { get; private set; }

        public string this[string name] => _variables[name];

        [Description("Verbose output for debugging.")]
        public void Verbose()
        {
            IsVerbose = true;
        }

        [Description("Define a variable.")]
        public void Define(IEnumerable<KeyValuePair<string, string>> variables)
        {
            foreach (var p in variables)
            {
                _variables[p.Key] = p.Value;
            }
        }
    }
}
