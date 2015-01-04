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
    public class SkewBinaryRandomAccessListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = SkewBinaryRandomAccessList<string>.Empty;
            Assert.IsTrue(SkewBinaryRandomAccessList<string>.IsEmpty(list));
            list = SkewBinaryRandomAccessList<string>.Cons("A", list);
            Assert.IsFalse(SkewBinaryRandomAccessList<string>.IsEmpty(list));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = SkewBinaryRandomAccessList<string>.Empty;
            var hd = SkewBinaryRandomAccessList<string>.Head(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = SkewBinaryRandomAccessList<string>.Empty;
            var tl = SkewBinaryRandomAccessList<string>.Tail(list);
        }

        [TestMethod]
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", SkewBinaryRandomAccessList<string>.Lookup(2, data));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            data = SkewBinaryRandomAccessList<string>.Update(1, "green", data);
            Assert.AreEqual("green", SkewBinaryRandomAccessList<string>.Lookup(1, data));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", SkewBinaryRandomAccessList<string>.Head(data));

            data = SkewBinaryRandomAccessList<string>.Update(0, "dog?", data);
            Assert.AreEqual("dog?", SkewBinaryRandomAccessList<string>.Head(data));
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            data = SkewBinaryRandomAccessList<string>.Tail(data);
            Assert.AreEqual("brown", SkewBinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("now,", SkewBinaryRandomAccessList<string>.Lookup(1, data));
            Assert.AreEqual("How", SkewBinaryRandomAccessList<string>.Lookup(2, data));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", SkewBinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("What's", SkewBinaryRandomAccessList<string>.Lookup(17, data));
        }
    }
}