// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinaryRandomAccessListTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.persistence
{
    using System;
    using System.Linq;

    using FunProgLib.persistence;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = BinaryRandomAccessList<string>.Empty;
            var hd = BinaryRandomAccessList<string>.Head(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = BinaryRandomAccessList<string>.Empty;
            var tl = BinaryRandomAccessList<string>.Tail(list);
        }

        [TestMethod]
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", BinaryRandomAccessList<string>.Lookup(2, data));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            data = BinaryRandomAccessList<string>.Update(1, "green", data);
            Assert.AreEqual("green", BinaryRandomAccessList<string>.Lookup(1, data));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", BinaryRandomAccessList<string>.Head(data));

            data = BinaryRandomAccessList<string>.Update(0, "dog?", data);
            Assert.AreEqual("dog?", BinaryRandomAccessList<string>.Head(data));
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            data = BinaryRandomAccessList<string>.Tail(data);
            Assert.AreEqual("brown", BinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("now,", BinaryRandomAccessList<string>.Lookup(1, data));
            Assert.AreEqual("How", BinaryRandomAccessList<string>.Lookup(2, data));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", BinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("What's", BinaryRandomAccessList<string>.Lookup(17, data));
        }
    }
}