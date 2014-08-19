using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void IsEnumerable_givenIEnumerableString_returnsTrue()
        {
            Assert.That( typeof(IEnumerable<string>).IsIEnumerable(), Is.True);
        }

        [Test]
        public void IsEnumerable_givenInt_returnsFalse()
        {
            Assert.That(typeof(int).IsIEnumerable(), Is.False);
        }

        [Test]
        public void IsEnumerable_givenIListString_returnsTrue()
        {
            Assert.That(typeof(IList<string>).IsIEnumerable(), Is.True);
        }

        [Test]
        public void IsEnumerable_givenICollectionString_returnsTrue()
        {
            Assert.That(typeof(ICollection<string>).IsIEnumerable(), Is.True);
        }

        [Test]
        public void IsEnumerable_givenListString_returnsTrue()
        {
            Assert.That(typeof(List<string>).IsIEnumerable(), Is.True);
        }

        [Test]
        public void IsEnumerable_givenString_returnsFalse()
        {
            Assert.That(typeof(string).IsIEnumerable(), Is.False);
        }

        [Test]
        public void GetIEnumerableItemType_givenIEnumerableString_returnsString()
        {
            Assert.That(typeof(IEnumerable<string>).GetIEnumerableItemType(), Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetIEnumerableItemType_givenIListInt_returnsInt()
        {
            Assert.That(typeof(IList<int>).GetIEnumerableItemType(), Is.EqualTo(typeof(int)));
        }

        [Test]
        public void GetIEnumerableItemType_givenListString_returnsInt()
        {
            Assert.That(typeof(List<string>).GetIEnumerableItemType(), Is.EqualTo(typeof(string)));
        }

        [Test]
        public void GetIEnumerableItemType_givenString_returnsNull()
        {
            Assert.That(typeof(string).GetIEnumerableItemType(), Is.Null);
        }
    }
}
