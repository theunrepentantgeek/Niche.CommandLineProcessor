using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineOptionFactoryTests
    {
        [Test]
        public void IsSwitch_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineOptionFactory.IsSwitch(null);
                });
        }

        [Test]
        public void IsSwitch_givenSwitchMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineOptionFactory.IsSwitch(method), Is.True);
        }

        [Test]
        public void IsSwitch_givenParameterMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineOptionFactory.IsSwitch(method), Is.False);
        }

        [Test]
        public void IsSwitch_givenMethodMissingDescription_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Verbose");
            Assert.That(CommandLineOptionFactory.IsSwitch(method), Is.False);
        }

        [Test]
        public void IsSwitch_givenMethodWithParameters_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineOptionFactory.IsSwitch(method), Is.False);
        }

        [Test]
        public void ConfigureSwitches_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateSwitches(null));
        }


    }
}
