using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineParameterTests
    {
        private readonly SampleDriver _driver = new SampleDriver();
        private readonly MethodInfo _findMethod = typeof(SampleDriver).GetMethod("Find");

        public class Constructor : CommandLineParameterTests
        {
            [Fact]
            public void Constructor_MissingInstance_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineParameter<string>(null, _findMethod));
                exception.ParamName.Should().Be("instance");
            }

            [Fact]
            public void Constructor_MissingMethod_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineParameter<string>(_driver, null));
                exception.ParamName.Should().Be("method");
            }

            [Fact]
            public void Constructor_ifMethodDoesNotApplyToInstance_ThrowsException()
            {
                var method = _driver.GetType().GetMethod("Find");
                var exception =
                    Assert.Throws<ArgumentException>(
                        () => new CommandLineParameter<string>(this, method));
                exception.ParamName.Should().Be("method");
            }
        }

        public class TryActivate : CommandLineParameterTests
        {
            [Fact]
            public void TryActivate_GivenNull_ThrowsException()
            {
                var method = _driver.GetType().GetMethod("Find");
                var commandLineParameter = new CommandLineParameter<string>(_driver, method);
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => commandLineParameter.TryActivate(null));
                exception.ParamName.Should().Be("arguments");
            }
        }

        public class ConfigureParameters : CommandLineParameterTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => CommandLineOptionFactory.CreateParameters(null));
            }
        }

        public class CreateHelp : CommandLineParameterTests
        {
            [Fact]
            public void GivenList_AddsEntry()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var help = commandLineParameter.CreateHelp().ToList();
                help.Should().HaveCount(1);
            }
        }

        public class Completed : CommandLineParameterTests
        {
            [Fact]
            public void WithNoErrorList_ThrowsException()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                Assert.Throws<ArgumentNullException>(
                    () => commandLineParameter.Completed(null));
            }

            [Fact]
            public void WhenRequiredParameterOmitted_GeneratesError()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenRequiredParameterSupplied_GeneratesNoErrors()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f", "search");
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                errors.Should().BeEmpty();
            }


            [Fact]
            public void WhenRequiredParameterSupplied_ConfiguresValue()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f", "search");
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
            }

            [Fact]
            public void WhenParameterWithValue_ConfiguresValue()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f:search");
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
            }

            [Fact]
            public void WhenOptionalParameterOmitted_GeneratesNoErrors()
            {
                var uploadMethod = typeof(SampleDriver).GetMethod("Upload");
                var commandLineParameter = new CommandLineParameter<string>(_driver, uploadMethod);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                errors.Should().BeEmpty();
            }

            [Fact]
            public void WhenSingleValuedParameterProvided_CallsMethod()
            {
                var arguments = CreateArguments("-f", "search");
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                _driver.TextSearch.Should().Be("search");
            }

            [Fact]
            public void WhenSingleValuedParameterProvidedTwice_createsError()
            {
                var method = _driver.GetType().GetMethod("Find");
                var arguments = CreateArguments("-f", "alpha", "-f", "beta");
                var commandLineParameter = new CommandLineParameter<string>(_driver, method);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenMultiValuedParameterProvided_CallsMethod()
            {
                var method = _driver.GetType().GetMethod("Upload");
                var arguments = CreateArguments("--upload", "alpha", "-u", "beta", "/u", "gamma");
                var commandLineParameter = new CommandLineParameter<string>(_driver, method);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                var errors = new List<string>();
                commandLineParameter.Completed(errors);
                _driver.FilesToUpload.Should().BeEquivalentTo(new List<string> { "alpha", "beta", "gamma" });
            }
        }

        private Queue<string> CreateArguments(params string[] arguments)
        {
            return new Queue<string>(arguments);
        }
    }
}
