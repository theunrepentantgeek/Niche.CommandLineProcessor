using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niche.CommandLine;

namespace Niche.CommandLine.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new CommandLineProcessor(args);
            var driver = new Driver();
            var files = processor.Configure(driver);


        }
    }
}
