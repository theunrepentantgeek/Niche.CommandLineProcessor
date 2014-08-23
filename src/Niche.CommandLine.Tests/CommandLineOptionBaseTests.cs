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
            var method = typeof(SampleDriver).GetMethod("Help");
            var description = CommandLineOptionBase.FindDescription(method);
            Assert.That(description, Is.EqualTo("Show Help"));
        }

        [Test]
        public void FindDescription_givenMemberWithNoDescription_returnsEmptyString()
        {
            var method = typeof(SampleDriver).GetMethod("Verbose");
            var description = CommandLineOptionBase.FindDescription(method);
            Assert.That(description, Is.EqualTo(string.Empty));
        }

    }
}
