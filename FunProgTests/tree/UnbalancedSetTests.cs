﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
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
    public class UnbalancedSetTests
    {
        private static string DumpTree<T>(UnbalancedSet<T>.Tree tree) where T : IComparable<T>
        {
            if (tree == UnbalancedSet<T>.Empty) return "\u2205";

            var results = new StringBuilder();

            results.Append('[');
            if (tree.A != UnbalancedSet<T>.Empty)
            {
                results.Append(DumpTree(tree.A));
                results.Append(",");
            }

            results.Append(tree.Y);

            if (tree.B != UnbalancedSet<T>.Empty)
            {
                results.Append(",");
                results.Append(DumpTree(tree.B));
            }
            results.Append(']');

            return results.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var tree = UnbalancedSet<string>.Empty;
            Assert.AreEqual("∅", DumpTree(tree));
        }

        [TestMethod]
        public void SingleElementTest()
        {
            var tree = UnbalancedSet<string>.Empty;
            tree = UnbalancedSet<string>.Insert("a", tree);
            Assert.AreEqual("[a]", DumpTree(tree));
        }

        [TestMethod]
        public void DuplicateElementTest()
        {
            var tree = UnbalancedSet<string>.Empty;
            tree = UnbalancedSet<string>.Insert("a", tree);
            tree = UnbalancedSet<string>.Insert("a", tree);
            Assert.AreEqual("[a]", DumpTree(tree));
        }

        [TestMethod]
        public void DumpTreeTest()
        {
            const string data = "How now, brown cow?";
            var tree = data.Split().Aggregate(UnbalancedSet<string>.Empty, (current, word) => UnbalancedSet<string>.Insert(word, current));
            Assert.AreEqual("[[brown,[cow?]],How,[now,]]", DumpTree(tree));
        }

        [TestMethod]
        public void ElementTest()
        {
            const string data = "How now, brown cow?";
            var tree = data.Split().Aggregate(UnbalancedSet<string>.Empty, (current, word) => UnbalancedSet<string>.Insert(word, current));
            foreach (var word in data.Split())
                Assert.IsTrue(UnbalancedSet<string>.Member(word, tree));
            Assert.IsFalse(UnbalancedSet<string>.Member("wow", tree));
        }
    }
}