// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgTests.utilities;

namespace FunProgTests.heap
{
    using FunProgLib.heap;
    using FunProgLib.lists;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;
    using static ExpectedException;

    [TestClass]
    public class LazyBinomialHeapTests
    {
        public static string DumpNode<T>(LazyBinomialHeap<T>.Tree tree) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(tree.Root);
            if (!List<LazyBinomialHeap<T>.Tree>.IsEmpty(tree.List))
            {
                foreach (var node in tree.List)
                {
                    result.Append(", ");
                    result.Append(DumpNode(node));
                }
            }
            result.Append("]");
            return result.ToString();
        }

        public static string DumpHeap<T>(Lazy<List<LazyBinomialHeap<T>.Tree>.Node> list, bool expandUnCreated) where T : IComparable<T>
        {
            if (!list.IsValueCreated && !expandUnCreated)
                return "$";

            var result = new StringBuilder();
            if (!list.IsValueCreated)
                result.Append("$");
            foreach (var node in list.Value)
            {
                result.Append(DumpNode(node));
                result.Append("; ");
            }
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        [TestMethod]
        public void BinomialTest1()
        {
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < 16; i++)
            {
                heap = LazyBinomialHeap<int>.Insert(i, heap);
                var dumpHeap = DumpHeap(heap, true);
                // Console.WriteLine(dumpHeap, true);

                var semicolons = Counters.CountChar(dumpHeap, ';');
                Assert.AreEqual(Counters.CountBinaryOnes(i + 1), semicolons);
            }
        }

        [TestMethod]
        public void BinomialTest2()
        {
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < 0x100; i++)
            {
                heap = LazyBinomialHeap<int>.Insert(1, heap);
                var dumpHeap = DumpHeap(heap, true);
                //Console.WriteLine(dumpHeap, true);
                var blocks = dumpHeap.Split(';');

                var j = 0;
                var p = 0;
                for (var k = i + 1; k > 0; k >>= 1, j++)
                {
                    if (k % 2 == 0) continue;
                    var q = (int)Math.Pow(2, j);
                    var block = blocks[p++];
                    Assert.AreEqual(q, Counters.CountChar(block, '1') - 1);
                }
            }
        }

        [TestMethod]
        public void MonolithicTest()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            var x1 = LazyBinomialHeap<int>.Insert(3, empty);
            var x2 = LazyBinomialHeap<int>.Insert(2, x1);
            Assert.IsFalse(x1.IsValueCreated);
            Assert.IsFalse(x2.IsValueCreated);

            // Once we look at one element, the entire list will be forced created.
            Assert.IsNotNull(x2.Value);
            Assert.IsTrue(x1.IsValueCreated);
            Assert.IsTrue(x2.IsValueCreated);
        }

        [TestMethod]
        public void EmptyTest()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            Assert.IsTrue(LazyBinomialHeap<int>.IsEmpty(empty));

            var heap = LazyBinomialHeap<int>.Insert(1, empty);
            Assert.IsFalse(LazyBinomialHeap<int>.IsEmpty(heap));
        }

        [TestMethod]
        public void InsertTest1()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            var heap = LazyBinomialHeap<int>.Insert(0, empty);
            Assert.AreEqual("$[0]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void InsertTest2()
        {
            var heap1 = Enumerable.Range(0, 2).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Insert(2, heap1);
            Assert.AreEqual("$[2]; [0, [1]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void InsertTest3()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Insert(3, heap1);
            Assert.AreEqual("$[0, [2, [3]], [1]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void MergeTest1()
        {
            var heap1 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var empty = LazyBinomialHeap<int>.Empty;
            var heap = LazyBinomialHeap<int>.Merge(heap1, empty);
            Assert.AreSame(heap1.Value, heap.Value);
        }

        [TestMethod]
        public void MergeTest2()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            var heap2 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Merge(empty, heap2);
            Assert.AreSame(heap2.Value, heap.Value);
        }

        [TestMethod]
        public void MergeTest3()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("$[12]; [10, [11]]; [0, [2, [3]], [1]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void MergeTest4()
        {
            var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("$[2]; [0, [1]]; [10, [12, [13]], [11]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void MergeTest5()
        {
            var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
            Assert.AreEqual("$[0, [10, [12, [13]], [11]], [2, [3]], [1]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void FindMinEmptyTest()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LazyBinomialHeap<int>.FindMin(empty));
        }

        [TestMethod]
        public void FindMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            var min = LazyBinomialHeap<int>.FindMin(heap);
            Assert.AreEqual(0, min);
        }

        [TestMethod]
        public void DeleteMinEmptyTest()
        {
            var empty = LazyBinomialHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => LazyBinomialHeap<int>.DeleteMin(empty));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
            heap = LazyBinomialHeap<int>.DeleteMin(heap);
            Assert.AreEqual("$[1]; [2, [3]]; [4, [6, [7]], [5]]", DumpHeap(heap, true));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = LazyBinomialHeap<int>.Insert(random.Next(size), heap);
            Assert.IsFalse(heap.IsValueCreated);

            var last = 0;
            var count = 0;
            while (!LazyBinomialHeap<int>.IsEmpty(heap))
            {
                var next = LazyBinomialHeap<int>.FindMin(heap);
                heap = LazyBinomialHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }

            Assert.AreEqual(size, count);
        }
    }
}
