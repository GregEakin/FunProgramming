// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LeftistHeapTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.tree
{
    using System;
    using System.Linq;

    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LeftistHeapTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var tree = LeftistHeap.Empty;
            Assert.AreEqual("∅", tree.ToString());
            Assert.IsTrue(tree.IsEmapty);
            Assert.AreEqual(0, tree.Rank());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyMinTest()
        {
            var tree = LeftistHeap.Empty;
            var x = tree.FindMin();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteMinTest()
        {
            var tree = LeftistHeap.Empty;
            tree = tree.DeleteMin();
        }

        [TestMethod]
        public void SingleElement()
        {
            var tree = LeftistHeap.Empty;
            tree = tree.Insert(2);
            Assert.AreEqual("2, ", tree.ToString());
        }

        [TestMethod]
        public void DumpTreeTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            Assert.AreEqual("3, 2, 5, 1, ", tree.ToString());
        }

        [TestMethod]
        public void InsertTwoTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            tree = tree.Insert(4);
            Assert.AreEqual("3, 2, 5, 1, 4, ", tree.ToString());
        }

        [TestMethod]
        public void InsertZeroTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            tree = tree.Insert(0);
            Assert.AreEqual("3, 2, 5, 1, 0, ", tree.ToString());
            Assert.AreEqual(0, tree.FindMin());
        }

        [TestMethod]
        public void MinTreeTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            Assert.AreEqual(1, tree.FindMin());
        }

        [TestMethod]
        public void DelMinTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            tree = tree.DeleteMin();
            Assert.AreEqual("3, 2, 5, ", tree.ToString());
            Assert.AreEqual(2, tree.FindMin());
        }

        [TestMethod]
        public void DelSecondMinTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, (current, word) => current.Insert(word));
            tree = tree.DeleteMin();
            tree = tree.DeleteMin();
            Assert.AreEqual("5, 3, ", tree.ToString());
            Assert.AreEqual(3, tree.FindMin());
        }
    }
}