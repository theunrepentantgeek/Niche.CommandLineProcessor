using System;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineModeTests
    {
        [Fact]
        public void Constructor_givenNullInstance_throwsException()
        {
            var method = typeof(BaseDriver).GetMethod("Help");
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineMode(typeof(BaseDriver), null, method));
        }

        [Fact]
        public void Constructor_givenNullMethod_throwsException()
        {
            var driver = new BaseDriver();
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineMode(typeof(BaseDriver), driver, null));
        }

        [Fact]
        public void Constructor_forTestPerformance_initialisesName()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            mode.Name.Should().Be("test-performance");
        }

        [Fact]
        public void Constructor_forHelp_initialisesDescription()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            mode.Description.Should().Be("Performance tests");
        }

        [Fact]
        public void Constructor_givenNonModeMethod_throwsException()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("Debug");
            Assert.Throws<ArgumentException>(
                () => new CommandLineMode(typeof(BaseDriver), driver, method));
        }

        [Fact]
        public void Constructor_givenMethodForOtherClass_throwsException()
        {
            var driver = new BaseDriver();
            var method = typeof(string).GetMethod("Clone");
            Assert.Throws<ArgumentException>(
                 () => new CommandLineMode(typeof(BaseDriver), driver, method));
        }

        [Fact]
        public void Activate_forTestPerformance_callsMethod()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            var result = mode.Activate();
            result.Should().BeOfType<TestDriver>();
        }

        [Fact]
        public void CreateHelp_forTestPerformance_generatesItem()
        {
            var driver = new BaseDriver();
            var method = typeof(BaseDriver).GetMethod("TestPerformance");
            var mode = new CommandLineMode(typeof(BaseDriver), driver, method);
            mode.CreateHelp().Should().HaveCount(1);
        }
    }
}
