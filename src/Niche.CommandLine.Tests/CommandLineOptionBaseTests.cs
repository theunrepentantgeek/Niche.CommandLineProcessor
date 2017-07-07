using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineOptionBaseTests
    {
        [Fact]
        public void FindDescription_givenMemberWithDescription_returnsDescription()
        {
            var method = typeof(SampleDriver).GetMethod("Debug");
            var description = CommandLineOptionBase.FindDescription(method);
            description.Should().Be("Show Diagnostics");
        }

        [Fact]
        public void FindDescription_givenMemberWithNoDescription_returnsEmptyString()
        {
            var method = typeof(SampleDriver).GetMethod("Verbose");
            var description = CommandLineOptionBase.FindDescription(method);
            description.Should().Be(string.Empty);
        }

    }
}
