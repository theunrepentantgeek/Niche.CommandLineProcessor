using System;
using System.Diagnostics;

namespace Niche.CommandLine.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger(
                ConsoleLoggerOptions.DisplayBanner
                | ConsoleLoggerOptions.UseLabels
                | ConsoleLoggerOptions.ShowTime);

            var driver = new Driver();
            var processor = new CommandLineProcessor(args);
            //processor.(driver);

            // If we had any errors, output the list and then exit
            if (processor.HasErrors)
            {
                logger.Failure(processor.Errors);
                return;
            }

            if (processor.ShowHelp)
            {
                logger.Information("Available commandline options:");
                logger.Detail(processor.OptionHelp);
            }

            logger.Information("Configured foreground: {0}", driver.ForegroundColor);

            logger.Heading("Heading");

            logger.Action("Action");
            logger.Information("Information");
            logger.Detail("Detail");
            logger.Debug("Debug");
            logger.Warning("Warning");
            logger.Success("Success");
            logger.Failure("Failure");

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }
    }
}
