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
    public class BinaryRandomAccessListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = BinaryRandomAccessList<string>.Empty;
            Assert.IsTrue(BinaryRandomAccessList<string>.IsEmpty(list));
            list = BinaryRandomAccessList<string>.Cons("A", list);
            Assert.IsFalse(BinaryRandomAccessList<string>.IsEmpty(list));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var list = BinaryRandomAccessList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BinaryRandomAccessList<string>.Head(list));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var list = BinaryRandomAccessList<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BinaryRandomAccessList<string>.Tail(list));
        }

        [TestMethod]
        public void LookupTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", BinaryRandomAccessList<string>.Lookup(2, list));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            list = BinaryRandomAccessList<string>.Update(1, "green", list);
            Assert.AreEqual("green", BinaryRandomAccessList<string>.Lookup(1, list));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", BinaryRandomAccessList<string>.Head(list));

            list = BinaryRandomAccessList<string>.Update(0, "dog?", list);
            Assert.AreEqual("dog?", BinaryRandomAccessList<string>.Head(list));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "How now, brown cow?";
            var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            list = BinaryRandomAccessList<string>.Tail(list);
            Assert.AreEqual("brown", BinaryRandomAccessList<string>.Lookup(0, list));
            Assert.AreEqual("now,", BinaryRandomAccessList<string>.Lookup(1, list));
            Assert.AreEqual("How", BinaryRandomAccessList<string>.Lookup(2, list));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", BinaryRandomAccessList<string>.Lookup(0, list));
            Assert.AreEqual("What's", BinaryRandomAccessList<string>.Lookup(17, list));
        }
    }
}