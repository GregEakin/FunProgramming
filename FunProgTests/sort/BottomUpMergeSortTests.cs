﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;

namespace FunProgTests.sort
{
    using FunProgLib.sort;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class BottomUpMergeSortTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var list = BottomUpMergeSort<string>.Empty;
            // Assert.IsFalse(list.Segs.IsValueCreated);
            Assert.IsNull(list.Segs.Value);
            Assert.IsTrue(list.Segs.IsValueCreated);
        }

        [TestMethod]
        public void LazyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            Assert.IsFalse(list.Segs.IsValueCreated);
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.IsTrue(list.Segs.IsValueCreated);
        }

        [TestMethod]
        public void SimpleSortTest()
        {
            const string data = "How now, jack brown cow? zed";
            var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[brown, cow?, How, jack, now,, zed]", xs.ToReadableString());
        }

        [TestMethod]
        public void SortAlphabetically()
        {
            const string data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
            var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual(data.Split().ToReadableString(), xs.ToReadableString());
        }

        [TestMethod]
        public void SortReverseAlphabetically()
        {
            const string data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
            var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
            var xs = BottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual(data.Split().Reverse().ToReadableString(), xs.ToReadableString());
        }
    }
}
