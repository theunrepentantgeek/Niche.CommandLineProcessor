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
        public void IsSwitch_givenModeMethod_returnsFalse()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            Assert.That(CommandLineOptionFactory.IsSwitch(method), Is.False);
        }

        [Test]
        public void CreateSwitches_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateSwitches(null));
        }

        [Test]
        public void IsParameter_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineOptionFactory.IsParameter(null);
                });
        }

        [Test]
        public void IsParameter_givenParameterMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineOptionFactory.IsParameter(method), Is.True);
        }

        [Test]
        public void IsParameter_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineOptionFactory.IsParameter(method), Is.False);
        }

        [Test]
        public void IsParameter_givenModeMethod_returnsFalse()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            Assert.That(CommandLineOptionFactory.IsParameter(method), Is.False);
        }

        [Test]
        public void CreateParameters_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateParameters(null));
        }

        [Test]
        public void IsMode_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.IsMode<BaseDriver>(null));
        }

        [Test]
        public void IsMode_givenParameterMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineOptionFactory.IsMode<BaseDriver>(method), Is.False);
        }

        [Test]
        public void IsMode_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineOptionFactory.IsMode<BaseDriver>(method), Is.False);
        }

        [Test]
        public void IsMode_givenModelMethod_returnsTrue()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            Assert.That(CommandLineOptionFactory.IsMode<BaseDriver>(method), Is.True);
        }

        [Test]
        public void CreateModes_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateModes<BaseDriver>(null));
        }
    }
}
