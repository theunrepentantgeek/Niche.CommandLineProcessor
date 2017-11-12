using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineExecuteFuncSyntaxTests
    {
        private readonly LoggingOptions _options = new LoggingOptions();

        private readonly List<string> _arguments = new List<string>();

        private readonly List<string> _errors = new List<string>();

        private readonly int _exitCode = -1;

        public class Constructor : CommandLineExecuteFuncSyntaxTests
        {
            [Fact]
            public void GivenNullOptions_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineExecuteFuncSyntax<LoggingOptions>(null, _arguments, _errors, _exitCode));
                exception.ParamName.Should().Be("options");
            }

            [Fact]
            public void GivenNullArguments_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineExecuteFuncSyntax<LoggingOptions>(_options, null, _errors, _exitCode));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void GivenNullErrorsReference_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineExecuteFuncSyntax<LoggingOptions>(_options, _arguments, null, _exitCode));
                exception.ParamName.Should().Be("errorsReference");
            }
        }

        public class ExecuteFuncWithOptions : CommandLineExecuteFuncSyntaxTests
        {
            private readonly CommandLineExecuteFuncSyntax<LoggingOptions> _syntax;
            private readonly Func<LoggingOptions, int> _func;

            public ExecuteFuncWithOptions()
            {
                _syntax = new CommandLineExecuteFuncSyntax<LoggingOptions>(_options, _arguments, _errors, _exitCode);
                _func = Substitute.For<Func<LoggingOptions, int>>();
            }

            [Fact]
            [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
            public void GivenNullFunc_ThrowsException()
            {
                Func<LoggingOptions, int> func = null;
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _syntax.Execute(func));
                exception.ParamName.Should().Be("func");
            }

            [Fact]
            public void GivenFunc_CallsFunc()
            {
                _syntax.Execute(_func);
                _func.Received()(_options);
            }

            [Fact]
            public void GivenFuncReturningValue_ReturnsValue()
            {
                var exitCode = -4;
                _syntax.Execute(o => exitCode).Should().Be(exitCode);
            }

            [Fact]
            public void WhenFuncThrows_CollectsErrors()
            {
                _syntax.Execute(options => throw new InvalidOperationException());
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenArgumentsPresent_DoesNotCallFunc()
            {
                _arguments.Add("argument");
                _syntax.Execute(_func);
                _func.DidNotReceive()(_options);
            }

            [Fact]
            public void WhenArgumentsPresent_CollectsErrors()
            {
                // Reminder - arguments are plain values
                _arguments.Add("argument");
                _syntax.Execute(_func);
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenOptionsPresent_CollectsErrors()
            {
                // Reminder - Options start with -- or - or /
                _arguments.Add("--option");
                _syntax.Execute(_func);
                _errors.Should().NotBeEmpty();
            }
        }

        public class ExecuteFuncWithOptionsAndArguments : CommandLineExecuteFuncSyntaxTests
        {
            private readonly CommandLineExecuteFuncSyntax<LoggingOptions> _syntax;
            private readonly Func<LoggingOptions, IEnumerable<string>, int> _func;

            public ExecuteFuncWithOptionsAndArguments()
            {
                _syntax = new CommandLineExecuteFuncSyntax<LoggingOptions>(_options, _arguments, _errors, _exitCode);
                _func = Substitute.For<Func<LoggingOptions, IEnumerable<string>, int>>();
            }

            [Fact]
            [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
            public void GivenNullFunc_ThrowsException()
            {
                Func<LoggingOptions, IEnumerable<string>, int> func = null;
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _syntax.Execute(func));
                exception.ParamName.Should().Be("func");
            }

            [Fact]
            public void GivenFunc_CallsFunc()
            {
                _syntax.Execute(_func);
                _func.Received()(_options, _arguments);
            }

            [Fact]
            public void GivenFuncReturningValue_ReturnsValue()
            {
                var exitCode = -4;
                _syntax.Execute((o, a) => exitCode).Should().Be(exitCode);
            }

            [Fact]
            public void WhenFuncThrows_CollectsErrors()
            {
                _syntax.Execute((o, a) => throw new InvalidOperationException());
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenArgumentsPresent_CallsFuncWithOptionsAndArguments()
            {
                _arguments.Add("argument");
                _syntax.Execute(_func);
                _func.Received()(_options, _arguments);
            }

            [Fact]
            public void WhenOptionsPresent_CollectsErrors()
            {
                // Reminder - Options start with '--' or '-' or '/'
                _arguments.Add("--option");
                _syntax.Execute(_func);
                _errors.Should().NotBeEmpty();
            }

            [Fact]
            public void WhenErrorsPresent_DoesNotCallFunc()
            {
                _errors.Add("Died");
                _arguments.Add("argument");
                _syntax.Execute(_func);
                _func.DidNotReceive()(_options, _arguments);
            }
        }
    }
}
