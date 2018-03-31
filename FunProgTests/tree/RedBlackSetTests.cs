// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

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
        private static string DumpSet<T>(RedBlackSet<T>.Tree s) where T : IComparable<T>
        {
            if (s == RedBlackSet<T>.EmptyTree) return "\u2205";
            return DumpTree(s);
        }

        private static string DumpTree<T>(RedBlackSet<T>.Tree s) where T : IComparable<T>
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
        public void DuplicateRootMemberTest()
        {
            var t = RedBlackSet<string>.EmptyTree;
            var x1 = RedBlackSet<string>.Insert("C", t);
            var x2 = RedBlackSet<string>.Insert("C", x1);
            Assert.AreEqual("(B: C)", DumpSet(x2));
            Assert.AreNotSame(x1, x2);
        }

        [TestMethod]
        public void DuplicateLeafMemberTest()
        {
            var empty = RedBlackSet<string>.EmptyTree;
            var a = RedBlackSet<string>.Insert("A", empty);
            var b = RedBlackSet<string>.Insert("B", a);
            var c1 = RedBlackSet<string>.Insert("C", b);
            Assert.AreEqual("(B: (B: A) B (B: C))", DumpSet(c1));
            var c2 = RedBlackSet<string>.Insert("C", c1);
            Assert.AreEqual("(B: (B: A) B (B: C))", DumpSet(c2));
            Assert.AreNotSame(c1, c2);
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
        public void BalanceTest1()
        {
            const string data = "z y x";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BalanceTest2()
        {
            const string data = "z x y";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BalanceTest3()
        {
            const string data = "x z y";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BalanceTest4()
        {
            const string data = "x y z";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (B: x) y (B: z))", DumpSet(t));
        }

        [TestMethod]
        public void BalanceTest5()
        {
            const string data = "y x z";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }

        [TestMethod]
        public void BalanceTest6()
        {
            const string data = "y z x";
            var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
            Assert.AreEqual("(B: (R: x) y (R: z))", DumpSet(t));
        }
    }
}