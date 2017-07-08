using Niche.CommandLine;
using System;
using FluentAssertions;
using Xunit;

namespace Niche.Commandline.Tests
{
    public class CamelCaseTests
    {
        public class ToDashedName : CamelCaseTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => CamelCase.ToDashedName(null));
            }

            [Fact]
            public void GivenEmptyIdentifier_ReturnsEmptyString()
            {
                var camelCase = string.Empty;
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenLowercaseIdentifier_ReturnsSameIdentifier()
            {
                var camelCase = "sample";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be(camelCase);
            }

            [Fact]
            public void givenSingleUppercaseCharacter_ReturnsSameIdentifier()
            {
                var camelCase = "X";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("x");
            }

            [Fact]
            public void givenSingleCamelCase_ReturnsDashedIdentifier()
            {
                var camelCase = "camelCase";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("camel-case");
            }

            [Fact]
            public void givenDoubleCamelCase_ReturnsDashedIdentifier()
            {
                var camelCase = "dorothyTheCamel";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("dorothy-the-camel");
            }

            [Fact]
            public void givenSinglePascalCase_ReturnsDashedIdentifier()
            {
                var camelCase = "PascalCase";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("pascal-case");
            }

            [Fact]
            public void withLeadingAbbreviation_ReturnsDashedIdentifier()
            {
                var camelCase = "XMLFile";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("xml-file");
            }

            [Fact]
            public void withEmbeddedAbbreviation_ReturnsDashedIdentifer()
            {
                var camelCase = "SubmitXMLFile";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("submit-xml-file");
            }

            [Fact]
            public void withTrailingAbbreviation_ReturnsDashedIdentifier()
            {
                var camelCase = "SubmitXML";
                var dashedName = CamelCase.ToDashedName(camelCase);
                dashedName.Should().Be("submit-xml");
            }
        }

        public class ToShorName : CamelCaseTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => CamelCase.ToShortName(null));
            }

            [Fact]
            public void GivenEmptyIdentifier_ReturnsEmptyString()
            {
                var camelCase = string.Empty;
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be(string.Empty);
            }

            [Fact]
            public void GivenLowercaseIdentifier_ReturnsSingleCharacter()
            {
                var camelCase = "sample";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("s");
            }

            [Fact]
            public void GivenSingleCamelCase_ReturnsShortName()
            {
                var camelCase = "fontFamily";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("ff");
            }

            [Fact]
            public void GivenDoubleCamelCase_ReturnsShortName()
            {
                var camelCase = "testFileName";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("tfn");
            }

            [Fact]
            public void WithLeadingAbbreviation_ReturnsShortName()
            {
                var camelCase = "XMLFile";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("xf");
            }

            [Fact]
            public void WithEmbeddedAbbreviation_ReturnsShortName()
            {
                var camelCase = "SubmitXMLFile";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("sxf");
            }

            [Fact]
            public void WithTrailingAbbreviation_ReturnsShortName()
            {
                var camelCase = "SubmitXML";
                var shortName = CamelCase.ToShortName(camelCase);
                shortName.Should().Be("sx");
            }
        }
    }
}
