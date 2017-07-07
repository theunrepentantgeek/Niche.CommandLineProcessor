using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineParameterTests
    {
        [Fact]
        public void Constructor_missingInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineParameter<string>(null, method));
        }

        [Fact]
        public void Constructor_missingMethod_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () => new CommandLineParameter<string>(driver, null));
        }

        [Fact]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentException>(
                () => new CommandLineParameter<string>(this, method));
        }

        [Fact]
        public void TryActivate_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            Assert.Throws<ArgumentNullException>(
            () => commandLineParameter.TryActivate(null));
        }

        [Fact]
        public void ConfigureParameters_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
            () => CommandLineOptionFactory.CreateParameters(null));
        }

        [Fact]
        public void CreateHelp_givenList_AddsEntry()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var help = commandLineParameter.CreateHelp().ToList();
            help.Should().HaveCount(1);
        }

        [Fact]
        public void Completed_withNoErrorList_throwsException()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            Assert.Throws<ArgumentNullException>(
                () => commandLineParameter.Completed(null));
        }

        [Fact]
        public void Completed_whenRequiredParameterOmitted_generatesError()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Completed_whenRequiredParameterSupplied_generatesNoErrors()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var arguments = new Queue<string>();
            arguments.Enqueue("-f");
            arguments.Enqueue("search");
            commandLineParameter.TryActivate(arguments);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            errors.Should().BeEmpty();
        }


        [Fact]
        public void Completed_whenRequiredParameterSupplied_configuresValue()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var arguments = new Queue<string>();
            arguments.Enqueue("-f");
            arguments.Enqueue("search");
            commandLineParameter.TryActivate(arguments);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
        }

        [Fact]
        public void Completed_whenParameterWithValue_configuresValue()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var arguments = new Queue<string>();
            arguments.Enqueue("-f:search");
            commandLineParameter.TryActivate(arguments);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
        }

        [Fact]
        public void Completed_whenOptionalParameterOmitted_generatesNoErrors()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Upload");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var errors = new List<string>();
            errors.Should().BeEmpty();
        }

        [Fact]
        public void Completed_whenSingleValuedParameterProvided_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var arguments = new Queue<string>();
            arguments.Enqueue("-f");
            arguments.Enqueue("search");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);
            driver.TextSearch.Should().Be("search");
        }

        [Fact]
        public void Completed_whenSingleValuedParameterProvidedTwice_createsError()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var arguments = new Queue<string>();
            arguments.Enqueue("-f");
            arguments.Enqueue("alpha");
            arguments.Enqueue("-f");
            arguments.Enqueue("beta");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);
            errors.Should().NotBeEmpty();
        }

        [Fact]
        public void Completed_whenMultiValuedParameterProvided_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Upload");
            var arguments = new Queue<string>();
            arguments.Enqueue("--upload");
            arguments.Enqueue("alpha");
            arguments.Enqueue("-u");
            arguments.Enqueue("beta");
            arguments.Enqueue("/u");
            arguments.Enqueue("gamma");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);

            driver.FilesToUpload.Should().BeEquivalentTo(new List<string> { "alpha", "beta", "gamma" });
        }
    }
}
