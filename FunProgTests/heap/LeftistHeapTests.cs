// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LeftistHeapTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.heap
{
    using System;
    using System.Linq;
    using System.Text;

    using FunProgLib.heap;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LeftistHeapTests
    {
        private static string DumpTree(LeftistHeap.Node tree)
        {
            if (tree == LeftistHeap.Empty) return "\u2205";

            var results = new StringBuilder();

            if (tree.Heap1 != LeftistHeap.Empty)
            {
                results.Append(DumpTree(tree.Heap1));
            }

            results.Append(tree.Min);
            //results.Append(" [");
            //results.Append(tree.rank);
            //results.Append("]");
            results.Append(", ");

            if (tree.Heap2 != LeftistHeap.Empty)
            {
                results.Append(DumpTree(tree.Heap2));
            }

            return results.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var tree = LeftistHeap.Empty;
            Assert.AreEqual("∅", DumpTree(tree));
        }

        [TestMethod]
        public void EmptyIsEmptyTest()
        {
            var tree = LeftistHeap.Empty;
            Assert.IsTrue(LeftistHeap.IsEmpty(tree));
        }

        [TestMethod]
        public void EmptyRankTest()
        {
            var tree = LeftistHeap.Empty;
            Assert.AreEqual(0, LeftistHeap.Rank(tree));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyMinTest()
        {
            var tree = LeftistHeap.Empty;
            var x = LeftistHeap.FindMin(tree);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyDeleteMinTest()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.DeleteMin(tree);
        }

        [TestMethod]
        public void SingleElement()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.Insert(tree, 2);
            Assert.AreEqual("2, ", DumpTree(tree));
        }

        [TestMethod]
        public void SingleIsEmptyTest()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.Insert(tree, 2);
            Assert.IsFalse(LeftistHeap.IsEmpty(tree));
        }

        [TestMethod]
        public void SingleRankTest()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.Insert(tree, 2);
            Assert.AreEqual(1, LeftistHeap.Rank(tree));
        }

        [TestMethod]
        public void SingleMinTest()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.Insert(tree, 2);
            var x = LeftistHeap.FindMin(tree);
            Assert.AreEqual(2, x);
        }

        [TestMethod]
        public void SingleDeleteMinTest()
        {
            var tree = LeftistHeap.Empty;
            tree = LeftistHeap.Insert(tree, 2);
            tree = LeftistHeap.DeleteMin(tree);
            Assert.IsTrue(LeftistHeap.IsEmpty(tree));
        }

        [TestMethod]
        public void DumpTreeTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            Assert.AreEqual("3, 2, 5, 1, ", DumpTree(tree));
        }

        [TestMethod]
        public void InsertFourTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            tree = LeftistHeap.Insert(tree, 4);
            Assert.AreEqual("3, 2, 5, 1, 4, ", DumpTree(tree));
            Assert.AreEqual(1, LeftistHeap.FindMin(tree));
        }

        [TestMethod]
        public void InsertZeroTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            tree = LeftistHeap.Insert(tree, 0);
            Assert.AreEqual("3, 2, 5, 1, 0, ", DumpTree(tree));
            Assert.AreEqual(0, LeftistHeap.FindMin(tree));
        }

        [TestMethod]
        public void MinTreeTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            Assert.AreEqual(1, LeftistHeap.FindMin(tree));
        }

        [TestMethod]
        public void DelMinTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            tree = LeftistHeap.DeleteMin(tree);
            Assert.AreEqual("3, 2, 5, ", DumpTree(tree));
            Assert.AreEqual(2, LeftistHeap.FindMin(tree));
        }

        [TestMethod]
        public void DelSecondMinTest()
        {
            var tree = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap.Empty, LeftistHeap.Insert);
            tree = LeftistHeap.DeleteMin(tree);
            tree = LeftistHeap.DeleteMin(tree);
            Assert.AreEqual("5, 3, ", DumpTree(tree));
            Assert.AreEqual(3, LeftistHeap.FindMin(tree));
        }
    }
}