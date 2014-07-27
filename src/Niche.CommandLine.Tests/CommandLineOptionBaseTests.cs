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
        public void ConvertValue_givenUnsupportedConversion_throwsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => CommandLineOptionBase.ConvertValue("fubar", typeof(CommandLineOptionBaseTests)));
        }

        [Test]
        public void ConvertValue_givenOverflow_throwsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => CommandLineOptionBase.ConvertValue("300", typeof(byte)));
        }
    }
}
