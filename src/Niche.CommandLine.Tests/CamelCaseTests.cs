using Niche.CommandLine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Niche.Commandline.Tests
{
    public class CamelCaseTests
    {
        public void ToDashedName_givenNull_throwsException()
        [Fact]
        {
            Assert.Throws<ArgumentNullException>(
                () => CamelCase.ToDashedName(null));
        }

        public void ToDashedName_givenEmptyIdentifier_returnsEmptyString()
        [Fact]
        {
            var camelCase = string.Empty;
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be(string.Empty);
        }

        public void ToDashedName_givenLowercaseIdentifier_returnsSameIdentifier()
        [Fact]
        {
            var camelCase = "sample";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be(camelCase);
        }

        public void ToDashedName_givenSingleUppercaseCharacter_returnsSameIdentifier()
        [Fact]
        {
            var camelCase = "X";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("x");
        }

        public void ToDashedName_givenSingleCamelCase_returnsDashedIdentifier()
        [Fact]
        {
            var camelCase = "camelCase";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("camel-case");
        }

        public void ToDashedName_givenDoubleCamelCase_returnsDashedIdentifier()
        [Fact]
        {
            var camelCase = "dorothyTheCamel";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("dorothy-the-camel");
        }

        public void ToDashedName_givenSinglePascalCase_returnsDashedIdentifier()
        [Fact]
        {
            var camelCase = "PascalCase";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("pascal-case");
        }

        public void ToDashedName_withLeadingAbbreviation_returnsDashedIdentifier()
        [Fact]
        {
            var camelCase = "XMLFile";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("xml-file");
        }

        public void ToDashedName_withEmbeddedAbbreviation_returnsDashedIdentifer()
        [Fact]
        {
            var camelCase = "SubmitXMLFile";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("submit-xml-file");
        }

        public void ToDashedName_withTrailingAbbreviation_returnsDashedIdentifier()
        [Fact]
        {
            var camelCase = "SubmitXML";
            var dashedName = CamelCase.ToDashedName(camelCase);
            dashedName.Should().Be("submit-xml");
        }

        public void ToShortName_givenNull_throwsException()
        [Fact]
        {
            Assert.Throws<ArgumentNullException>(
                () => CamelCase.ToShortName(null));
        }

        public void ToShortName_givenEmptyIdentifier_returnsEmptyString()
        [Fact]
        {
            var camelCase = string.Empty;
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be(string.Empty);
        }

        public void ToShortName_givenLowercaseIdentifier_returnsSingleCharacter()
        [Fact]
        {
            var camelCase = "sample";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("s");
        }

        public void ToShortName_givenSingleCamelCase_returnsShortName()
        [Fact]
        {
            var camelCase = "fontFamily";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("ff");
        }

        public void ToShortName_givenDoubleCamelCase_returnsShortName()
        [Fact]
        {
            var camelCase = "testFileName";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("tfn");
        }

        public void ToShortName_withLeadingAbbreviation_returnsShortName()
        [Fact]
        {
            var camelCase = "XMLFile";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("xf");
        }

        public void ToShortName_withEmbeddedAbbreviation_returnsShortName()
        [Fact]
        {
            var camelCase = "SubmitXMLFile";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("sxf");
        }

        public void ToShortName_withTrailingAbbreviation_returnsShortName()
        [Fact]
        {
            var camelCase = "SubmitXML";
            var shortName = CamelCase.ToShortName(camelCase);
            shortName.Should().Be("sx");
        }
    }
}
