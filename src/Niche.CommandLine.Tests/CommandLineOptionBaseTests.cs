using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineOptionBaseTests
    {
        [Test]
        public void FindDescription_givenMemberWithDescription_returnsDescription()
        {
            var method = typeof(SampleDriver).GetMethod("Debug");
            var description = CommandLineOptionBase.FindDescription(method);
            description.Should().Be("Show Diagnostics");
        }

        [Test]
        public void FindDescription_givenMemberWithNoDescription_returnsEmptyString()
        {
            var method = typeof(SampleDriver).GetMethod("Verbose");
            var description = CommandLineOptionBase.FindDescription(method);
            description.Should().Be(string.Empty);
        }

    }
}
