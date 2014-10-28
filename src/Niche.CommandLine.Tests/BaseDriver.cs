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
        public bool ShowHelp { get; private set; }

        /// <summary>
        /// This is a switch method
        /// </summary>
        [Description("Show Help")]
        public void Help()
        {
            ShowHelp = true;
        }


    }
}
