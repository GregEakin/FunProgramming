﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using Heap = FunProgLib.heap.BinomialHeap<int>;

namespace FunProgTests.heap
{
    [TestClass]
    public class BinomialHeapTests
    {
        private static string DumpTree<T>(BinomialHeap<T>.Tree tree) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(tree.Root);
            if (!FunProgLib.lists.List<BinomialHeap<T>.Tree>.IsEmpty(tree.List))
            {
                foreach (var node1 in tree.List)
                {
                    result.Append(", ");
                    result.Append(DumpTree(node1));
                }
            }
            result.Append("]");
            return result.ToString();
        }

        private static string DumpHeap<T>(IEnumerable<BinomialHeap<T>.Tree> list) where T : IComparable<T>
        {
            if (Equals(list, FunProgLib.lists.List<BinomialHeap<T>.Tree>.Empty))
                return string.Empty;

            var result = new StringBuilder();
            foreach (var node in list)
            {
                result.Append(DumpTree(node));
                result.Append("; ");
            }
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        private static int CountBinaryOnes(int n)
        {
            var count = 0;
            while (n != 0)
            {
                n = n & (n - 1);
                count++;
            }

            return count;
        }

        private static int CountChar(string s, char c) => s.Split(c).Length;

        [TestMethod]
        public void Test1()
        {
            var heap = BinomialHeap<int>.Empty;
            for (var i = 0; i < 16; i++)
            {
                heap = BinomialHeap<int>.Insert(i, heap);
                var dumpHeap = DumpHeap(heap);
                // Console.WriteLine(dumpHeap);

                var semicolons = CountChar(dumpHeap, ';');
                Assert.AreEqual(CountBinaryOnes(i + 1), semicolons);
            }
        }

        [TestMethod]
        public void Test2()
        {
            var heap = BinomialHeap<int>.Empty;
            for (var i = 0; i < 0x100; i++)
            {
                heap = BinomialHeap<int>.Insert(1, heap);
                var dumpHeap = DumpHeap(heap);
                // Console.WriteLine(dumpHeap);
                var blocks = dumpHeap.Split(';');

                var j = 0;
                var p = 0;
                for (var k = i + 1; k > 0; k >>= 1, j++)
                {
                    if (k % 2 == 0) continue;
                    var q = (int)Math.Pow(2, j);
                    var block = blocks[p++];
                    Assert.AreEqual(q, CountChar(block, '1') - 1);
                }
            }
        }

        [TestMethod]
        public void EmptyTest()
        {
            var heap = BinomialHeap<int>.Empty;
            Assert.IsTrue(BinomialHeap<int>.IsEmpty(heap));

            heap = BinomialHeap<int>.Insert(0, heap);
            Assert.IsFalse(BinomialHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void InsertTest1()
        {
            var heap1 = BinomialHeap<int>.Empty;
            var heap = BinomialHeap<int>.Insert(0, heap1);
            Assert.AreEqual("[0]", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertTest2()
        {
            var heap1 = Enumerable.Range(0, 2).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Insert(2, heap1);
            Assert.AreEqual("[2]; [0, [1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void InsertTest3()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Insert(3, heap1);
            Assert.AreEqual("[0, [2, [3]], [1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void MergeTest1()
        {
            var heap1 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap2 = BinomialHeap<int>.Empty;
            var heap = BinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreSame(heap1, heap);
        }

        [TestMethod]
        public void MergeTest2()
        {
            var heap1 = BinomialHeap<int>.Empty;
            var heap2 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreSame(heap2, heap);
        }

        [TestMethod]
        public void MergeTest3()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[12]; [10, [11]]; [0, [2, [3]], [1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void MergeTest4()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[2]; [0, [1]]; [10, [12, [13]], [11]]", DumpHeap(heap));
        }

        [TestMethod]
        public void MergeTest5()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var heap = BinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("[0, [10, [12, [13]], [11]], [2, [3]], [1]]", DumpHeap(heap));
        }

        [TestMethod]
        public void FindMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            var min = BinomialHeap<int>.FindMin(heap);
            Assert.AreEqual(0, min);
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
            heap = BinomialHeap<int>.DeleteMin(heap);
            Assert.AreEqual("[1]; [2, [3]]; [4, [6, [7]], [5]]", DumpHeap(heap));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = BinomialHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = BinomialHeap<int>.Insert(random.Next(size), heap);

            var last = 0;
            var count = 0;
            while (!BinomialHeap<int>.IsEmpty(heap))
            {
                var next = BinomialHeap<int>.FindMin(heap);
                heap = BinomialHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(size, count);
        }
    }
}
