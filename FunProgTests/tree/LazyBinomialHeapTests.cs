// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LazyBinomialHeapTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.tree
{
    using System;
    using System.Linq;
    using System.Text;

    using FunProgLib.persistence;
    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LazyBinomialHeapTests
    {
        private static string DumpNode<T>(LazyBinomialHeap<T>.Tree tree) where T : IComparable
        {
            var result = new StringBuilder();
            result.Append("[");
            result.Append(tree.Root);
            if (tree.List != LinkList<LazyBinomialHeap<T>.Tree>.Empty)
            {
                result.Append(", ");
                foreach (var node1 in tree.List)
                {
                    result.Append(DumpNode(node1));
                }
            }
            result.Append("]");
            return result.ToString();
        }

        private static string DumpHeap<T>(Lazy<LinkList<LazyBinomialHeap<T>.Tree>.List> list) where T : IComparable
        {
            var result = new StringBuilder();
            result.Append("[");
            if (list.Value != LinkList<LazyBinomialHeap<T>.Tree>.Empty)
            {
                foreach (var node in list.Value)
                {
                    result.Append(DumpNode(node));
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
            var t = LazyBinomialHeap<string>.Empty;
            Assert.IsTrue(LazyBinomialHeap<string>.IsEmapty(t));

            var t1 = LazyBinomialHeap<string>.Insert("C", t);
            Assert.IsFalse(LazyBinomialHeap<string>.IsEmapty(t1));
        }

        [TestMethod]
        public void Test1()
        {
            var t = LazyBinomialHeap<string>.Empty;
            var x1 = LazyBinomialHeap<string>.Insert("C", t);
            var x2 = LazyBinomialHeap<string>.Insert("B", x1);
            Assert.AreEqual("[[B, [C]]]", DumpHeap(x2));
        }

        [TestMethod]
        public void Test2()
        {
            const string Words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var t = Words.Split().Aggregate(LazyBinomialHeap<string>.Empty, (current, word) => LazyBinomialHeap<string>.Insert(word, current));
            Assert.AreEqual("[[as, [sweet.]][a, [a, [call, [That, [which]][we]][in, [What's]][name?]][name, [smell, [would]][other]][any, [by]][rose]]]", DumpHeap(t));
        }

        [TestMethod]
        public void MergeTest()
        {
            const string Data1 = "What's in a name?";
            var ts1 = Data1.Split().Aggregate(LazyBinomialHeap<string>.Empty, (current, word) => LazyBinomialHeap<string>.Insert(word, current));

            const string Data2 = "That which we call a rose by any other name would smell as sweet";
            var ts2 = Data2.Split().Aggregate(LazyBinomialHeap<string>.Empty, (current, word) => LazyBinomialHeap<string>.Insert(word, current));

            var t = LazyBinomialHeap<string>.Merge(ts1, ts2);
            Assert.AreEqual("[[as, [sweet]][a, [a, [call, [That, [which]][we]][any, [by]][rose]][name, [smell, [would]][other]][in, [What's]][name?]]]", DumpHeap(t));

        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = LazyBinomialHeap<int>.Empty;
            var t1 = LazyBinomialHeap<int>.Insert(5, t);
            var t2 = LazyBinomialHeap<int>.Insert(3, t1);
            var t3 = LazyBinomialHeap<int>.Insert(6, t2);
            var t4 = LazyBinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[[5, [6]]]", DumpHeap(t4));
            Assert.AreEqual(5, LazyBinomialHeap<int>.FindMin(t4));

            Assert.AreEqual(3, LazyBinomialHeap<int>.FindMin(t3));
        }
    }
}