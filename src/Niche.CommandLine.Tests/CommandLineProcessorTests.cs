using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineProcessorTests
    {
        [Test]
        public void Constructor_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineProcessor(null);
                });
        }

        [Test]
        public void Constructor_givenLogger_willUseIt()
        {
            var arguments = new List<string>();
            var logger = new ConsoleLogger();
            var processor = new CommandLineProcessor(arguments, logger);
            Assert.That(processor.Logger, Is.EqualTo(logger));
        }

        [Test]
        public void Configure_givenNull_throwsException()
        {
            var arguments = new List<string>();
            var processor = new CommandLineProcessor(arguments);
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    processor.Configure(null);
                });
        }

        [Test]
        public void Configure_withLongFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "--help" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(driver.ShowHelp, Is.True);
        }

        [Test]
        public void Configure_withShortFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "-h" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(driver.ShowHelp, Is.True);
        }
    }
}
