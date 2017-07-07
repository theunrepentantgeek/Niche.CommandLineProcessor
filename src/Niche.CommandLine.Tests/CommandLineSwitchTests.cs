using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineSwitchTests
    {
        [Fact]
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

        [Fact]
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

        [Fact]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            Assert.Throws<ArgumentException>(
                () =>
                {
                    new CommandLineSwitch(this, method);
                });
        }

        [Fact]
        public void TryActivate_whenConfigured_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            var arguments = new Queue<string>();
            arguments.Enqueue("-d");
            commandLineSwitch.TryActivate(arguments);
            driver.ShowDiagnostics.Should().BeTrue();
        }

        [Fact]
        public void ShortName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            commandLineSwitch.ShortName.Should().Be("-d");
        }

        [Fact]
        public void LongName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Debug");
            var commandLineSwitch = new CommandLineSwitch(driver, method);
            commandLineSwitch.LongName.Should().Be("--debug");
        }

        [Fact]
        public void CreateHelp_givenList_AddsEntry()
        {
            var driver = new SampleDriver();
            var commandLineSwitch = CommandLineOptionFactory.CreateSwitches(driver).First();
            var help = commandLineSwitch.CreateHelp().ToList();
            help.Should().HaveCount(1);
        }
    }
}
