using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class TypeExtensionsTests
    {
        public class IsEnumerable : TypeExtensionsTests
        {
            [Fact]
            public void GivenIEnumerableString_ReturnsTrue()
            {
                typeof(IEnumerable<string>).IsIEnumerable().Should().BeTrue();
            }

            [Fact]
            public void GivenInt_ReturnsFalse()
            {
                typeof(int).IsIEnumerable().Should().BeFalse();
            }

            [Fact]
            public void GivenIListString_ReturnsTrue()
            {
                typeof(IList<string>).IsIEnumerable().Should().BeTrue();
            }

            [Fact]
            public void GivenICollectionString_ReturnsTrue()
            {
                typeof(ICollection<string>).IsIEnumerable().Should().BeTrue();
            }

            [Fact]
            public void GivenListString_ReturnsTrue()
            {
                typeof(List<string>).IsIEnumerable().Should().BeTrue();
            }

            [Fact]
            public void GivenString_ReturnsFalse()
            {
                typeof(string).IsIEnumerable().Should().BeFalse();
            }
        }

        public class GetIEnumerableItemType : TypeExtensionsTests
        {
            [Fact]
            public void GivenIEnumerableString_ReturnsString()
            {
                typeof(IEnumerable<string>).GetIEnumerableItemType().Should().Be(typeof(string));
            }

            [Fact]
            public void GivenIListInt_ReturnsInt()
            {
                typeof(IList<int>).GetIEnumerableItemType().Should().Be(typeof(int));
            }

            [Fact]
            public void GivenListString_ReturnsInt()
            {
                typeof(List<string>).GetIEnumerableItemType().Should().Be(typeof(string));
            }

            [Fact]
            public void GivenString_ReturnsNull()
            {
                typeof(string).GetIEnumerableItemType().Should().BeNull();
            }
        }

        public class IsKeyValuePair : TypeExtensionsTests
        {
            [Fact]
            public void GivenString_ReturnsFalse()
            {
                typeof(string).IsKeyValuePair().Should().BeFalse();
            }

            [Fact]
            public void GivenInteger_ReturnsFalse()
            {
                typeof(int).IsKeyValuePair().Should().BeFalse();
            }

            [Fact]
            public void GivenKeyValuePair_ReturnsFalse()
            {
                typeof(KeyValuePair<string, string>).IsKeyValuePair().Should().BeTrue();
            }
        }

        public class GetKeyValueKeyType : TypeExtensionsTests
        {
            [Fact]
            public void GivenString_ReturnsNull()
            {
                typeof(string).GetKeyValueKeyType().Should().BeNull();
            }

            [Fact]
            public void GivenKeyValuePair_ReturnsNull()
            {
                typeof(KeyValuePair<string, int>).GetKeyValueKeyType().Should().Be(typeof(string));
            }
        }

        public class GetKeyValueValueType : TypeExtensionsTests
        {
            [Fact]
            public void GivenString_ReturnsNull()
            {
                typeof(string).GetKeyValueValueType().Should().BeNull();

            }

            [Fact]
            public void GivenKeyValuePair_ReturnsNull()
            {
                typeof(KeyValuePair<string, int>).GetKeyValueValueType().Should().Be(typeof(int));
            }
        }
    }
}
