// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.lists
{
    using System;
    using System.Linq;

    using FunProgLib.lists;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static utilities.ExpectedException;

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
        public void EmptyHeadTest()
        {
            var list = SkewBinaryRandomAccessList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => SkewBinaryRandomAccessList<string>.Head(list));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = SkewBinaryRandomAccessList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => SkewBinaryRandomAccessList<string>.Tail(list));
        }

        [TestMethod]
        public void LookupTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", SkewBinaryRandomAccessList<string>.Lookup(2, list));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            list = SkewBinaryRandomAccessList<string>.Update(1, "green", list);
            Assert.AreEqual("green", SkewBinaryRandomAccessList<string>.Lookup(1, list));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", SkewBinaryRandomAccessList<string>.Head(list));

            list = SkewBinaryRandomAccessList<string>.Update(0, "dog?", list);
            Assert.AreEqual("dog?", SkewBinaryRandomAccessList<string>.Head(list));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            list = SkewBinaryRandomAccessList<string>.Tail(list);
            Assert.AreEqual("brown", SkewBinaryRandomAccessList<string>.Lookup(0, list));
            Assert.AreEqual("now,", SkewBinaryRandomAccessList<string>.Lookup(1, list));
            Assert.AreEqual("How", SkewBinaryRandomAccessList<string>.Lookup(2, list));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", SkewBinaryRandomAccessList<string>.Lookup(0, list));
            Assert.AreEqual("What's", SkewBinaryRandomAccessList<string>.Lookup(17, list));
        }
    }
}