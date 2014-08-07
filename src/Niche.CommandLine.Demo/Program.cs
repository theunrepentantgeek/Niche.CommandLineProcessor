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
            var logger = new ConsoleLogger();
            logger.ConsoleBanner();

            var processor = new CommandLineProcessor<Driver>(args);

            // If we had any errors, output the list and then exit
            if (processor.HasErrors)
            {
                logger.Failure(processor.Errors);
                return;
            }

            if (processor.Driver.ShowHelp)
            {
                logger.Information("Available commandline options:");
                logger.Detail(processor.Help);
            }
        }
    }
}
