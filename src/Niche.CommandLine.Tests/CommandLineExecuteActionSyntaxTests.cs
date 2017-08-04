using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineExecuteActionSyntaxTests
    {
        private readonly LoggingOptions _options = new LoggingOptions();

        private readonly List<string> _errors = new List<string>();

        public class Constructor : CommandLineExecuteActionSyntaxTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineExecuteActionSyntax<LoggingOptions>(null, _errors));
                exception.ParamName.Should().Be("options");
            }

            [Fact]
            public void GivenNullErrors_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineExecuteActionSyntax<LoggingOptions>(_options, null));
                exception.ParamName.Should().Be("errorsReference");
            }
        }

        public class Execute : CommandLineExecuteActionSyntaxTests
        {
            private readonly CommandLineExecuteActionSyntax<LoggingOptions> _syntax;

            public Execute()
            {
                _syntax = new CommandLineExecuteActionSyntax<LoggingOptions>(_options, _errors);
            }

            [Fact]
            public void GivenNullAction_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _syntax.Execute(null));
                exception.ParamName.Should().Be("action");
            }

            [Fact]
            public void GivenAction_CallsAction()
            {
                var action = Substitute.For < Action<LoggingOptions>>();
                _syntax.Execute(action);
                action.Received()(_options);
            }

            [Fact]
            public void WhenActionThrows_CollectsErrors()
            {
                _syntax.Execute(options => throw new InvalidOperationException());
                _errors.Should().NotBeEmpty();
            }
        }
    }
}
