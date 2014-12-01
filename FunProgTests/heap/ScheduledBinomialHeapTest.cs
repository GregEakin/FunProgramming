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
    using FunProgLib.lists;
    using FunProgLib.streams;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScheduledBinomialHeapTests
    {
        private static string DumpTree<T>(ScheduledBinomialHeap<T>.Tree tree) where T : IComparable<T>
        {
            if (tree == null) return string.Empty;
            var result = new StringBuilder();
            result.Append("[");
            result.Append(tree.Node);
            if (tree.TreeList != List<ScheduledBinomialHeap<T>.Tree>.Empty)
            {
                result.Append(", ");
                foreach (var node1 in tree.TreeList)
                {
                    result.Append(DumpTree(node1));
                    result.Append(", ");
                }
                result.Remove(result.Length - 2, 2);
            }
            result.Append("]");
            return result.ToString();
        }

        private static string DumpDigitStream<T>(Lazy<Stream<ScheduledBinomialHeap<T>.Digit>.StreamCell> stream) where T : IComparable<T>
        {
            if (stream == ScheduledBinomialHeap<T>.EmptyStream) return string.Empty;
            if (!stream.IsValueCreated) return " -$- ";
            if (stream == Stream<ScheduledBinomialHeap<T>.Digit>.DollarNil) return string.Empty;
            var result = new StringBuilder();
            // result.Append(DumpTree(stream.Value.Element.One));
            // result.Append(DumpDigitStream(stream.Value.Next));
            return result.ToString();
        }

        private static string DumpHeap<T>(ScheduledBinomialHeap<T>.Heap heap) where T : IComparable<T>
        {
            var result = new StringBuilder();
            result.Append("[");
            if (heap.DigitStream != null)
            {
                result.Append(DumpDigitStream(heap.DigitStream));
                result.Append(", ");
            }
            result.Remove(result.Length - 2, 2);
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = ScheduledBinomialHeap<string>.Empty;
            Assert.IsTrue(ScheduledBinomialHeap<string>.IsEmpty(t));

            var t1 = ScheduledBinomialHeap<string>.Insert("C", t);
            Assert.IsFalse(ScheduledBinomialHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void TestEmpty()
        {
            var t = ScheduledBinomialHeap<string>.Empty;
            Assert.AreEqual("[]", DumpHeap(t));
        }

        [TestMethod]
        public void Test0()
        {
            var t = ScheduledBinomialHeap<string>.Empty;
            var x1 = ScheduledBinomialHeap<string>.Insert("C", t);
            Assert.AreEqual("[[C]]", DumpHeap(x1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = ScheduledBinomialHeap<string>.Empty;
            var x1 = ScheduledBinomialHeap<string>.Insert("C", t);
            var x2 = ScheduledBinomialHeap<string>.Insert("B", x1);
            Assert.AreEqual("[[B, [C]]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var t = Words.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));
            Assert.AreEqual("[[as, [sweet.]] -$- ]", DumpHeap(t));

            var x = ScheduledBinomialHeap<string>.Merge(t, ScheduledBinomialHeap<string>.Empty);
            Assert.AreEqual("[[as, [sweet.]][a, [a, [call, [That, [which]], [we]], [in, [What's]], [name?]], [name, [smell, [would]], [other]], [any, [by]], [rose]]]", DumpHeap(x));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));

            var t = ScheduledBinomialHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[[as, [sweet]][a, [a, [call, [That, [which]], [we]], [any, [by]], [rose]], [name, [smell, [would]], [other]], [in, [What's]], [name?]]]", DumpHeap(t));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = ScheduledBinomialHeap<int>.Empty;
            var t1 = ScheduledBinomialHeap<int>.Insert(5, t);
            var t2 = ScheduledBinomialHeap<int>.Insert(3, t1);
            var t3 = ScheduledBinomialHeap<int>.Insert(6, t2);

            var t4 = ScheduledBinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[[5, [6]]]", DumpHeap(t4));
            Assert.AreEqual(5, ScheduledBinomialHeap<int>.FindMin(t4));

            Assert.AreEqual(3, ScheduledBinomialHeap<int>.FindMin(t3));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            var random = new Random(1000);
            var t = ScheduledBinomialHeap<int>.Empty;

            for (var i = 0; i < 1000; i++)
            {
                var j = random.Next(1000);
                t = ScheduledBinomialHeap<int>.Insert(j, t);
            }

            var min = 0;
            for (var i = 0; i < 1000; i++)
            {
                var j = ScheduledBinomialHeap<int>.FindMin(t);
                t = ScheduledBinomialHeap<int>.DeleteMin(t);
                Assert.IsTrue(min <= j);
                min = j;
            }

            Assert.IsTrue(ScheduledBinomialHeap<int>.IsEmpty(t));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest2()
        {
            var random = new Random(1000);
            var t = ScheduledBinomialHeap<int>.Empty;

            var min = 0;
            for (var i = 0; i < 1000; i++)
            {
                var j = random.Next(1000);
                min = Math.Min(j, min);
                t = ScheduledBinomialHeap<int>.Insert(j, t);

                j = random.Next(1000);
                min = Math.Min(j, min);
                t = ScheduledBinomialHeap<int>.Insert(j, t);

                var k = ScheduledBinomialHeap<int>.FindMin(t);
                t = ScheduledBinomialHeap<int>.DeleteMin(t);
                Assert.IsTrue(min <= k);
                min = k;
            }

            for (var i = 0; i < 1000; i++)
            {
                var j = ScheduledBinomialHeap<int>.FindMin(t);
                t = ScheduledBinomialHeap<int>.DeleteMin(t);
                Assert.IsTrue(min <= j);
                min = j;
            }

            Assert.IsTrue(ScheduledBinomialHeap<int>.IsEmpty(t));
        }
    }
}