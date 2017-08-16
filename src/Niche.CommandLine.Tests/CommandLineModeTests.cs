using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineModeTests
    {
        private readonly BaseDriver _driver = new BaseDriver();

        private readonly MethodInfo _method = typeof(BaseDriver).GetMethod("TestPerformance");

        private readonly CommandLineMode _mode;

        public CommandLineModeTests()
        {
            _mode = new CommandLineMode(typeof(BaseDriver), _driver, _method);
        }

        public class Constructor : CommandLineModeTests
        {
            [Fact]
            public void GivenNullType_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new CommandLineMode(null, _driver, _method));
            }

            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new CommandLineMode(typeof(BaseDriver), null, _method));
            }

            [Fact]
            public void GivenNullMethod_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineMode(typeof(BaseDriver), _driver, null));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void ForTestPerformance_InitializesName()
            {
                _mode.Name.Should().Be("test-performance");
            }

            [Fact]
            public void ForHelp_InitializesDescription()
            {
                _mode.Description.Should().Be("Performance tests");
            }

            [Fact]
            public void GivenNonModeMethod_ThrowsException()
            {
                var method = typeof(BaseDriver).GetMethod("Debug");
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => new CommandLineMode(typeof(BaseDriver), _driver, method));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void GivenMethodForOtherClass_ThrowsException()
            {
                var method = typeof(string).GetMethod("Clone");
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => new CommandLineMode(typeof(BaseDriver), _driver, method));
                exception.ParamName.Should().Be("method");
            }
        }

        public class Activate : CommandLineModeTests
        {
            [Fact]
            public void ForTestPerformance_CallsMethod()
            {
                var result = _mode.Activate();
                result.Should().BeOfType<TestDriver>();
            }
        }

        public class CreateHelp : CommandLineModeTests
        {
            [Fact]
            public void ForTestPerformance_GeneratesItem()
            {
                _mode.CreateHelp().Should().HaveCount(1);
            }
        }
    }
}
