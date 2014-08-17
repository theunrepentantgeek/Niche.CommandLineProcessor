﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineParameterTests
    {
        [Test]
        public void Constructor_missingInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter(null, method);
                });
        }

        [Test]
        public void Constructor_missingMethod_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter(driver, null);
                });
        }

        [Test]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentException>(
                () =>
                {
                    new CommandLineParameter(this, method);
                });
        }

        [Test]
        public void IsParameter_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CommandLineParameter.IsParameter(null);
                });
        }

        [Test]
        public void IsParameter_givenParameterMethod_returnsTrue()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.That(CommandLineParameter.IsParameter(method), Is.True);
        }

        [Test]
        public void IsParameter_givenSwitchMethod_returnsFalse()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Help");
            Assert.That(CommandLineParameter.IsParameter(method), Is.False);
        }

        [Test]
        public void Activate_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineParameter = new CommandLineParameter(driver, method);
            Assert.Throws<ArgumentNullException>(
            () => commandLineParameter.Activate(null));
        }

        [Test]
        public void Activate_whenConfigured_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineParameter = new CommandLineParameter(driver, method);
            var arguments = new Queue<string>();
            arguments.Enqueue("search");
            commandLineParameter.Activate(arguments);
            Assert.That(driver.Searches, Is.EquivalentTo(new List<string> { "search" }));
        }

        [Test]
        public void ShortName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineSwitch = new CommandLineParameter(driver, method);
            Assert.That(commandLineSwitch.ShortName, Is.EqualTo("-f"));
        }

        [Test]
        public void LongName_whenConfigured_isExpected()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineSwitch = new CommandLineParameter(driver, method);
            Assert.That(commandLineSwitch.LongName, Is.EqualTo("--find"));
        }

        [Test]
        public void ConfigureParameters_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
            () => CommandLineParameter.CreateParameters(null));
        }

        [Test]
        public void AddOptionsTo_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var commandLineParameter = CommandLineParameter.CreateParameters(driver).First();
            Assert.Throws<ArgumentNullException>(
                () => commandLineParameter.AddOptionsTo(null));
        }

        [Test]
        public void AddOptionsTo_givenDictionary_AddsEntries()
        {
            var driver = new SampleDriver();
            var commandLineParameter = CommandLineParameter.CreateParameters(driver).First();
            var options = new Dictionary<string, CommandLineOptionBase>();
            commandLineParameter.AddOptionsTo(options);
            Assert.That(options.Count, Is.EqualTo(3));
        }

        [Test]
        public void AddHelpTo_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var commandLineParameter = CommandLineParameter.CreateParameters(driver).First();
            Assert.Throws<ArgumentNullException>(
                () => commandLineParameter.AddHelpTo(null));
        }

        [Test]
        public void AddHelpTo_givenList_AddsEntry()
        {
            var driver = new SampleDriver();
            var commandLineParameter
                = CommandLineParameter.CreateParameters(driver)
                .Single(p => p.ShortName == "-f");
            var help = new List<string>();
            commandLineParameter.AddHelpTo(help);
            Assert.That(help, Has.Count.EqualTo(1));
        }

        [Test]
        public void Completed_whenRequiredParameterOmitted_generatesError()
        {
            var driver = new SampleDriver();
            var commandLineParameter
                = CommandLineParameter.CreateParameters(driver)
                .Single(p => p.ShortName == "-f");
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            Assert.That(errors, Is.Not.Empty);
        }

        [Test]
        public void Completed_whenOptionalParameterOmitted_generatesNoErrors()
        {
            var driver = new SampleDriver();
            var commandLineParameter
                = CommandLineParameter.CreateParameters(driver)
                .Single(p => p.ShortName == "-u");
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            Assert.That(errors, Is.Empty);
        }
    }
}
