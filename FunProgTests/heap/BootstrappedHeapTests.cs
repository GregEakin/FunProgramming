﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2016 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Collections.Generic;
using System.Text;

namespace FunProgTests.heap
{
    using System;
    using System.Linq;
    using FunProgLib.heap;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

    [TestClass]
    public class BootstrappedHeapTests
    {
        private static string DumpElement<T>(BootstrappedHeap<T>.PrimH.Element element) where T : IComparable<T>
        {
            if (BootstrappedHeap<T>.PrimH.IsEmpty(element)) return "Empty";
            var result = new StringBuilder();
            result.Append("{");
            result.Append(DumpHeap(element.H1));
            result.Append(": ");
            result.Append(DumpElement(element.H2));
            result.Append("}");
            return result.ToString();
        }

        private static string DumpHeap<T>(BootstrappedHeap<T>.Heap heap) where T : IComparable<T>
        {
            if (BootstrappedHeap<T>.IsEmpty(heap)) return "Empty";

            var result = new StringBuilder();
            result.Append("[");
            result.Append(heap.X);
            result.Append(": ");
            result.Append(DumpElement(heap.P));
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var t = BootstrappedHeap<string>.Empty;
            Assert.IsTrue(BootstrappedHeap<string>.IsEmpty(t));

            var t1 = BootstrappedHeap<string>.Insert("C", t);
            Assert.IsFalse(BootstrappedHeap<string>.IsEmpty(t1));
        }

        [TestMethod]
        public void Merge1Test()
        {
            var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            var heap3 = BootstrappedHeap<string>.Merge(BootstrappedHeap<string>.Empty, heap2);
            Assert.AreSame(heap2, heap3);
        }

        [TestMethod]
        public void Merge2Test()
        {
            var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            var heap3 = BootstrappedHeap<string>.Merge(heap1, BootstrappedHeap<string>.Empty);
            Assert.AreSame(heap1, heap3);
        }

        [TestMethod]
        public void Merge3Test()
        {
            var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            var heap3 = BootstrappedHeap<string>.Merge(heap1, heap2);
            Assert.AreEqual("[a: {[b: Empty]: {[c: Empty]: {[x: {[y: Empty]: {[z: Empty]: Empty}}]: Empty}}}]", DumpHeap(heap3));
        }

        [TestMethod]
        public void InsertTest()
        {
            var empty = BootstrappedHeap<string>.Empty;
            var heap = BootstrappedHeap<string>.Insert("A", empty);
            Assert.AreEqual("A", BootstrappedHeap<string>.FindMin(heap));
        }

        [TestMethod]
        public void FindEmptyMinTest()
        {
            AssertThrows<ArgumentException>(() => BootstrappedHeap<string>.FindMin(BootstrappedHeap<string>.Empty));
        }

        [TestMethod]
        public void FindMinTest()
        {
            var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            Assert.AreEqual("a", BootstrappedHeap<string>.FindMin(ts1));
        }

        [TestMethod]
        public void DeleteEmptyMinTest()
        {
            AssertThrows<ArgumentException>(() => BootstrappedHeap<string>.DeleteMin(BootstrappedHeap<string>.Empty));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
            var ts2 = BootstrappedHeap<string>.DeleteMin(ts1);
            Assert.AreEqual("b", BootstrappedHeap<string>.FindMin(ts2));
        }
    }
}