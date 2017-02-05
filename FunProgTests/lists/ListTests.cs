// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.lists
{
    using System;
    using System.Linq;

    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = List<string>.Empty;
            Assert.IsTrue(List<string>.IsEmpty(list));
            list = List<string>.Cons("A", list);
            Assert.IsFalse(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = List<string>.Empty;
            AssertThrows<ArgumentNullException>(() => List<string>.Head(list));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = List<string>.Empty;
            AssertThrows<ArgumentNullException>(() => List<string>.Tail(list));
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string Data = "a b c";
            var list = Data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
            CollectionAssert.AreEqual(new[] { "c", "b", "a" }, list.ToArray());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = List<string>.Reverse(List<string>.Empty);
            Assert.IsTrue(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void ReverseSingleListTest()
        {
            var list = List<string>.Cons("Wow", List<string>.Empty);
            var reverse = List<string>.Reverse(list);
            Assert.AreSame(list, reverse);
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
            var list = List<string>.Reverse(data);
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, list.ToArray());
        }

        [TestMethod]
        public void CatBothEmptyTest()
        {
            var list = List<string>.Cat(List<string>.Empty, List<string>.Empty);
            Assert.IsTrue(List<string>.IsEmpty(list));
        }

        [TestMethod]
        public void CatLeftEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list = List<string>.Cat(List<string>.Empty, data);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list = List<string>.Cat(data, List<string>.Empty);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatTest()
        {
            const string Data1 = "How now,";
            var data1 = Data1.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            const string Data2 = "brown cow?";
            var data2 = Data2.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list = List<string>.Cat(data1, data2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, list.ToArray());
        }
    }
}