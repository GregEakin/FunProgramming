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
    using FunProgLib.Utilities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = LinkList<string>.Empty;
            Assert.IsTrue(LinkList<string>.IsEmpty(list));
            list = LinkList<string>.Cons(list, "A");
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
            var list = Data.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);
            CollectionAssert.AreEqual(new[] { "c", "b", "a" }, list.ToList());
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);
            var list = LinkList<string>.Reverse(data);
            Console.WriteLine(list.ToReadableString());
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, list.ToList());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = LinkList<string>.Reverse(LinkList<string>.Empty);
            Assert.IsTrue(LinkList<string>.IsEmpty(list));
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
            var data = Data.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);

            var list = LinkList<string>.Cat(LinkList<string>.Empty, data);
            CollectionAssert.AreEqual(new[] { "cow?", "brown", "now,", "How" }, list.ToList());
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);

            var list = LinkList<string>.Cat(data, LinkList<string>.Empty);
            CollectionAssert.AreEqual(new[] { "cow?", "brown", "now,", "How" }, list.ToList());
        }

        [TestMethod]
        public void CatTest()
        {
            const string Data1 = "How now,";
            var data1 = Data1.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);

            const string Data2 = "brown cow?";
            var data2 = Data2.Split().Aggregate(LinkList<string>.Empty, LinkList<string>.Cons);

            var list = LinkList<string>.Cat(data1, data2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, list.ToList());
        }
    }
}