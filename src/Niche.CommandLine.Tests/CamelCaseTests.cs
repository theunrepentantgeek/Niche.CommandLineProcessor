using System;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
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

            [Theory]
            [InlineData("", "")]
            [InlineData("sample", "sample")]
            [InlineData("X", "x")]
            [InlineData("camelCase", "camel-case")]
            [InlineData("PascalCase", "pascal-case")]
            [InlineData("aliceTheCamel", "alice-the-camel")]
            [InlineData("XMLFile", "xml-file")]
            [InlineData("SubmitXMLFile", "submit-xml-file")]
            [InlineData("SubmitXML", "submit-xml")]
            public void GivenString_ReturnsExpectedValue(string original, string expected)
            {
                CamelCase.ToDashedName(original).Should().Be(expected);
            }

            [Fact]
            public void GivenPascalCaseName_ReturnsDashedName()
            {
                const string name = "InputSpecification";
                var result = CamelCase.ToDashedName(name);
                result.Should().Be("input-specification");
            }
        }

        public class ToShortName : CamelCaseTests
        {
            [Fact]
            public void GivenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(
                    () => CamelCase.ToShortName(null));
            }

            [Theory]
            [InlineData("", "")]
            [InlineData("sample", "s")]
            [InlineData("X", "x")]
            [InlineData("camelCase", "cc")]
            [InlineData("PascalCase", "pc")]
            [InlineData("aliceTheCamel", "atc")]
            [InlineData("XMLFile", "xf")]
            [InlineData("SubmitXMLFile", "sxf")]
            [InlineData("SubmitXML", "sx")]
            [InlineData("fontFamily", "ff")]
            public void GivenString_ReturnsExpectedValue(string original, string expected)
            {
                CamelCase.ToShortName(original).Should().Be(expected);
            }
        }
    }
}
