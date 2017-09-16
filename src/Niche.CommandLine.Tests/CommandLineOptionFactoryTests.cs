using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineOptionFactoryTests
    {
        private readonly SampleDriver _driver = new SampleDriver();

        public class IsSwitch : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => CommandLineOptionFactory.IsSwitch(null));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void GivenSwitchMethod_ReturnsTrue()
            {
                var method = _driver.GetType().GetMethod("Debug");
                CommandLineOptionFactory.IsSwitch(method).Should().BeTrue();
            }

            [Fact]
            public void GivenParameterMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Find");
                CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
            }

            [Fact]
            public void GivenMethodMissingDescription_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Verbose");
                CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
            }

            [Fact]
            public void GivenMethodWithParameters_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Find");
                CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
            }

            [Fact]
            public void GivenModeMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("TestPerformance");
                CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
            }
        }

        public class CreateSwitches : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => CommandLineOptionFactory.CreateSwitches(null));
                exception.ParamName.Should().Be("instance");
            }
        }

        public class IsParameter : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => CommandLineOptionFactory.IsParameter(null));
            }

            [Fact]
            public void GivenParameterMethod_ReturnsTrue()
            {
                var method = _driver.GetType().GetMethod("Find");
                CommandLineOptionFactory.IsParameter(method).Should().BeTrue();
            }

            [Fact]
            public void GivenSwitchMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Debug");
                CommandLineOptionFactory.IsParameter(method).Should().BeFalse();
            }

            [Fact]
            public void GivenModeMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("TestPerformance");
                CommandLineOptionFactory.IsParameter(method).Should().BeFalse();
            }
        }

        public class CreateParameters : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => CommandLineOptionFactory.CreateParameters(null));
                exception.ParamName.Should().Be("instance");
            }

            [Fact]
            public void GivenDriver_ReturnsParameters()
            {
                var parameters = CommandLineOptionFactory.CreateParameters(_driver);
                parameters.Should().NotBeEmpty();
            }

            [Fact]
            public void GivenDriver_ReturnsParameterOfCorrectType()
            {
                var parameters
                    = CommandLineOptionFactory.CreateParameters(_driver);
                var uploadParameter = parameters.Single(p => p.Method.Name == "Upload");
                uploadParameter.Should().BeOfType<CommandLineParameter<string>>();
            }
        }

        public class IsMode : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => CommandLineOptionFactory.IsMode<BaseDriver>(null));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void GivenParameterMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Find");
                CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeFalse();
            }

            [Fact]
            public void GivenSwitchMethod_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Debug");
                CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeFalse();
            }

            [Fact]
            public void GivenModelMethod_ReturnsTrue()
            {
                var method = _driver.GetType().GetMethod("TestPerformance");
                CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeTrue();
            }
        }

        public class CreateModes : CommandLineOptionFactoryTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => CommandLineOptionFactory.CreateModes<BaseDriver>(null));
                exception.ParamName.Should().Be("instance");
            }
        }
    }
}
