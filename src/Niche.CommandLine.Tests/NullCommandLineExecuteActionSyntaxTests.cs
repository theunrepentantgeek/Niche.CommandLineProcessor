using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class NullCommandLineExecuteActionSyntaxTests
    {
        private readonly NullCommandLineExecuteActionSyntax<string> _syntax
            = new NullCommandLineExecuteActionSyntax<string>();

        [Fact]
        public void Execute_GivenNull_ThrowsExpectedException()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(
                    () => _syntax.Execute(null));
            exception.ParamName.Should().Be("action");
        }

        [Fact]
        public void Execute_GivenAction_DoesNotCallIt()
        {
            var called = false;
            void Action(string s) => called = true;
            _syntax.Execute(Action);
            called.Should().BeFalse();
        }
    }
}
