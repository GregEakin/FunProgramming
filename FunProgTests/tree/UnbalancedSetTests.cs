// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		UnbalancedSetTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.tree
{
    using System.Linq;

    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnbalancedSetTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var tree = UnbalancedSet<string>.E;
            Assert.AreEqual("∅", tree.ToString());
        }

        [TestMethod]
        public void Test1()
        {
            var tree = UnbalancedSet<string>.E;
            tree = tree.Insert("a");
            Assert.AreEqual("a, ", tree.ToString());
        }

        [TestMethod]
        public void DumpTreeTest()
        {
            var tree = new[] { "how", "now", "brown", "cow" }.Aggregate(UnbalancedSet<string>.E, (current, word) => current.Insert(word));
            Assert.AreEqual("brown, cow, how, now, ", tree.ToString());
        }

        [TestMethod]
        public void ElementTest()
        {
            var tree = new[] { "how", "now", "brown", "cow" }.Aggregate(UnbalancedSet<string>.E, (current, word) => current.Insert(word));
            Assert.IsTrue(tree.Member("how"));
            Assert.IsFalse(tree.Member("wow"));
        }

    }
}