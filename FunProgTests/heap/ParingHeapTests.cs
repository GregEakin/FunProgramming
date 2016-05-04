﻿// Fun Programming Data Structures 1.0
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
    using static FunProgTests.utilities.ExpectedException;

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
                result.Append(", ");
                foreach (var node1 in node.List)
                {
                    result.Append(DumpHeap(node1));
                }
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
            var t = ParingHeap<string>.Empty;
            Assert.IsTrue(ParingHeap<string>.IsEmpty(t));

            var t1 = ParingHeap<string>.Insert("C", t);
            Assert.IsFalse(ParingHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = ParingHeap<string>.Empty;
            var x1 = ParingHeap<string>.Insert("C", t);
            var x2 = ParingHeap<string>.Insert("B", x1);
            Assert.AreEqual("[B, [C]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet";
            var ts = Words.Split().Aggregate(ParingHeap<string>.Empty, (current, word) => ParingHeap<string>.Insert(word, current));
            Assert.AreEqual("[a, [sweet][as][smell][would][name][other][any][by][rose][a, [call][we][which][That][name?][in, [What's]]]]", DumpHeap(ts));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(ParingHeap<string>.Empty, (current, word) => ParingHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(ParingHeap<string>.Empty, (current, word) => ParingHeap<string>.Insert(word, current));

            var t = ParingHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[a, [a, [sweet][as][smell][would][name][other][any][by][rose][call, [That, [we][which]]]][name?][in, [What's]]]", DumpHeap(t));

        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = ParingHeap<int>.Empty;
            var t1 = ParingHeap<int>.Insert(5, t);
            var t2 = ParingHeap<int>.Insert(3, t1);
            var t3 = ParingHeap<int>.Insert(6, t2);
            var t4 = ParingHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[5, [6]]", DumpHeap(t4));
            Assert.AreEqual(5, ParingHeap<int>.FindMin(t4));

            Assert.AreEqual(3, ParingHeap<int>.FindMin(t3));
        }

        [TestMethod]
        public void DeleteLotsOfMinsTest()
        {
            var random = new Random(3456);
            var heap = ParingHeap<int>.Empty;
            for (var i = 0; i < 100; i++) heap = ParingHeap<int>.Insert(random.Next(100), heap);
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
            Assert.AreEqual(100, count);
        }

        [TestMethod]
        public void FindMinNullTest()
        {
            AssertThrows<ArgumentException>(() => ParingHeap<int>.FindMin(ParingHeap<int>.Empty));
        }

        [TestMethod]
        public void DeleteMinNullTest()
        {
            AssertThrows<ArgumentException>(() => ParingHeap<int>.DeleteMin(ParingHeap<int>.Empty));
        }
    }
}