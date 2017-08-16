using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineSwitchTests
    {
        private readonly SampleDriver _driver = new SampleDriver();

        public class Constructor : CommandLineSwitchTests
        {
            [Fact]
            public void MissingInstance_ThrowsException()
            {
                var method = _driver.GetType().GetMethod("Verbose");
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineSwitch(null, method));
                    exception.ParamName.Should().Be("instance");
            }

            [Fact]
            public void MissingMethod_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineSwitch(_driver, null));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void IfMethodDoesNotApplyToInstance_ThrowsException()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => new CommandLineSwitch(this, method));
                exception.ParamName.Should().Be("method");
            }
        }

        public class TryActivate : CommandLineSwitchTests
        {
            [Fact]
            public void GivenNullArguments_ThrowsException()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var commandLineSwitch = new CommandLineSwitch(_driver, method);
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => commandLineSwitch.TryActivate(null));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void GivenEmptyArguments_ReturnsFalse()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var commandLineSwitch = new CommandLineSwitch(_driver, method);
                var arguments = new Queue<string>();
                commandLineSwitch.TryActivate(arguments).Should().BeFalse();
            }

            [Fact]
            public void WhenConfigured_CallsMethod()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var commandLineSwitch = new CommandLineSwitch(_driver, method);
                var arguments = new Queue<string>();
                arguments.Enqueue("-d");
                commandLineSwitch.TryActivate(arguments);
                _driver.ShowDiagnostics.Should().BeTrue();
            }
        }

        public class ShortName : CommandLineSwitchTests
        {
            [Fact]
            public void WhenConfigured_isExpected()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var commandLineSwitch = new CommandLineSwitch(_driver, method);
                commandLineSwitch.ShortName.Should().Be("-d");
            }
        }

        public class LongName : CommandLineSwitchTests
        {
            [Fact]
            public void WhenConfigured_isExpected()
            {
                var method = _driver.GetType().GetMethod("Debug");
                var commandLineSwitch = new CommandLineSwitch(_driver, method);
                commandLineSwitch.LongName.Should().Be("--debug");
            }
        }

        public class CreateHelp : CommandLineSwitchTests
        {
            [Fact]
            public void GivenList_AddsEntry()
            {
                var commandLineSwitch = CommandLineOptionFactory.CreateSwitches(_driver).First();
                var help = commandLineSwitch.CreateHelp().ToList();
                help.Should().HaveCount(1);
            }
        }
    }
}
