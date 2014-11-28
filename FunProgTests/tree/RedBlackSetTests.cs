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
    using System.Text;

    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RedBlackSetTests
    {
        private static string DumpSet<T>(RedBlackSet<T>.Tree s) where T : IComparable
        {
            if (s == RedBlackSet<T>.EmptyTree) return "\u2205";
            return DumpTree(s);
        }

        private static string DumpTree<T>(RedBlackSet<T>.Tree s) where T : IComparable
        {
            var results = new StringBuilder();
            results.Append('(');
            results.Append(s.Color);
            results.Append(": ");

            if (s.Tree1 != RedBlackSet<T>.EmptyTree)
            {
                results.Append(DumpTree(s.Tree1));
                results.Append(' ');
            }

            results.Append(s.Elem);

            if (s.Tree2 != RedBlackSet<T>.EmptyTree)
            {
                results.Append(' ');
                results.Append(DumpTree(s.Tree2));
            }

            results.Append(')');
            return results.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            Assert.AreEqual("∅", DumpSet(t));
        }

        [TestMethod]
        public void EmptyLeafTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            var x1 = RedBlackSet<string>.Insert("C", t);
            Assert.AreEqual("(B: C)", DumpSet(x1));
        }

        [TestMethod]
        public void DuplicateMemberTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            var x1 = RedBlackSet<string>.Insert("C", t);
            var x2 = RedBlackSet<string>.Insert("C", x1);
            Assert.AreEqual("(B: C)", DumpSet(x2));
        }

        [TestMethod]
        public void MemberTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            var x1 = RedBlackSet<string>.Insert("C", t);
            var x2 = RedBlackSet<string>.Insert("B", x1);
            Assert.IsTrue(RedBlackSet<string>.Member("B", x2));
            Assert.IsFalse(RedBlackSet<string>.Member("A", x2));
            Assert.IsFalse(RedBlackSet<string>.Member("D", x2));
        }

        [TestMethod]
        public void BlanceTest1()
        {
            const string Data = "z y x";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest2()
        {
            const string Data = "z x y";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest3()
        {
            const string Data = "x z y";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest4()
        {
            const string Data = "x y z";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest5()
        {
            const string Data = "y x z";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }

        [TestMethod]
        public void BlanceTest6()
        {
            const string Data = "y z x";
            var t = Data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }
    }
}