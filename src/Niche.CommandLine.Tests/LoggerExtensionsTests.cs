using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
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
                var expected = new List<string> { "alpha   beta   gamma", "one     two    three" };
                result.Should().BeEquivalentTo(expected);
            }
        }

        public class Failure : LoggerExtensionsTests
        {
            [Fact]
            public void GivenSimpleException_logsOneMessage()
            {
                var logger = Substitute.For<ILogger>();
                var ex = new Exception("This is an exception.");
                logger.Failure(ex);
                logger.Received(1).Failure(Arg.Any<string>());
            }

            [Fact]
            public void GivenNestedException_logsExpectedMessageCount()
            {
                var logger = Substitute.For<ILogger>();
                var inner = new InvalidOperationException();
                var ex = new Exception("This is an exception.", inner);
                logger.Failure(ex);
                logger.Received(2).Failure(Arg.Any<string>());
            }

            [Fact]
            public void GivenSimpleExceptionWithData_logsExpectedMessageCount()
            {
                var logger = Substitute.For<ILogger>();
                var ex = new Exception("This is an exception.");
                ex.Data["name"] = "George";
                ex.Data["repetitions"] = 45;
                logger.Failure(ex);
                logger.Received(3).Failure(Arg.Any<string>());
            }
        }
    }
}
