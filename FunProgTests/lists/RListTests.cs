// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.lists;
using FunProgLib.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static FunProgTests.utilities.ExpectedException;

namespace FunProgTests.lists
{
    [TestClass]
    public class RListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = RList<string>.Empty;
            Assert.IsTrue(RList<string>.IsEmpty(list));
            list = RList<string>.Cons("A", list);
            Assert.IsFalse(RList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = RList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RList<string>.Head(list));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = RList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RList<string>.Tail(list));
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string data = "a b c";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
            Assert.AreEqual("[c, b, a]", list.ToReadableString());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = RList<string>.Reverse(RList<string>.Empty);
            Assert.IsTrue(RList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void ReverseSingleListTest()
        {
            var list = RList<string>.Cons("Wow", RList<string>.Empty);
            var reverse = RList<string>.Reverse(list);
            Assert.AreSame(list, reverse);
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
            var reverse = RList<string>.Reverse(list);
            Assert.AreEqual("[How, now,, brown, cow?]", reverse.ToReadableString());
        }

        [TestMethod]
        public void CatBothEmptyTest()
        {
            var list = RList<string>.Cat(RList<string>.Empty, RList<string>.Empty);
            Assert.IsTrue(RList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void CatLeftEmptyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var list2 = RList<string>.Cat(RList<string>.Empty, list);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var list2 = RList<string>.Cat(list, RList<string>.Empty);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatTest()
        {
            const string data1 = "How now,";
            var list1 = data1.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            const string data2 = "brown cow?";
            var list2 = data2.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var list3 = RList<string>.Cat(list1, list2);
            Assert.AreEqual("[now,, How, cow?, brown]", list3.ToReadableString());
        }

        [TestMethod]
        public void LookupEmptyTest()
        {
            var exception = AssertThrows<ArgumentNullException>(() => RList<string>.Lookup(0, RList<string>.Empty));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: list", exception.Message);
        }

        [TestMethod]
        public void LookupNegativeTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var exception = AssertThrows<ArgumentException>(() => RList<string>.Lookup(-1, list));
            Assert.AreEqual("neg\r\nParameter name: i", exception.Message);
        }

        [TestMethod]
        public void LookupZeroTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var item = RList<string>.Lookup(0, list);
            Assert.AreEqual("cow?", item);
        }

        [TestMethod]
        public void LookupOneTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var item = RList<string>.Lookup(1, list);
            Assert.AreEqual("brown", item);
        }

        [TestMethod]
        public void UpdateEmptyTest()
        {
            var exception = AssertThrows<ArgumentNullException>(() => RList<string>.Fupdate(null, 0, RList<string>.Empty));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: ts", exception.Message);
        }

        [TestMethod]
        public void UpdateNegativeTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

            var exception = AssertThrows<ArgumentException>(() => RList<string>.Fupdate(null, -1, list));
            Assert.AreEqual("Negative\r\nParameter name: i", exception.Message);
        }
    }
}