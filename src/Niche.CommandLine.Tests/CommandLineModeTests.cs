using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineModeTests
    {
        [Test]
        public void Constructor_givenNullInstance_throwsException()
        {
            var method = typeof(BaseDriver).GetMethod("Help");
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineMode<BaseDriver>(null, method));
        }

        [Test]
        public void Constructor_givenNullMethod_throwsException()
        {
            var driver = new BaseDriver();
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineMode<BaseDriver>(driver, null));
        }

        [Test]
        public void Constructor_forHelp_initialisesName()
        {
            // Arrange
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("Help");
            // Act
            var mode = new CommandLineMode<BaseDriver>(driver, method);
            // Assert
            Assert.That(mode.Name, Is.EqualTo("help"));
        }

        [Test]
        public void Constructor_forTestPerformance_initialisesName()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode<BaseDriver>(driver, method);
            Assert.That(mode.Name, Is.EqualTo("test-performance"));
        }

        [Test]
        public void Constructor_forHelp_initialisesDescription()
        {
            // Arrange
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("Help");
            // Act
            var mode = new CommandLineMode<BaseDriver>(driver, method);
            // Assert
            Assert.That(mode.Description, Is.EqualTo("Show Help"));
        }

        [Test]
        public void Activate_forTestPerformance_callsMethod()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode<BaseDriver>(driver, method);
            var result = mode.Activate();
            Assert.That(result, Is.InstanceOf<TestDriver>());
        }
    }
}
