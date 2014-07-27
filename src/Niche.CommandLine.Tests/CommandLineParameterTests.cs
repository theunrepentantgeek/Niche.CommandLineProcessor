using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineParameterTests
    {
        [Test]
        public void Constructor_missingInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter(null, method);
                });
        }

        [Test]
        public void Constructor_missingMethod_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter(null, method);
                });
        }

        [Test]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentException>(
                () =>
                {
                    new CommandLineParameter(this, method);
                });
        }

        [Test]
        public void IsParameter_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineParameter.IsParameter(null);
                });
        }

        [Test]
        public void IsParameter_givenParameterMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineParameter.IsParameter(method), Is.True);
        }

        [Test]
        public void IsParameter_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineParameter.IsParameter(method), Is.False);
        }

        [Test]
        public void Activate_whenConfigured_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineParameter = new CommandLineParameter(driver, method);
            var arguments = new Queue<string>();
            arguments.Enqueue("search");
            commandLineParameter.Activate(arguments);
            Assert.That(driver.Searches, Is.EquivalentTo(new List<string> { "search" }));
        }

        [Test]
        public void ShortName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineSwitch = new CommandLineParameter(driver, method);
            Assert.That(commandLineSwitch.ShortName, Is.EqualTo("-f"));
        }

        [Test]
        public void LongName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineSwitch = new CommandLineParameter(driver, method);
            Assert.That(commandLineSwitch.LongName, Is.EqualTo("--find"));
        }

        [Test]
        public void ConfigureParameters_givenNullInstance_throwsException()
        {
            var options = new Dictionary<string, CommandLineOptionBase>();
            Assert.Throws<ArgumentNullException>(
            () => CommandLineParameter.ConfigureParameters(null, options));
        }

        [Test]
        public void ConfigureParameters_givenNullOptions_throwsException()
        {
            var driver = new SampleDriver();
            Assert.Throws<ArgumentNullException>(
            () => CommandLineParameter.ConfigureParameters(driver, null));
        }

        [Test]
        public void ConfigureParameters_givenInstance_configuresOptions()
        {
            var driver = new SampleDriver();
            var options = new Dictionary<string, CommandLineOptionBase>();
            CommandLineParameter.ConfigureParameters(driver, options);
            Assert.That(options.Count, Is.EqualTo(2));
        }
    }
}
