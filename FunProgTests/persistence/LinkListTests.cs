// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ListTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.persistence
{
    using System;
    using System.Linq;

    using FunProgLib.persistence;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LinkListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = LinkList<string>.Empty;
            Assert.IsTrue(LinkList<string>.IsEmpty(list));
            list = LinkList<string>.Cons("A", list);
            Assert.IsFalse(LinkList<string>.IsEmpty(list));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = LinkList<string>.Empty;
            var hd = LinkList<string>.Head(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = LinkList<string>.Empty;
            var tl = LinkList<string>.Tail(list);
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string Data = "a b c";
            var list = Data.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));
            CollectionAssert.AreEqual(new[] { "c", "b", "a" }, list.ToList());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = LinkList<string>.Reverse(LinkList<string>.Empty);
            Assert.IsTrue(LinkList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void ReverseSingleListTest()
        {
            var list = LinkList<String>.Cons("Wow", LinkList<string>.Empty);
            var reverse = LinkList<string>.Reverse(list);
            Assert.AreSame(list, reverse);
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));
            var list = LinkList<string>.Reverse(data);
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, list.ToList());
        }

        [TestMethod]
        public void CatBothEmptyTest()
        {
            var list = LinkList<string>.Cat(LinkList<string>.Empty, LinkList<string>.Empty);
            Assert.IsTrue(LinkList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void CatLeftEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));

            var list = LinkList<string>.Cat(LinkList<string>.Empty, data);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));

            var list = LinkList<string>.Cat(data, LinkList<string>.Empty);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatTest()
        {
            const string Data1 = "How now,";
            var data1 = Data1.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));

            const string Data2 = "brown cow?";
            var data2 = Data2.Split().Aggregate(LinkList<string>.Empty, (current, word) => LinkList<string>.Cons(word, current));

            var list = LinkList<string>.Cat(data1, data2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, list.ToList());
        }
    }
}