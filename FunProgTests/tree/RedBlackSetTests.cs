// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		RedBlackSetTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.tree
{
    using System;
    using System.Linq;

    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RedBlackSetTests
    {
        private static string DumpSet<T>(RedBlackSet<T>.Tree s) where T : IComparable
        {
            if (s == RedBlackSet<T>.EmptyTree) return "\u2205";
            return s.ToString();
        }

        [TestMethod]
        public void MemberTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            var x1 = RedBlackSet<string>.Insert("C", t);
            var x2 = RedBlackSet<string>.Insert("B", x1);
            Assert.IsTrue(RedBlackSet<string>.Member("B", x2));
            Assert.IsFalse(RedBlackSet<string>.Member("A", x2));
        }

        [TestMethod]
        public void BlanceTest1()
        {
            const string Data = "z y x";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest2()
        {
            const string Data = "z x y";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest3()
        {
            const string Data = "x z y";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest4()
        {
            const string Data = "x y z";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest5()
        {
            const string Data = "y x z";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest6()
        {
            const string Data = "y z x";
            var t = Data.Split(null).Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }
    }
}