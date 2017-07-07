using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void AsString_givenString_returnString()
        {
            var sample = "sample";
            var result = sample.As<string>();
            result.Should().Be(sample);
        }

        [Fact]
        public void AsInt_givenNumber_returnInt()
        {
            var sample = "42";
            var result = sample.As<int>();
            result.Should().Be(42);
        }

        [Fact]
        public void AsColor_givenColor_returnsColor()
        {
            var sample = "Red";
            var result = sample.As<Color>();
            result.Should().Be(Color.Red);
        }

        [Fact]
        public void AsVersion_givenVersion_returnsVersion()
        {
            var sample = "1.2.3.4";
            var result = sample.As<Version>();
            result.ToString(4).Should().Be("1.2.3.4");
        }

        [Fact]
        public void AsList_givenString_throwsException()
        {
            var sample = "sample";
            Assert.Throws<InvalidOperationException>(
                () => sample.As<List<int>>());
        }

        [Fact]
        public void AsDirectoryInfo_givenStringWithoutTrailingSlash_returnsInstance()
        {
            var dir = "C:\\SampleFolder";
            var info = dir.As<DirectoryInfo>();
            info.Should().NotBeNull();
        }

        [Fact]
        public void AsDirectoryInfo_givenStringWithTrailingSlash_returnsInstance()
        {
            var dir = "C:\\SampleFolder\\";
            var info = dir.As<DirectoryInfo>();
            info.Should().NotBeNull();
        }

        [Fact]
        public void AsKeyValuePair_givenStringWithEquals_returnsInstance()
        {
            var value = "Name=Bob";
            var instance = value.As<KeyValuePair<string, string>>();
            instance.Should().NotBeNull();
        }

        [Fact]
        public void AsKeyValuePair_givenStringWithEquals_returnsKey()
        {
            var value = "Name=Bob";
            var instance = value.As<KeyValuePair<string, string>>();
            instance.Key.Should().Be("Name");
        }

        [Fact]
        public void AsKeyValuePair_givenStringWithEquals_returnsValue()
        {
            var value = "Name=Bob";
            var instance = value.As<KeyValuePair<string, string>>();
            instance.Value.Should().Be("Bob");
        }

        [Fact]
        public void AsKeyValuePair_givenStringWithEquals_returnsInt()
        {
            var value = "Count=42";
            var instance = value.As<KeyValuePair<string, int>>();
            instance.Value.Should().Be(42);
        }
    }
}
