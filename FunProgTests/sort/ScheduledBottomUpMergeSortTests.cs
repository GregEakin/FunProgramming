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
    using System.Linq;

    using FunProgLib.sort;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ScheduledBottomUpMergeSortTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var list = ScheduledBottomUpMergeSort<string>.Empty;
            Assert.AreEqual(0, list.Size);
            Assert.IsNull(list.Segs);

            list = ScheduledBottomUpMergeSort<string>.Add("One", list);
            Assert.AreEqual(1, list.Size);
            Assert.IsNotNull(list.Segs);
        }

        [TestMethod]
        public void LazyTest()
        {
            var list = ScheduledBottomUpMergeSort<string>.Empty;
            list = ScheduledBottomUpMergeSort<string>.Add("One", list);

            Assert.IsFalse(list.Segs.Element.Stream.IsValueCreated);
            Assert.AreEqual("One", list.Segs.Element.Stream.Value.Element);
            Assert.IsTrue(list.Segs.Element.Stream.IsValueCreated);
        }

        [TestMethod]
        public void SortLazyTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
            Assert.IsFalse(list.Segs.Element.Stream.IsValueCreated);
            var sorted = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.IsTrue(list.Segs.Element.Stream.IsValueCreated);
        }

        [TestMethod]
        public void AddTest()
        {
            var list = ScheduledBottomUpMergeSort<string>.Empty;
            list = ScheduledBottomUpMergeSort<string>.Add("One", list);
            Assert.AreEqual(1, list.Size);
            var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[One]", xs.ToReadableString());

            list = ScheduledBottomUpMergeSort<string>.Add("Two", list);
            Assert.AreEqual(2, list.Size);
            xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[One, Two]", xs.ToReadableString());

            list = ScheduledBottomUpMergeSort<string>.Add("Three", list);
            Assert.AreEqual(3, list.Size);
            xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[One, Three, Two]", xs.ToReadableString());

            list = ScheduledBottomUpMergeSort<string>.Add("Four", list);
            Assert.AreEqual(4, list.Size);
            xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[Four, One, Three, Two]", xs.ToReadableString());
        }

        [TestMethod]
        public void SimpleSortTest()
        {
            const string data = "How now, jack brown cow? zed";
            var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
            var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual("[brown, cow?, How, jack, now,, zed]", xs.ToReadableString());
        }

        [TestMethod]
        public void SortAlphabetically()
        {
            const string data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
            var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
            var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual(data.Split().ToReadableString(), xs.ToReadableString());
        }

        [TestMethod]
        public void SortReverseAlphabetically()
        {
            const string data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
            var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
            var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
            Assert.AreEqual(data.Split().Reverse().ToReadableString(), xs.ToReadableString());
        }
    }
}
