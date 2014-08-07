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
                    new CommandLineProcessor<SampleDriver>(null);
                });
        }

        [Test]
        public void Constructor_withLongFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "--help" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withShortFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "-h" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withAlternateShortFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "/h" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withLongFormParameter_callsMethods()
        {
            var arguments = new List<string> { "--find", "fu" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.Searches, Is.EquivalentTo(new List<string> { "fu" }));
        }

        [Test]
        public void Constructor_withShortFormParameter_callsMethod()
        {
            var arguments = new List<string> { "-f", "fu" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.Searches, Is.EquivalentTo(new List<string> { "fu" }));
        }

        [Test]
        public void Constructor_withAlternateShortFormParameter_callsMethod()
        {
            var arguments = new List<string> { "/f", "fu" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.Searches, Is.EquivalentTo(new List<string> { "fu" }));
        }

        [Test]
        public void Constructor_withParameterRequiringConversion_callsMethod()
        {
            var arguments = new List<string> { "-r", "4" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Driver.Repeats, Is.EqualTo(4));
        }

        [Test]
        public void Constructor_withUnexpectedArgument_leavesItInList()
        {
            var arguments = new List<string> { "snafu" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.Arguments, Is.EquivalentTo(new List<string> { "snafu" }));
        }

        [Test]
        public void Constructor_withUnexpectedOption_generatesError()
        {
            var arguments = new List<string> { "-s" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments);
            Assert.That(processor.HasErrors, Is.True);
        }
    }
}
