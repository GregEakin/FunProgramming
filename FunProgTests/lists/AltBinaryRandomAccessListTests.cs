// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		AltBinaryRandomAccessListTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.persistence
{
    using System;
    using System.Linq;

    using FunProgLib.persistence;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AltBinaryRandomAccessListTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            Assert.IsTrue(AltBinaryRandomAccessList<string>.IsEmpty(list));
            list = AltBinaryRandomAccessList<string>.Cons("A", list);
            Assert.IsFalse(AltBinaryRandomAccessList<string>.IsEmpty(list));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var hd = AltBinaryRandomAccessList<string>.Head(list);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = AltBinaryRandomAccessList<string>.Empty;
            var tl = AltBinaryRandomAccessList<string>.Tail(list);
        }

//        [TestMethod]
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", AltBinaryRandomAccessList<string>.Lookup(2, data));
        }

//        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = AltBinaryRandomAccessList<string>.Update(1, "green", data);
            Assert.AreEqual("green", AltBinaryRandomAccessList<string>.Lookup(1, data));
        }

//        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", AltBinaryRandomAccessList<string>.Head(data));

            data = AltBinaryRandomAccessList<string>.Update(0, "dog?", data);
            Assert.AreEqual("dog?", AltBinaryRandomAccessList<string>.Head(data));
        }

//        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = AltBinaryRandomAccessList<string>.Tail(data);
            Assert.AreEqual("brown", AltBinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("now,", AltBinaryRandomAccessList<string>.Lookup(1, data));
            Assert.AreEqual("How", AltBinaryRandomAccessList<string>.Lookup(2, data));
        }

//        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", AltBinaryRandomAccessList<string>.Lookup(0, data));
            Assert.AreEqual("What's", AltBinaryRandomAccessList<string>.Lookup(17, data));
        }
    }
}