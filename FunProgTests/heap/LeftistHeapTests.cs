﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.heap
{
    using System;
    using System.Linq;
    using System.Text;

    using FunProgLib.heap;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class LeftistHeapTests
    {
        private static string DumpHeap<T>(LeftistHeap<T>.Heap heap) where T : IComparable<T>
        {
            if (LeftistHeap<T>.IsEmpty(heap)) return "\u2205";

            var results = new StringBuilder();

            if (!LeftistHeap<T>.IsEmpty(heap.A))
            {
                results.Append(DumpHeap(heap.A));
            }

            results.Append(heap.X);
            //results.Append(" [");
            //results.Append(heap.r);
            //results.Append("]");
            results.Append(", ");

            if (!LeftistHeap<T>.IsEmpty(heap.B))
            {
                results.Append(DumpHeap(heap.B));
            }

            return results.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var heap = LeftistHeap<int>.Empty;
            Assert.AreEqual("∅", DumpHeap(heap));
        }

        [TestMethod]
        public void EmptyIsEmptyTest()
        {
            var heap = LeftistHeap<int>.Empty;
            Assert.IsTrue(LeftistHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void EmptyMinTest()
        {
            var heap = LeftistHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void EmptyDeleteMinTest()
        {
            var heap = LeftistHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LeftistHeap<int>.DeleteMin(heap));
        }

        [TestMethod]
        public void SingleElement()
        {
            var heap = LeftistHeap<int>.Empty;
            heap = LeftistHeap<int>.Insert(2, heap);
            Assert.AreEqual("2, ", DumpHeap(heap));
        }

        [TestMethod]
        public void SingleIsEmptyTest()
        {
            var heap = LeftistHeap<int>.Empty;
            heap = LeftistHeap<int>.Insert(2, heap);
            Assert.IsFalse(LeftistHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void SingleMinTest()
        {
            var heap = LeftistHeap<int>.Empty;
            heap = LeftistHeap<int>.Insert(2, heap);
            var x = LeftistHeap<int>.FindMin(heap);
            Assert.AreEqual(2, x);
        }

        [TestMethod]
        public void SingleDeleteMinTest()
        {
            var heap = LeftistHeap<int>.Empty;
            heap = LeftistHeap<int>.Insert(2, heap);
            heap = LeftistHeap<int>.DeleteMin(heap);
            Assert.IsTrue(LeftistHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void DumpTreeTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            Assert.AreEqual("3, 2, 5, 1, ", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertFourTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            heap = LeftistHeap<int>.Insert(4, heap);
            Assert.AreEqual("3, 2, 5, 1, 4, ", DumpHeap(heap));
            Assert.AreEqual(1, LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void InsertZeroTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            heap = LeftistHeap<int>.Insert(0, heap);
            Assert.AreEqual("3, 2, 5, 1, 0, ", DumpHeap(heap));
            Assert.AreEqual(0, LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void MinTreeTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            Assert.AreEqual(1, LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void DelMinTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            heap = LeftistHeap<int>.DeleteMin(heap);
            Assert.AreEqual("3, 2, 5, ", DumpHeap(heap));
            Assert.AreEqual(2, LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void DelSecondMinTest()
        {
            var heap = new[] { 3, 2, 5, 1 }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
            heap = LeftistHeap<int>.DeleteMin(heap);
            heap = LeftistHeap<int>.DeleteMin(heap);
            Assert.AreEqual("5, 3, ", DumpHeap(heap));
            Assert.AreEqual(3, LeftistHeap<int>.FindMin(heap));
        }

        [TestMethod]
        public void MergeTest()
        {
            var heap1 = new[] { "How", "now," }.Aggregate(LeftistHeap<string>.Empty, (h, x) => LeftistHeap<string>.Insert(x, h));
            var heap2 = new[] { "brown", "cow?" }.Aggregate(LeftistHeap<string>.Empty, (h, x) => LeftistHeap<string>.Insert(x, h));
            var heap = LeftistHeap<string>.Merge(heap1, heap2);
            Assert.AreEqual("cow?, brown, now,, How, ", DumpHeap(heap));
            Assert.AreEqual("brown", LeftistHeap<string>.FindMin(heap));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            var random = new Random(3456);
            var heap = LeftistHeap<int>.Empty;
            for (var i = 0; i < 100; i++) heap = LeftistHeap<int>.Insert(random.Next(100), heap);
            var last = 0;
            var count = 0;
            while (!LeftistHeap<int>.IsEmpty(heap))
            {
                var next = LeftistHeap<int>.FindMin(heap);
                heap = LeftistHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(100, count);
        }
    }
}