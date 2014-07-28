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
            var list = List<string>.Empty;
            Assert.IsTrue(List<string>.IsEmpty(list));
            list = List<string>.Cons(list, "A");
            Assert.IsFalse(List<string>.IsEmpty(list));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = List<string>.Empty;
            var hd = List<string>.Head(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = List<string>.Empty;
            var tl = List<string>.Tail(list);
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string Data = "a b c";
            var list = Data.Split().Aggregate(List<string>.Empty, List<string>.Cons);
            CollectionAssert.AreEqual(new[] { "c", "b", "a" }, list.ToList());
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List<string>.Empty, List<string>.Cons);
            var list = List<string>.Reverse(data);
            Console.WriteLine(list.ToReadableString());
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, list.ToList());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = List<string>.Reverse(List<string>.Empty);
            Assert.IsTrue(List<string>.IsEmpty(list));
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
            var data = Data.Split().Aggregate(List<string>.Empty, List<string>.Cons);

            var list = List<string>.Cat(List<string>.Empty, data);
            CollectionAssert.AreEqual(new[] { "cow?", "brown", "now,", "How" }, list.ToList());
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List<string>.Empty, List<string>.Cons);

            var list = List<string>.Cat(data, List<string>.Empty);
            CollectionAssert.AreEqual(new[] { "cow?", "brown", "now,", "How" }, list.ToList());
        }

        [TestMethod]
        public void CatTest()
        {
            const string Data1 = "How now,";
            var data1 = Data1.Split().Aggregate(List<string>.Empty, List<string>.Cons);

            const string Data2 = "brown cow?";
            var data2 = Data2.Split().Aggregate(List<string>.Empty, List<string>.Cons);

            var list = List<string>.Cat(data1, data2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, list.ToList());
        }
    }
}