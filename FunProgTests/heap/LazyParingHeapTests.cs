// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
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
    public class LazyParingHeapTests
    {
        private static string DumpHeap<T>(LazyParingHeap<T>.Heap node, bool showSusp) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(node.Root);
            if (!LazyParingHeap<T>.IsEmpty(node.List))
            {
                result.Append(", ");
                result.Append(DumpHeap(node.List, showSusp));
            }
            if (showSusp || node.LazyList.IsValueCreated)
            {
                if (!LazyParingHeap<T>.IsEmpty(node.LazyList.Value))
                {
                    result.Append("; ");
                    result.Append(DumpHeap(node.LazyList.Value, showSusp));
                }
            }
            else
            {
                result.Append("; susp");
            }
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var empty = LazyParingHeap<int>.Empty;
            Assert.IsTrue(LazyParingHeap<int>.IsEmpty(empty));

            var heap = LazyParingHeap<int>.Insert(3, empty);
            Assert.IsFalse(LazyParingHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void MergeTest1()
        {
            var heap1 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var empty = LazyParingHeap<int>.Empty;
            var heap = LazyParingHeap<int>.Merge(heap1, empty);
            Assert.AreSame(heap1, heap);
        }

        [TestMethod]
        public void MergeTest2()
        {
            var empty = LazyParingHeap<int>.Empty;
            var heap2 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Merge(empty, heap2);
            Assert.AreSame(heap2, heap);
        }

        [TestMethod]
        public void MergeTest3()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0; [1; [2, [3, [10; [11, [12]]]]]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void MergeTest4()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0, [10, [13]; [11, [12]]]; [1, [2]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void MergeTest5()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0; [1; [2, [3, [10, [13]; [11, [12]]]]]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void InsertTest1()
        {
            var empty = LazyParingHeap<int>.Empty;
            var heap = LazyParingHeap<int>.Insert(0, empty);
            Assert.AreEqual("[0]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void InsertTest2()
        {
            var heap1 = Enumerable.Range(0, 2).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Insert(2, heap1);
            Assert.AreEqual("[0; [1, [2]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void InsertTest3()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var heap = LazyParingHeap<int>.Insert(3, heap1);
            Assert.AreEqual("[0, [3]; [1, [2]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void FindMinEmptyTest()
        {
            var empty = LazyParingHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LazyParingHeap<int>.FindMin(empty));
        }

        [TestMethod]
        public void FindMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            var min = LazyParingHeap<int>.FindMin(heap);
            Assert.AreEqual(0, min);
        }

        [TestMethod]
        public void DeleteMinEmptyTest()
        {
            var empty = LazyParingHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LazyParingHeap<int>.DeleteMin(empty));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
            heap = LazyParingHeap<int>.DeleteMin(heap);
            Assert.AreEqual("[1; [2; [3; [4, [5; [6, [7]]]]]]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = LazyParingHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = LazyParingHeap<int>.Insert(random.Next(size), heap);
            var last = 0;
            var count = 0;
            while (!LazyParingHeap<int>.IsEmpty(heap))
            {
                var next = LazyParingHeap<int>.FindMin(heap);
                heap = LazyParingHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(size, count);
        }
    }
}