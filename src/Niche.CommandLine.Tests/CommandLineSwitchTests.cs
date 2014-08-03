using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineSwitchTests
    {
        [Test]
        public void Constructor_missingInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineSwitch(null, method);
                });
        }

        [Test]
        public void Constructor_missingMethod_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineSwitch(driver, null);
                });
        }

        [Test]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.Throws<ArgumentException>(
                () =>
                {
                    new CommandLineSwitch(this, method);
                });
        }

        [Test]
        public void IsSwitch_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineSwitch.IsSwitch(null);
                });
        }

        [Test]
        public void IsSwitch_givenSwitchMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineSwitch.IsSwitch(method), Is.True);
        }

        [Test]
        public void IsSwitch_givenParameterMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineSwitch.IsSwitch(method), Is.False);
        }

        [Test]
        public void IsSwitch_givenMethodMissingDescription_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Verbose");
            Assert.That(CommandLineSwitch.IsSwitch(method), Is.False);
        }

        [Test]
        public void IsSwitch_givenMethodWithParameters_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineSwitch.IsSwitch(method), Is.False);
        }

        [Test]
        public void Activate_whenConfigured_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            var arguments = new Queue<string>();
            commandLineSwitch.Activate(arguments);
            Assert.That(driver.ShowHelp, Is.True);
        }

        [Test]
        public void ShortName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            Assert.That(commandLineSwitch.ShortName, Is.EqualTo("-h"));
        }

        [Test]
        public void LongName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            Assert.That(commandLineSwitch.LongName, Is.EqualTo("--help"));
        }

        [Test]
        public void ConfigureSwitches_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineSwitch.CreateSwitches(null));
        }

        [Test]
        public void AddOptionsTo_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var commandLineSwitch = CommandLineSwitch.CreateSwitches(driver).First();
            Assert.Throws<ArgumentNullException>(
                () => commandLineSwitch.AddOptionsTo(null));
        }

        [Test]
        public void AddOptionsTo_givenDictionary_AddsEntries()
        {
            var driver = new SampleDriver();
            var commandLineSwitch = CommandLineSwitch.CreateSwitches(driver).First();
            var options = new Dictionary<string, CommandLineOptionBase>();
            commandLineSwitch.AddOptionsTo(options);
            Assert.That(options.Count, Is.EqualTo(3));
        }
    }
}
