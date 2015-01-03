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
                () => new CommandLineMode(typeof(BaseDriver), null, method));
        }

        [Test]
        public void Constructor_givenNullMethod_throwsException()
        {
            var driver = new BaseDriver();
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineMode(typeof(BaseDriver), driver, null));
        }

        [Test]
        public void Constructor_forTestPerformance_initialisesName()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            Assert.That(mode.Name, Is.EqualTo("test-performance"));
        }

        [Test]
        public void Constructor_forHelp_initialisesDescription()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            Assert.That(mode.Description, Is.EqualTo("Performance tests"));
        }

        [Test]
        public void Constructor_givenNonModeMethod_throwsException()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("Debug");
            Assert.Throws<ArgumentException>(
                () => new CommandLineMode(typeof(BaseDriver), driver, method));
        }

        [Test]
        public void Constructor_givenMethodForOtherClass_throwsException()
        {
            var driver = new BaseDriver();
            var method = typeof(string).GetMethod("Clone");
            Assert.Throws<ArgumentException>(
                 () => new CommandLineMode(typeof(BaseDriver), driver, method));
        }

        [Test]
        public void Activate_forTestPerformance_callsMethod()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            var result = mode.Activate();
            Assert.That(result, Is.InstanceOf<TestDriver>());
        }

        [Test]
        public void CreateHelp_forTestPerformance_generatesItem()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            Assert.That(mode.CreateHelp().ToList(), Has.Count.EqualTo(1));
        }
    }
}
