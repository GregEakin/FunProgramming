// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinomialHeapTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.tree
{
    using System.Linq;

    using FunProgLib.tree;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BinomialHeapTests
    {
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
            Assert.AreEqual("[[B, [C]]]", BinomialHeap<string>.DumpString(x2));
        }

        [TestMethod]
        public void Test2()
        {
            var t = BinomialHeap<string>.Empty;
            var words = "what's in a name that which we call a rose by any other name would smell as sweet";
            t = words.Split(null).Aggregate(t, (current, word) => BinomialHeap<string>.Insert(word, current));
            Assert.AreEqual("[[as, [sweet]][a, [a, [call, [that, [which]][we]][in, [what's]][name]][name, [smell, [would]][other]][any, [by]][rose]]]", BinomialHeap<string>.DumpString(t));
        }

        [TestMethod]
        public void DeleteMinTest()
        {
            var t = BinomialHeap<int>.Empty;
            var t1 = BinomialHeap<int>.Insert(5, t);
            var t2 = BinomialHeap<int>.Insert(3, t1);
            var t3 = BinomialHeap<int>.Insert(6, t2);
            var t4 = BinomialHeap<int>.DeleteMin(t3);
            Assert.AreEqual("[[5, [6]]]", BinomialHeap<int>.DumpString(t4));
            Assert.AreEqual(5, BinomialHeap<int>.FindMin(t4));

            Assert.AreEqual(3, BinomialHeap<int>.FindMin(t3));
        }
    }
}