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
            if (showSusp || node.List2.IsValueCreated)
            {
                if (!LazyParingHeap<T>.IsEmpty(node.List2.Value))
                {
                    result.Append("; ");
                    result.Append(DumpHeap(node.List2.Value, showSusp));
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
            var t = LazyParingHeap<string>.Empty;
            Assert.IsTrue(LazyParingHeap<string>.IsEmpty(t));

            var t1 = LazyParingHeap<string>.Insert("C", t);
            Assert.IsFalse(LazyParingHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = LazyParingHeap<string>.Empty;
            var x1 = LazyParingHeap<string>.Insert("X", t);
            var x2 = LazyParingHeap<string>.Insert("Y", x1);
            Assert.AreEqual("[X, [Y; susp]; susp]", DumpHeap(x2, false));
            Assert.AreEqual("[X, [Y]]", DumpHeap(x2, true));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet";
            var ts = Words.Split().Aggregate(LazyParingHeap<string>.Empty, (current, word) => LazyParingHeap<string>.Insert(word, current));
            Assert.AreEqual("[a; susp]", DumpHeap(ts, false));
            Assert.AreEqual("[a; [a, [as, [sweet]]; [any; [by; [call; [in, [name, [rose]; [other, [smell, [would]]]]; [name?; [That, [What's]; [we, [which]]]]]]]]]]", DumpHeap(ts, true));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(LazyParingHeap<string>.Empty, (current, word) => LazyParingHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(LazyParingHeap<string>.Empty, (current, word) => LazyParingHeap<string>.Insert(word, current));

            var t = LazyParingHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[a, [a; susp]; susp]", DumpHeap(t, false));
            Assert.AreEqual("[a, [a; [any, [as, [sweet]]; [by; [call, [name; [other, [smell, [would]]]]; [rose, [That; [we, [which]]]]]]]]; [in; [name?, [What's]]]]", DumpHeap(t, true));

        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = LazyParingHeap<int>.Empty;
            var t1 = LazyParingHeap<int>.Insert(5, t);
            var t2 = LazyParingHeap<int>.Insert(3, t1);
            var t3 = LazyParingHeap<int>.Insert(6, t2);
            var t4 = LazyParingHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[5, [6]]", DumpHeap(t4, false));
            Assert.AreEqual("[5, [6]]", DumpHeap(t4, true));
            Assert.AreEqual(5, LazyParingHeap<int>.FindMin(t4));

            Assert.AreEqual(3, LazyParingHeap<int>.FindMin(t3));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FindMinNullTest()
        {
            LazyParingHeap<int>.FindMin(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteMinNullTest()
        {
            LazyParingHeap<int>.DeleteMin(null);
        }
    }
}