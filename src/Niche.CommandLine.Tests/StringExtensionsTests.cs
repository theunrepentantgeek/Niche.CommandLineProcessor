using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void AsString_GivenString_Returnstring()
        {
            var sample = "sample";
            sample.As<string>().Should().Be(sample);
        }

        [Fact]
        public void AsInt_GivenNumber_returnInt()
        {
            var sample = "42";
            sample.As<int>().Should().Be(42);
        }

        [Fact]
        public void AsVersion_GivenVersion_ReturnsVersion()
        {
            var sample = "1.2.3.4";
            sample.As<Version>().ToString(4).Should().Be("1.2.3.4");
        }

        [Fact]
        public void AsList_GivenString_ThrowsException()
        {
            var sample = "sample";
            Assert.Throws<InvalidOperationException>(
                () => sample.As<List<int>>());
        }

        public class AsDirectoryInfo : StringExtensionsTests
        {
            private readonly string _dir = "C:\\SampleFolder\\";

            [Fact]
            public void GivenStringWithoutTrailingSlash_ReturnsInstance()
            {
                _dir.As<DirectoryInfo>().Should().NotBeNull();
            }

            [Fact]
            public void GivenStringWithTrailingSlash_ReturnsInstance()
            {
                _dir.As<DirectoryInfo>().Should().NotBeNull();
            }
        }

        public class AsKeyValuePair : StringExtensionsTests
        {
            [Fact]
            public void GivenStringWithEquals_ReturnsInstance()
            {
                var value = "Name=Bob";
                value.As<KeyValuePair<string, string>>().Should().NotBeNull();
            }

            [Fact]
            public void GivenStringWithEquals_ReturnsKey()
            {
                var value = "Name=Bob";
                value.As<KeyValuePair<string, string>>().Key.Should().Be("Name");
            }

            [Fact]
            public void GivenStringWithEquals_ReturnsValue()
            {
                var value = "Name=Bob";
                value.As<KeyValuePair<string, string>>().Value.Should().Be("Bob");
            }

            [Fact]
            public void GivenStringWithEquals_ReturnsInt()
            {
                var value = "Count=42";
                value.As<KeyValuePair<string, int>>().Value.Should().Be(42);
            }
        }
    }
}
