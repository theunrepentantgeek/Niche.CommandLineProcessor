using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Niche.ConsoleLogging.Tests
{
    public class LoggerExtensionsTests
    {
        public class Tablefy : LoggerExtensionsTests
        {
            [Fact]
            public void GivenSimpleString_ReturnsString()
            {
                var original = new List<string> { "sample" };
                var result = LoggerExtensions.Tablefy(original);
                result.Should().BeEquivalentTo(original);
            }

            [Fact]
            public void GivenMultipleSimpleStrings_ReturnsStrings()
            {
                var original = new List<string> { "alpha", "beta", "gamma" };
                var result = LoggerExtensions.Tablefy(original);
                result.Should().BeEquivalentTo(original);
            }

            [Fact]
            public void GivenTabbedString_ReturnsConvertedString()
            {
                var original = new List<string> { "alpha\tbeta\tgamma" };
                var result = LoggerExtensions.Tablefy(original);
                result.Single().Should().Be("alpha   beta   gamma");
            }

            [Fact]
            public void GivenTwoTabbedStrings_ReturnsFormattedText()
            {
                var original = new List<string> { "alpha\tbeta\tgamma", "one\ttwo\tthree" };
                var result = LoggerExtensions.Tablefy(original);
                var expected = new List<string>
                {
                    "alpha   beta   gamma",
                    "one     two    three"
                };

                result.Should().BeEquivalentTo(expected);
            }
        }

        public class Failure : LoggerExtensionsTests
        {
            private readonly ILogger _logger = Substitute.For<ILogger>();

            [Fact]
            public void GivenSimpleException_logsOneMessage()
            {
                var ex = new Exception("This is an exception.");
                _logger.Failure(ex);
                _logger.Received(1).Failure(Arg.Any<string>());
            }

            [Fact]
            public void GivenNestedException_logsExpectedMessageCount()
            {
                var inner = new InvalidOperationException();
                var ex = new Exception("This is an exception.", inner);
                _logger.Failure(ex);
                _logger.Received(2).Failure(Arg.Any<string>());
            }

            [Fact]
            public void GivenSimpleExceptionWithData_logsExpectedMessageCount()
            {
                var ex = new Exception("This is an exception.");
                ex.Data["name"] = "George";
                ex.Data["repetitions"] = 45;
                _logger.Failure(ex);
                _logger.Received(3).Failure(Arg.Any<string>());
            }
        }
    }
}
