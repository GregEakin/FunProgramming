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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FunProgLib.heap;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class ParingHeapTests
    {
        private static string DumpHeap<T>(ParingHeap<T>.Heap node) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(node.Root);
            if (!FunProgLib.lists.List<ParingHeap<T>.Heap>.IsEmpty(node.List) && node.List.Any())
            {
                result.Append(": ");
                foreach (var node1 in node.List)
                    result.Append(DumpHeap(node1));
            }
            result.Append("]");
            return result.ToString();
        }

        private static string DumpHeapList<T>(IEnumerable<ParingHeap<T>.Heap> list) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            if (Equals(list, FunProgLib.lists.List<ParingHeap<T>.Heap>.Empty))
            {
                foreach (var node in list)
                {
                    result.Append(DumpHeap(node));
                }
                result.Append(", ");
            }
            result.Remove(result.Length - 2, 2);
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var empty = ParingHeap<int>.Empty;
            Assert.IsTrue(ParingHeap<int>.IsEmpty(empty));

            var heap = ParingHeap<int>.Insert(3, empty);
            Assert.IsFalse(ParingHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void MergeTest1()
        {
            var heap1 = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var empty = ParingHeap<int>.Empty;
            var heap = ParingHeap<int>.Merge(heap1, empty);
            Assert.AreSame(heap1, heap);
        }

        [TestMethod]
        public void MergeTest2()
        {
            var empty = ParingHeap<int>.Empty;
            var heap2 = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Merge(empty, heap2);
            Assert.AreSame(heap2, heap);
        }

        [TestMethod]
        public void MergeTest3()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0: [10: [12][11]][3][2][1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void MergeTest4()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0: [10: [13][12][11]][2][1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void MergeTest5()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0: [10: [13][12][11]][3][2][1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertTest1()
        {
            var empty = ParingHeap<int>.Empty;
            var heap = ParingHeap<int>.Insert(0, empty);
            Assert.AreEqual("[0]", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertTest2()
        {
            var heap1 = Enumerable.Range(0, 2).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Insert(2, heap1);
            Assert.AreEqual("[0: [2][1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertTest3()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var heap = ParingHeap<int>.Insert(3, heap1);
            Assert.AreEqual("[0: [3][2][1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void FindMinEmptyTest()
        {
            var empty = ParingHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => ParingHeap<int>.FindMin(empty));
        }

        [TestMethod]
        public void FindMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            var min = ParingHeap<int>.FindMin(heap);
            Assert.AreEqual(0, min);
        }

        [TestMethod]
        public void DeleteMinEmptyTest()
        {
            var empty = ParingHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => ParingHeap<int>.DeleteMin(empty));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
            heap = ParingHeap<int>.DeleteMin(heap);
            Assert.AreEqual("[1: [6: [7]][4: [5]][2: [3]]]", DumpHeap(heap));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = ParingHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = ParingHeap<int>.Insert(random.Next(size), heap);
            var last = 0;
            var count = 0;
            while (!ParingHeap<int>.IsEmpty(heap))
            {
                var next = ParingHeap<int>.FindMin(heap);
                heap = ParingHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(size, count);
        }
    }
}