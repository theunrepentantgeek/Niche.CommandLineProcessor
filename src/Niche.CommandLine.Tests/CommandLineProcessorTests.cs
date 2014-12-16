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
        public void Constructor_givenNullForArguments_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineProcessor<BaseDriver>(null, new BaseDriver());
                });
        }

        [Test]
        public void Constructor_givenNullForDriver_throwsException()
        {
            var arguments = new List<string> { "--help" };
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineProcessor<BaseDriver>(arguments, null);
                });
        }

        [Test]
        public void Constructor_withLongFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "--help" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withShortFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "-h" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withAlternateShortFormSwitch_callsMethod()
        {
            var arguments = new List<string> { "/h" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.Driver.ShowHelp, Is.True);
        }

        [Test]
        public void Constructor_withLongFormParameter_callsMethods()
        {
            var arguments = new List<string> { "--find", "file" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Driver.TextSearch, Is.EqualTo("file"));
        }

        [Test]
        public void Constructor_withShortFormParameter_callsMethod()
        {
            var arguments = new List<string> { "-f", "file" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Driver.TextSearch, Is.EqualTo("file"));
        }

        [Test]
        public void Constructor_withAlternateShortFormParameter_callsMethod()
        {
            var arguments = new List<string> { "/f", "file" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Driver.TextSearch, Is.EqualTo("file"));
        }

        [Test]
        public void Constructor_withParameterRequiringConversion_callsMethod()
        {
            var arguments = new List<string> { "-r", "4" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Driver.Repeats, Is.EqualTo(4));
        }

        [Test]
        public void Constructor_withUnexpectedArgument_leavesItInList()
        {
            var arguments = new List<string> { "snafu" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Arguments, Is.EquivalentTo(new List<string> { "snafu" }));
        }

        [Test]
        public void Constructor_withUnexpectedOption_generatesError()
        {
            var arguments = new List<string> { "-s" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.HasErrors, Is.True);
        }

        [Test]
        public void Constructor_withValidValueForParameter_configuresDriver()
        {
            var arguments = new List<string> { "--repeat", "5" };
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.Driver.Repeats, Is.EqualTo(5));
        }

        [Test]
        public void Constructor_withInvalidValueForParameter_generatesError()
        {
            var arguments = new List<string> { "--repeat", "twice" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.HasErrors, Is.True);
        }

        [Test]
        public void Constructor_specifyingMode_returnsDriverForMode()
        {
            var arguments = new List<string> { "test-performance", "--help" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.Driver, Is.InstanceOfType<TestDriver>());
        }

        [Test]
        public void OptionHelp_forValidDriver_returnsText()
        {
            var arguments = new List<string> { "test-performance", "--help" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.OptionHelp, Is.Not.Empty);
        }

        [Test]
        public void Help_forValidDriver_SetsShowHelp()
        {
            var arguments = new List<string> { "test-performance", "--help" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            processor.Help();
            Assert.That(processor.ShowHelp, Is.True);
        }

        [Test]
        public void Errors_forInvalidParameter_ListsContent()
        {
            var arguments = new List<string> { "--not-an-option" };
            var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
            Assert.That(processor.Errors, Is.Not.Empty);
        }

        [Test]
        public void OptionHelp_withNoOptions_returnsHelp()
        {
            var arguments = new List<string>();
            var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
            Assert.That(processor.OptionHelp.ToList(), Has.Count.GreaterThan(0));
        }
    }
}
