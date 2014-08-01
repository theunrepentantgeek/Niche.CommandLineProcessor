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
        public void Constructor_givenArguments_populatesArguments()
        {
            var arguments = new List<string> { "--help" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            Assert.That(processor.Arguments, Has.Count.EqualTo(1));
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

        [Test]
        public void Configure_withLongFormParameter_callsMethods()
        {
            var arguments = new List<string> { "--find", "fu" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(driver.Searches, Is.EquivalentTo(new List<string> { "fu" }));
        }

        [Test]
        public void Configure_withShortFormParameter_callsMethod()
        {
            var arguments = new List<string> { "-f", "fu" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(driver.Searches, Is.EquivalentTo(new List<string> { "fu" }));
        }

        [Test]
        public void Configure_withParameterRequiringConversion_callsMethod()
        {
            var arguments = new List<string> { "-r", "4" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(driver.Repeats, Is.EqualTo(4));
        }

        [Test]
        public void Configure_withUnexpectedOption_leavesOptionInList()
        {
            var arguments = new List<string> { "snafu" };
            var driver = new SampleDriver();
            var processor = new CommandLineProcessor(arguments);
            processor.Configure(driver);
            Assert.That(processor.Arguments, Is.EquivalentTo(new List<string> { "snafu" }));
        }
    }
}
