// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinomialHeapTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.heap
{
    using System;
    using System.Linq;
    using System.Text;

    using FunProgLib.heap;
    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BinomialHeapTests
    {
        private static string DumpTree<T>(BinomialHeap<T>.Tree tree) where T : IComparable
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(tree.Root);
            if (tree.List != LinkList<BinomialHeap<T>.Tree>.Empty)
            {
                result.Append(", ");
                foreach (var node1 in tree.List)
                {
                    result.Append(DumpTree(node1));
                }
            }
            result.Append("]");
            return result.ToString();
        }

        private static string DumpHeap<T>(LinkList<BinomialHeap<T>.Tree>.List list) where T : IComparable
        {
            var result = new StringBuilder();
            result.Append("[");
            if (list != LinkList<BinomialHeap<T>.Tree>.Empty)
            {
                foreach (var node in list)
                {
                    result.Append(DumpTree(node));
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
            var t = BinomialHeap<string>.Empty;
            Assert.IsTrue(BinomialHeap<string>.IsEmapty(t));

            var t1 = BinomialHeap<string>.Insert("C", t);
            Assert.IsFalse(BinomialHeap<string>.IsEmapty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = BinomialHeap<string>.Empty;
            var x1 = BinomialHeap<string>.Insert("C", t);
            var x2 = BinomialHeap<string>.Insert("B", x1);
            Assert.AreEqual("[[B, [C]]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet";
            var ts = Words.Split().Aggregate(BinomialHeap<string>.Empty, (current, word) => BinomialHeap<string>.Insert(word, current));
            Assert.AreEqual("[[as, [sweet]][a, [a, [call, [That, [which]][we]][in, [What's]][name?]][name, [smell, [would]][other]][any, [by]][rose]]]", DumpHeap(ts));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(BinomialHeap<string>.Empty, (current, word) => BinomialHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(BinomialHeap<string>.Empty, (current, word) => BinomialHeap<string>.Insert(word, current));

            var t = BinomialHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[[as, [sweet]][a, [a, [call, [That, [which]][we]][any, [by]][rose]][name, [smell, [would]][other]][in, [What's]][name?]]]", DumpHeap(t));

        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = BinomialHeap<int>.Empty;
            var t1 = BinomialHeap<int>.Insert(5, t);
            var t2 = BinomialHeap<int>.Insert(3, t1);
            var t3 = BinomialHeap<int>.Insert(6, t2);
            var t4 = BinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[[5, [6]]]", DumpHeap(t4));
            Assert.AreEqual(5, BinomialHeap<int>.FindMin(t4));

            Assert.AreEqual(3, BinomialHeap<int>.FindMin(t3));
        }
    }
}