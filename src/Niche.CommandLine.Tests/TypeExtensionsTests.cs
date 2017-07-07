using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsEnumerable_givenIEnumerableString_returnsTrue()
        {
            typeof(IEnumerable<string>).IsIEnumerable().Should().BeTrue();
        }

        [Fact]
        public void IsEnumerable_givenInt_returnsFalse()
        {
            typeof(int).IsIEnumerable().Should().BeFalse();
        }

        [Fact]
        public void IsEnumerable_givenIListString_returnsTrue()
        {
            typeof(IList<string>).IsIEnumerable().Should().BeTrue();
        }

        [Fact]
        public void IsEnumerable_givenICollectionString_returnsTrue()
        {
            typeof(ICollection<string>).IsIEnumerable().Should().BeTrue();
        }

        [Fact]
        public void IsEnumerable_givenListString_returnsTrue()
        {
            typeof(List<string>).IsIEnumerable().Should().BeTrue();
        }

        [Fact]
        public void IsEnumerable_givenString_returnsFalse()
        {
            typeof(string).IsIEnumerable().Should().BeFalse();
        }

        [Fact]
        public void GetIEnumerableItemType_givenIEnumerableString_returnsString()
        {
            typeof(IEnumerable<string>).GetIEnumerableItemType().Should().Be(typeof(string));
        }

        [Fact]
        public void GetIEnumerableItemType_givenIListInt_returnsInt()
        {
            typeof(IList<int>).GetIEnumerableItemType().Should().Be(typeof(int));
        }

        [Fact]
        public void GetIEnumerableItemType_givenListString_returnsInt()
        {
            typeof(List<string>).GetIEnumerableItemType().Should().Be(typeof(string));
        }

        [Fact]
        public void GetIEnumerableItemType_givenString_returnsNull()
        {
            typeof(string).GetIEnumerableItemType().Should().BeNull();
        }

        [Fact]
        public void IsKeyValuePair_givenString_returnsFalse()
        {
            typeof(string).IsKeyValuePair().Should().BeFalse();
        }

        [Fact]
        public void IsKeyValuePair_givenInteger_returnsFalse()
        {
            typeof(int).IsKeyValuePair().Should().BeFalse();
        }

        [Fact]
        public void IsKeyValuePair_givenKeyValuePair_returnsFalse()
        {
            typeof(KeyValuePair<string, string>).IsKeyValuePair().Should().BeTrue();
        }

        [Fact]
        public void GetKeyValueKeyType_givenString_returnsNull()
        {
            typeof(string).GetKeyValueKeyType().Should().BeNull();
        }

        [Fact]
        public void GetKeyValueKeyType_givenKeyValuePair_returnsNull()
        {
            typeof(KeyValuePair<string,int>).GetKeyValueKeyType().Should().Be(typeof(string));
        }

        [Fact]
        public void GetKeyValueValueType_givenString_returnsNull()
        {
            typeof(string).GetKeyValueValueType().Should().BeNull();
            
        }

        [Fact]
        public void GetKeyValueValueType_givenKeyValuePair_returnsNull()
        {
            typeof(KeyValuePair<string, int>).GetKeyValueValueType().Should().Be(typeof(int));
        }
    }
}
