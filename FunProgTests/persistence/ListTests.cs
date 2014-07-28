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
    }
}