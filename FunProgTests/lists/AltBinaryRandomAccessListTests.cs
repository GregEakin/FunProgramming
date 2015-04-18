// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Linq;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.lists
{
    [TestClass]
    public class AltBinaryRandomAccessListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            Assert.IsTrue(list.IsEmpty);
            list = list.Cons("A");
            Assert.IsFalse(list.IsEmpty);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var hd = list.Head;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var tl = list.Tail;
        }

        [TestMethod]
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));

            Assert.AreEqual("now,", data.Lookup(2));
            Assert.AreEqual("now,", data.Lookup(2));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            data = data.Fupdate(y => "green", 1);
            data = data.Fupdate(y => "white", 1);
            Assert.AreEqual("cow?", data.Lookup(0));
            Assert.AreEqual("white", data.Lookup(1));
            Assert.AreEqual("now,", data.Lookup(2));
        }


        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            Assert.AreEqual("cow?", data.Head);

            data = data.Fupdate(y => "dog?", 0);
            Assert.AreEqual("dog?", data.Head);
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            data = data.Tail;
            Assert.AreEqual("brown", data.Lookup(0));
            Assert.AreEqual("now,", data.Lookup(1));
            Assert.AreEqual("How", data.Lookup(2));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            Assert.AreEqual("sweet.", data.Lookup(0));
            Assert.AreEqual("What's", data.Lookup(17));
        }

        [TestMethod]
        public void UnconsTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            var data5 = data.Uncons;
            Assert.AreEqual("sweet.", data5.Item1);

            var data6 = data5.Item2;
            Assert.AreEqual("smell", data6.Lookup(1));
            Assert.AreEqual("as", data6.Lookup(0));
        }
    }
}