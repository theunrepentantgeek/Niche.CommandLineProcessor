using Niche.CommandLine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.Commandline.Tests
{
    [TestFixture]
    public class CamelCaseTests
    {
        [Test]
        public void ToDashedName_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CamelCase.ToDashedName(null));
        }

        [Test]
        public void ToDashedName_givenEmptyIdentifier_returnsEmptyString()
        {
            var camelCase = string.Empty;
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ToDashedName_givenLowercaseIdentifier_returnsSameIdentifier()
        {
            var camelCase = "sample";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo(camelCase));
        }

        [Test]
        public void ToDashedName_givenSingleUppercaseCharacter_returnsSameIdentifier()
        {
            var camelCase = "X";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("x"));
        }

        [Test]
        public void ToDashedName_givenSingleCamelCase_returnsDashedIdentifier()
        {
            var camelCase = "camelCase";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("camel-case"));
        }

        [Test]
        public void ToDashedName_givenDoubleCamelCase_returnsDashedIdentifier()
        {
            var camelCase = "dorothyTheCamel";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("dorothy-the-camel"));
        }

        [Test]
        public void ToDashedName_givenSinglePascalCase_returnsDashedIdentifier()
        {
            var camelCase = "PascalCase";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("pascal-case"));
        }

        [Test]
        public void ToDashedName_withLeadingAbbreviation_returnsDashedIdentifier()
        {
            var camelCase = "XMLFile";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("xml-file"));
        }

        [Test]
        public void ToDashedName_withEmbeddedAbbreviation_returnsDashedIdentifer()
        {
            var camelCase = "SubmitXMLFile";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("submit-xml-file"));
        }

        [Test]
        public void ToDashedName_withTrailingAbbreviation_returnsDashedIdentifier()
        {
            var camelCase = "SubmitXML";
            var dashedName = CamelCase.ToDashedName(camelCase);
            Assert.That(dashedName, Is.EqualTo("submit-xml"));
        }

        [Test]
        public void ToShortName_givenNull_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => CamelCase.ToShortName(null));
        }

        [Test]
        public void ToShortName_givenEmptyIdentifier_returnsEmptyString()
        {
            var camelCase = string.Empty;
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ToShortName_givenLowercaseIdentifier_returnsSingleCharacter()
        {
            var camelCase = "sample";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("s"));
        }

        [Test]
        public void ToShortName_givenSingleCamelCase_returnsShortName()
        {
            var camelCase = "fontFamily";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("ff"));
        }

        [Test]
        public void ToShortName_givenDoubleCamelCase_returnsShortName()
        {
            var camelCase = "testFileName";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("tfn"));
        }

        [Test]
        public void ToShortName_withLeadingAbbreviation_returnsShortName()
        {
            var camelCase = "XMLFile";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("xf"));
        }

        [Test]
        public void ToShortName_withEmbeddedAbbreviation_returnsShortName()
        {
            var camelCase = "SubmitXMLFile";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("sxf"));
        }

        [Test]
        public void ToShortName_withTrailingAbbreviation_returnsShortName()
        {
            var camelCase = "SubmitXML";
            var shortName = CamelCase.ToShortName(camelCase);
            Assert.That(shortName, Is.EqualTo("sx"));
        }
    }
}
