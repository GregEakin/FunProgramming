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
    public class SplayHeapTests
    {
        private static string DumpHeap<T>(SplayHeap<T>.Heap heap) where T : IComparable<T>
        {
            if (SplayHeap<T>.IsEmpty(heap)) return "\u2205";

            var result = new StringBuilder();
            result.Append("[");

            if (!SplayHeap<T>.IsEmpty(heap.A))
            {
                result.Append(DumpHeap(heap.A));
                result.Append(", ");
            }

            result.Append(heap.X);

            if (!SplayHeap<T>.IsEmpty(heap.B))
            {
                result.Append(", ");
                result.Append(DumpHeap(heap.B));
            }

            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = SplayHeap<string>.Empty;
            Assert.IsTrue(SplayHeap<string>.IsEmpty(t));
            Assert.AreEqual("∅", DumpHeap(t));

            var t1 = SplayHeap<string>.Insert("C", t);
            Assert.IsFalse(SplayHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = SplayHeap<string>.Empty;
            var x1 = SplayHeap<string>.Insert("C", t);
            var x2 = SplayHeap<string>.Insert("B", x1);
            Assert.AreEqual("[B, [C]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet";
            var ts = Words.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));
            Assert.AreEqual("[[[[[[a], a], any], as, [by, [[call, [in]], name, [name?]]]], other, [[rose], smell]], sweet, [[That, [[we], What's, [which]]], would]]", DumpHeap(ts));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));

            var t = SplayHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[[[[a], a, [any, [as]]], by, [[call], in, [name]]], name?, [other, [[[[rose], smell], sweet, [That, [we]]], What's, [[which], would]]]]", DumpHeap(t));
        }

        [TestMethod]
        public void FindMinTest1()
        {
            var t = SplayHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => SplayHeap<int>.FindMin(t));
        }

        [TestMethod]
        public void FindMinTest2()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(5, t0);
            var result = SplayHeap<int>.FindMin(t1);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void FindMinTest3()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(5, t0);
            var t2 = SplayHeap<int>.Insert(3, t1);
            var result = SplayHeap<int>.FindMin(t2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void FindMinTest4()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(3, t0);
            var t2 = SplayHeap<int>.Insert(5, t1);
            var result = SplayHeap<int>.FindMin(t2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void DeleteMinTest1()
        {
            var t = SplayHeap<int>.Empty;
            AssertThrows<ArgumentNullException>(() => SplayHeap<int>.DeleteMin(t));
        }

        [TestMethod]
        public void DeleteMinTest2()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(5, t0);
            var result = SplayHeap<int>.DeleteMin(t1);
            Assert.AreEqual("∅", DumpHeap(result));
        }

        [TestMethod]
        public void DeleteMinTest3()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(5, t0);
            var t2 = SplayHeap<int>.Insert(3, t1);
            var result = SplayHeap<int>.DeleteMin(t2);
            Assert.AreEqual("[5]", DumpHeap(result));
        }

        [TestMethod]
        public void DeleteMinTest4()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(3, t0);
            var t2 = SplayHeap<int>.Insert(5, t1);
            var result = SplayHeap<int>.DeleteMin(t2);
            Assert.AreEqual("[5]", DumpHeap(result));
        }

        [TestMethod]
        public void DeleteMinTest5()
        {
            var t0 = SplayHeap<int>.Empty;
            var t1 = SplayHeap<int>.Insert(3, t0);
            var t2 = SplayHeap<int>.Insert(5, t1);
            var t3 = SplayHeap<int>.Insert(6, t2);
            var result = SplayHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[5, [6]]", DumpHeap(result));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = SplayHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = SplayHeap<int>.Insert(random.Next(size), heap);
            var last = 0;
            var count = 0;
            while (!SplayHeap<int>.IsEmpty(heap))
            {
                var next = SplayHeap<int>.FindMin(heap);
                heap = SplayHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(size, count);
        }

        [TestMethod]
        public void Test3()
        {
            var heap = SplayHeap<int>.Empty;
            for (var i = 1; i < 8; i++)
                heap = SplayHeap<int>.Insert(i, heap);
            Assert.AreEqual("[[[[[[[1], 2], 3], 4], 5], 6], 7]", DumpHeap(heap));

            var x = SplayHeap<int>.Insert(0, heap);
            Assert.AreEqual("[0, [[[[1], 2, [3]], 4, [5]], 6, [7]]]", DumpHeap(x));

            var y = SplayHeap<int>.DeleteMin(x);
            Assert.AreEqual("[[[[1], 2, [3]], 4, [5]], 6, [7]]", DumpHeap(y));
        }
    }
}