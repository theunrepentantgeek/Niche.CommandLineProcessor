using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void AsString_givenString_returnString()
        {
            var sample = "sample";
            var result = sample.As<string>();
            Assert.That(result, Is.EqualTo(sample));
        }

        [Test]
        public void AsInt_givenNumber_returnInt()
        {
            var sample = "42";
            var result = sample.As<int>();
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void AsColor_givenColor_returnsColor()
        {
            var sample = "Red";
            var result = sample.As<Color>();
            Assert.That(result, Is.EqualTo(Color.Red));
        }

        [Test]
        public void AsVersion_givenVersion_returnsVersion()
        {
            var sample = "1.2.3.4";
            var result = sample.As<Version>();
            Assert.That(result.ToString(4), Is.EqualTo("1.2.3.4"));
        }

        [Test]
        public void AsList_givenString_throwsException()
        {
            var sample = "sample";
            Assert.Throws<InvalidOperationException>(
                () => sample.As<List<int>>());
        }

        [Test]
        public void AsDirectoryInfo_givenStringWithoutTrailingSlash_returnsInstance()
        {
            var dir = "C:\\SampleFolder";
            var info = dir.As<DirectoryInfo>();
            Assert.That(info, Is.Not.Null);
        }

        [Test]
        public void AsDirectoryInfo_givenStringWithTrailingSlash_returnsInstance()
        {
            var dir = "C:\\SampleFolder\\";
            var info = dir.As<DirectoryInfo>();
            Assert.That(info, Is.Not.Null);
        }
    }
}
