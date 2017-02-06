﻿// Fun Programming Data Structures 1.0
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
            const string data = "a b c";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
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
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));
            var reverse = List<string>.Reverse(list);
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, reverse.ToArray());
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
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list2 = List<string>.Cat(List<string>.Empty, list);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list2 = List<string>.Cat(list, List<string>.Empty);
            Assert.AreSame(list, list2);
        }

        [TestMethod]
        public void CatTest()
        {
            const string data1 = "How now,";
            var list1 = data1.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            const string data2 = "brown cow?";
            var list2 = data2.Split().Aggregate(List<string>.Empty, (current, word) => List<string>.Cons(word, current));

            var list3 = List<string>.Cat(list1, list2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, list3.ToArray());
        }
    }
}