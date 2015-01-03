using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    public class BaseDriver
    {
        public bool ShowDiagnostics { get; private set; }

        /// <summary>
        /// This is a switch method
        /// </summary>
        [Description("Show Diagnostics")]
        public void Debug()
        {
            ShowDiagnostics = true;
        }

        [Description("Performance tests")]
        public TestDriver TestPerformance()
        {
            return new TestDriver();
        }

    }
}
