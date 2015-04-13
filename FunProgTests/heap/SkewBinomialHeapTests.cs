// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Linq;
using System.Text;
using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.heap
{
    [TestClass]
    public class SkewBinomialHeapTests
    {
        private static string DumpList<T>(FunProgLib.lists.List<T>.Node tree) where T : IComparable<T>
        {
            if (FunProgLib.lists.List<T>.IsEmpty(tree))
                return string.Empty;

            var result = new StringBuilder();
            result.Append(", ");
            foreach (var node1 in tree)
            {
                result.Append(node1);
                result.Append(", ");
            }
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        private static string DumpTree<T>(SkewBinomialHeap<T>.Tree tree) where T : IComparable<T>
        {
            if (tree == null) return string.Empty;
            var result = new StringBuilder();
            result.Append("[");
            //result.Append(tree.Rank);
            //result.Append(", ");
            result.Append(tree.Node);
            result.Append(DumpList(tree.List));
            if (!FunProgLib.lists.List<SkewBinomialHeap<T>.Tree>.IsEmpty(tree.TreeList))
                result.Append(DumpHeap(tree.TreeList));
            result.Append("]");
            return result.ToString();
        }

        private static string DumpHeap<T>(FunProgLib.lists.List<SkewBinomialHeap<T>.Tree>.Node heap) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            while (!FunProgLib.lists.List<SkewBinomialHeap<T>.Tree>.IsEmpty(heap))
            {
                var head = FunProgLib.lists.List<SkewBinomialHeap<T>.Tree>.Head(heap);
                result.Append(DumpTree(head));
                heap = FunProgLib.lists.List<SkewBinomialHeap<T>.Tree>.Tail(heap);
            }
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = SkewBinomialHeap<string>.Empty;
            Assert.IsTrue(SkewBinomialHeap<string>.IsEmpty(t));

            var t1 = SkewBinomialHeap<string>.Insert("C", t);
            Assert.IsFalse(SkewBinomialHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void TestEmpty()
        {
            var t = SkewBinomialHeap<string>.Empty;
            Assert.AreEqual("[]", DumpHeap(t));
        }

        [TestMethod]
        public void Test0()
        {
            var t = SkewBinomialHeap<string>.Empty;
            var x1 = SkewBinomialHeap<string>.Insert("C", t);
            Assert.AreEqual("[[C]]", DumpHeap(x1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = SkewBinomialHeap<string>.Empty;
            var x1 = SkewBinomialHeap<string>.Insert("C", t);
            var x2 = SkewBinomialHeap<string>.Insert("B", x1);
            Assert.AreEqual("[[B][C]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var t = Words.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));
            Assert.AreEqual("[[sweet.][as][name, smell[[would]]][a, other, call[[any, by[[rose]]][we]]][name?, which[[That]]][a, in[[What's]]]]", DumpHeap(t));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));

            var t = SkewBinomialHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[[name?[[sweet]]][a, in[[any, name[[other]]][as, smell[[would]]][What's]]][a, by, rose[[That, we[[which]]][call]]]]", DumpHeap(t));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = SkewBinomialHeap<int>.Empty;
            var t1 = SkewBinomialHeap<int>.Insert(5, t);
            var t2 = SkewBinomialHeap<int>.Insert(3, t1);
            var t3 = SkewBinomialHeap<int>.Insert(6, t2);

            var t4 = SkewBinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[[6][5]]", DumpHeap(t4));
            Assert.AreEqual(5, SkewBinomialHeap<int>.FindMin(t4));

            Assert.AreEqual(3, SkewBinomialHeap<int>.FindMin(t3));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            var random = new Random(3456);
            var heap = SkewBinomialHeap<int>.Empty;
            for (var i = 0; i < 100; i++) heap = SkewBinomialHeap<int>.Insert(random.Next(100), heap);
            var last = 0;
            var count = 0;
            while (!SkewBinomialHeap<int>.IsEmpty(heap))
            {
                var next = SkewBinomialHeap<int>.FindMin(heap);
                heap = SkewBinomialHeap<int>.DeleteMin(heap);
                Assert.IsTrue(last <= next);
                last = next;
                count++;
            }
            Assert.AreEqual(100, count);
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest2()
        {
            var random = new Random(1000);
            var t = SkewBinomialHeap<int>.Empty;

            var min = 0;
            for (var i = 0; i < 1000; i++)
            {
                var j = random.Next(1000);
                min = Math.Min(j, min);
                t = SkewBinomialHeap<int>.Insert(j, t);

                j = random.Next(1000);
                min = Math.Min(j, min);
                t = SkewBinomialHeap<int>.Insert(j, t);

                var k = SkewBinomialHeap<int>.FindMin(t);
                t = SkewBinomialHeap<int>.DeleteMin(t);
                Assert.IsTrue(min <= k);
                min = k;
            }

            for (var i = 0; i < 1000; i++)
            {
                var j = SkewBinomialHeap<int>.FindMin(t);
                t = SkewBinomialHeap<int>.DeleteMin(t);
                Assert.IsTrue(min <= j);
                min = j;
            }

            Assert.IsTrue(SkewBinomialHeap<int>.IsEmpty(t));
        }
    }
}
