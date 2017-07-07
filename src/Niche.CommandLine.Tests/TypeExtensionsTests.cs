using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void IsEnumerable_givenIEnumerableString_returnsTrue()
        {
            typeof(IEnumerable<string>).IsIEnumerable().Should().BeTrue();
        }

        [Test]
        public void IsEnumerable_givenInt_returnsFalse()
        {
            typeof(int).IsIEnumerable().Should().BeFalse();
        }

        [Test]
        public void IsEnumerable_givenIListString_returnsTrue()
        {
            typeof(IList<string>).IsIEnumerable().Should().BeTrue();
        }

        [Test]
        public void IsEnumerable_givenICollectionString_returnsTrue()
        {
            typeof(ICollection<string>).IsIEnumerable().Should().BeTrue();
        }

        [Test]
        public void IsEnumerable_givenListString_returnsTrue()
        {
            typeof(List<string>).IsIEnumerable().Should().BeTrue();
        }

        [Test]
        public void IsEnumerable_givenString_returnsFalse()
        {
            typeof(string).IsIEnumerable().Should().BeFalse();
        }

        [Test]
        public void GetIEnumerableItemType_givenIEnumerableString_returnsString()
        {
            typeof(IEnumerable<string>).GetIEnumerableItemType().Should().Be(typeof(string));
        }

        [Test]
        public void GetIEnumerableItemType_givenIListInt_returnsInt()
        {
            typeof(IList<int>).GetIEnumerableItemType().Should().Be(typeof(int));
        }

        [Test]
        public void GetIEnumerableItemType_givenListString_returnsInt()
        {
            typeof(List<string>).GetIEnumerableItemType().Should().Be(typeof(string));
        }

        [Test]
        public void GetIEnumerableItemType_givenString_returnsNull()
        {
            typeof(string).GetIEnumerableItemType().Should().BeNull();
        }

        [Test]
        public void IsKeyValuePair_givenString_returnsFalse()
        {
            typeof(string).IsKeyValuePair().Should().BeFalse();
        }

        [Test]
        public void IsKeyValuePair_givenInteger_returnsFalse()
        {
            typeof(int).IsKeyValuePair().Should().BeFalse();
        }

        [Test]
        public void IsKeyValuePair_givenKeyValuePair_returnsFalse()
        {
            typeof(KeyValuePair<string, string>).IsKeyValuePair().Should().BeTrue();
        }

        [Test]
        public void GetKeyValueKeyType_givenString_returnsNull()
        {
            typeof(string).GetKeyValueKeyType().Should().BeNull();
        }

        [Test]
        public void GetKeyValueKeyType_givenKeyValuePair_returnsNull()
        {
            typeof(KeyValuePair<string,int>).GetKeyValueKeyType().Should().Be(typeof(string));
        }

        [Test]
        public void GetKeyValueValueType_givenString_returnsNull()
        {
            typeof(string).GetKeyValueValueType().Should().BeNull();
        }

        [Test]
        public void GetKeyValueValueType_givenKeyValuePair_returnsNull()
        {
            typeof(KeyValuePair<string, int>).GetKeyValueValueType().Should().Be(typeof(int));
        }
    }
}
