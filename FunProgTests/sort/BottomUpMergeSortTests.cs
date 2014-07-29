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
        public void LazyTest()
        {
            const string Data = "How now, brown cow?";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            Assert.IsFalse(list.Segs.IsValueCreated);
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.IsTrue(list.Segs.IsValueCreated);
        }

        [TestMethod]
        public void SimpleSortTest()
        {
            const string Data = "How now, jack brown cow? zed";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            CollectionAssert.AreEqual(new[] { "brown", "cow?", "How", "jack", "now,", "zed" }, xs.ToList());
        }

        [TestMethod]
        public void SortAlphabetically()
        {
            const string Data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            CollectionAssert.AreEqual(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu" }, xs.ToList());
        }

        [TestMethod]
        public void SortReverseAlphabetically()
        {
            const string Data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
            var list = Data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            CollectionAssert.AreEqual(new[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Quebec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu" }, xs.ToList());
        }
    }
}
