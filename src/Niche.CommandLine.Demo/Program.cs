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

            var processor = new CommandLineProcessor(args);
            var driver = new Driver();
            processor.Configure(driver);

            // If we had any errors, output the list and then exit
            if (processor.HasErrors)
            {
                logger.Failure(processor.Errors, " ");
                return;
            }

            logger.Information("Available commandline options:");
            logger.Detail(processor.Help, "   ");

        }
    }
}
