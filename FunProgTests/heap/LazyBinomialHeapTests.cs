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
    using FunProgLib.heap;
    using FunProgLib.lists;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class LazyBinomialHeapTests
    {
        public static string DumpNode<T>(LazyBinomialHeap<T>.Tree tree) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("{");
            result.Append(tree.Root);
            if (!List<LazyBinomialHeap<T>.Tree>.IsEmpty(tree.List))
            {
                result.Append(": ");
                foreach (var node1 in tree.List)
                    result.Append(DumpNode(node1));
            }
            result.Append("}");
            return result.ToString();
        }

        public static string DumpHeap<T>(Lazy<List<LazyBinomialHeap<T>.Tree>.Node> list) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            if (!List<LazyBinomialHeap<T>.Tree>.IsEmpty(list.Value))
            {
                foreach (var node in list.Value)
                    result.Append(DumpNode(node));
            }
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = LazyBinomialHeap<string>.Empty;
            Assert.IsTrue(LazyBinomialHeap<string>.IsEmpty(t));

            var t1 = LazyBinomialHeap<string>.Insert("C", t);
            Assert.IsFalse(LazyBinomialHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = LazyBinomialHeap<string>.Empty;
            var x1 = LazyBinomialHeap<string>.Insert("C", t);
            var x2 = LazyBinomialHeap<string>.Insert("B", x1);
            Assert.AreEqual("[{B: {C}}]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var t = words.Split()
                .Aggregate(LazyBinomialHeap<string>.Empty,
                    (current, word) => LazyBinomialHeap<string>.Insert(word, current));
            Assert.AreEqual(
                "[{as: {sweet.}}{a: {a: {call: {That: {which}}{we}}{in: {What's}}{name?}}{name: {smell: {would}}{other}}{any: {by}}{rose}}]",
                DumpHeap(t));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string data1 = "What's in a name?";
            var ts1 = data1.Split()
                .Aggregate(LazyBinomialHeap<string>.Empty,
                    (current, word) => LazyBinomialHeap<string>.Insert(word, current));

            const string data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = data2.Split()
                .Aggregate(LazyBinomialHeap<string>.Empty,
                    (current, word) => LazyBinomialHeap<string>.Insert(word, current));

            var t = LazyBinomialHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual(
                "[{as: {sweet}}{a: {a: {call: {That: {which}}{we}}{any: {by}}{rose}}{name: {smell: {would}}{other}}{in: {What's}}{name?}}]",
                DumpHeap(t));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = LazyBinomialHeap<int>.Empty;
            var t1 = LazyBinomialHeap<int>.Insert(5, t);
            var t2 = LazyBinomialHeap<int>.Insert(3, t1);
            var t3 = LazyBinomialHeap<int>.Insert(6, t2);
            Assert.AreEqual("[{6}{3: {5}}]", DumpHeap(t3));

            var t4 = LazyBinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[{5: {6}}]", DumpHeap(t4));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            const int size = 1000;
            var random = new Random(3456);
            var heap = LazyBinomialHeap<int>.Empty;
            for (var i = 0; i < size; i++) heap = LazyBinomialHeap<int>.Insert(random.Next(size), heap);
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