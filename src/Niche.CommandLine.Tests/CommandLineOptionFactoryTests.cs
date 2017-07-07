using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineOptionFactoryTests
    {
        [Fact]
        public void IsSwitch_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineOptionFactory.IsSwitch(null);
                });
        }

        [Fact]
        public void IsSwitch_givenSwitchMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            CommandLineOptionFactory.IsSwitch(method).Should().BeTrue();
        }

        [Fact]
        public void IsSwitch_givenParameterMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
        }

        [Fact]
        public void IsSwitch_givenMethodMissingDescription_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Verbose");
            CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
        }

        [Fact]
        public void IsSwitch_givenMethodWithParameters_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
        }

        [Fact]
        public void IsSwitch_givenModeMethod_returnsFalse()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            CommandLineOptionFactory.IsSwitch(method).Should().BeFalse();
        }

        [Fact]
        public void CreateSwitches_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateSwitches(null));
        }

        [Fact]
        public void IsParameter_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineOptionFactory.IsParameter(null);
                });
        }

        [Fact]
        public void IsParameter_givenParameterMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            CommandLineOptionFactory.IsParameter(method).Should().BeTrue();
        }

        [Fact]
        public void IsParameter_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            CommandLineOptionFactory.IsParameter(method).Should().BeFalse();
        }

        [Fact]
        public void IsParameter_givenModeMethod_returnsFalse()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            CommandLineOptionFactory.IsParameter(method).Should().BeFalse();
        }

        [Fact]
        public void CreateParameters_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateParameters(null));
        }

        [Fact]
        public void CreateParameters_givenDriver_returnsParameters()
        {
            var driver = new SampleDriver();
            var parameters = CommandLineOptionFactory.CreateParameters(driver);
            parameters.Should().NotBeEmpty();
        }

        [Fact]
        public void CreateParameters_givenDriver_returnsParameterOfCorrectType()
        {
            var driver = new SampleDriver();
            var parameters
                = CommandLineOptionFactory.CreateParameters(driver);
            var uploadParameter = parameters.Single(p => p.Method.Name == "Upload");
            uploadParameter.Should().BeOfType<CommandLineParameter<string>>();
        }

        [Fact]
        public void IsMode_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.IsMode<BaseDriver>(null));
        }

        [Fact]
        public void IsMode_givenParameterMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeFalse();
        }

        [Fact]
        public void IsMode_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeFalse();
        }

        [Fact]
        public void IsMode_givenModelMethod_returnsTrue()
        {
            var driver = new BaseDriver();
            var method = driver.GetType().GetMethod("TestPerformance");
            CommandLineOptionFactory.IsMode<BaseDriver>(method).Should().BeTrue();
        }

        [Fact]
        public void CreateModes_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CommandLineOptionFactory.CreateModes<BaseDriver>(null));
        }
    }
}
