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
        private readonly MethodInfo _uploadMethod = typeof(SampleDriver).GetMethod("Upload");

        private Queue<string> CreateArguments(params string[] arguments)
        {
            return new Queue<string>(arguments);
        }

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
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => commandLineParameter.TryActivate(null));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void TryActivate_GivenEmptyArguments_ReturnsFalse()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var queue = new Queue<string>();
                commandLineParameter.TryActivate(queue).Should().BeFalse();
            }

            [Fact]
            public void GivenArguments_ShouldConfigureInstance()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var queue = CreateArguments("--find", "term");
                commandLineParameter.TryActivate(queue).Should().BeTrue();
            }

            [Fact]
            public void GivenArguments_ShouldCallMethodOnInstance()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var queue = CreateArguments("--find", "term");
                commandLineParameter.TryActivate(queue);
                commandLineParameter.Values.Should().BeEquivalentTo("term");
            }

            [Fact]
            public void WhenParameterIsMissingValue_ReturnsTrue()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var queue = CreateArguments("--find");
                commandLineParameter.TryActivate(queue).Should().BeTrue();
            }

            [Fact]
            public void WhenParameterIsMissingValue_ConsumesParameter()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var queue = CreateArguments("--find");
                commandLineParameter.TryActivate(queue);
                queue.Should().BeEmpty();
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
            private readonly List<string> _errors = new List<string>();

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
                commandLineParameter.Completed(_errors);
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenRequiredParameterSupplied_GeneratesNoErrors()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f", "search");
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                _errors.Should().BeEmpty();
            }

            [Fact]
            public void WhenRequiredParameterSupplied_ConfiguresValue()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f", "search");
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
            }

            [Fact]
            public void WhenParameterWithValue_ConfiguresValue()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                var arguments = CreateArguments("-f:search");
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                commandLineParameter.Values.Should().BeEquivalentTo(new List<string> { "search" });
            }

            [Fact]
            public void WhenOptionalParameterIsMissingValue_CreatesError()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _uploadMethod);
                var queue = CreateArguments("--upload");
                commandLineParameter.TryActivate(queue);
                commandLineParameter.Completed(_errors);
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenOptionalParameterOmitted_GeneratesNoErrors()
            {
                var commandLineParameter = new CommandLineParameter<string>(_driver, _uploadMethod);
                commandLineParameter.Completed(_errors);
                _errors.Should().BeEmpty();
            }

            [Fact]
            public void WhenSingleValuedParameterProvided_CallsMethod()
            {
                var arguments = CreateArguments("-f", "search");
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                _driver.TextSearch.Should().Be("search");
            }

            [Fact]
            public void WhenSingleValuedParameterProvidedTwice_createsError()
            {
                var arguments = CreateArguments("-f", "alpha", "-f", "beta");
                var commandLineParameter = new CommandLineParameter<string>(_driver, _findMethod);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenMultiValuedParameterProvided_CallsMethod()
            {
                var arguments = CreateArguments("--upload", "alpha", "-u", "beta", "/u", "gamma");
                var commandLineParameter = new CommandLineParameter<string>(_driver, _uploadMethod);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.TryActivate(arguments);
                commandLineParameter.Completed(_errors);
                _driver.FilesToUpload.Should().BeEquivalentTo(new List<string> { "alpha", "beta", "gamma" });
            }
        }
    }
}
