// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BottomUpMergeSortTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.sort
{
    using System.Linq;

    using FunProgLib.sort;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BottomUpMergeSortTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var list = BottomUpMergeSort<string>.Empty;
            Assert.IsFalse(list.Segs.IsValueCreated);
            Assert.IsNull(list.Segs.Value);
            Assert.IsTrue(list.Segs.IsValueCreated);
        }

        [TestMethod]
        public void SortTest()
        {
            const string Data = "How now, jack brown cow? zed";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, BottomUpMergeSort<string>.Add);
            var xs = BottomUpMergeSort<string>.Sort(list);
            CollectionAssert.AreEqual(new[] { "brown", "cow?", "How", "jack", "now,", "zed" }, xs.ToList());
        }

        [TestMethod]
        public void LazyTest()
        {
            const string Data = "How now, jack brown cow? zed";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, BottomUpMergeSort<string>.Add);
            Assert.IsFalse(list.Segs.IsValueCreated);
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.IsTrue(list.Segs.IsValueCreated);
        }
    }
}