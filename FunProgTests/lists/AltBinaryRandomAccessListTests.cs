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

        [TestMethod]
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

            Assert.AreEqual("now,", data.Lookup(2));
        }

        [TestMethod]
        public void UpdateTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = data.Fupdate(y => "green", 1);
            Assert.AreEqual("green", data.Lookup(1));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("cow?", AltBinaryRandomAccessList<string>.Head(data));

            data = data.Fupdate(y => "dog?", 0);
            Assert.AreEqual("dog?", AltBinaryRandomAccessList<string>.Head(data));
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            data = AltBinaryRandomAccessList<string>.Tail(data);
            Assert.AreEqual("brown", data.Lookup(0));
            Assert.AreEqual("now,", data.Lookup(1));
            Assert.AreEqual("How", data.Lookup(2));
        }

        [TestMethod]
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
            Assert.AreEqual("sweet.", data.Lookup(0));
            Assert.AreEqual("What's", data.Lookup(17));
        }

        [TestMethod]
        public void Test1A()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

            var data5 = data.Uncons();
            Assert.AreEqual("sweet.", data5.Item1);

            var data6 = data5.Item2;
            Assert.AreEqual("smell", data6.Lookup(1));
            Assert.AreEqual("as", data6.Lookup(0));
        }

        [TestMethod]
        public void Test1()
        {
            var data1 = AltBinaryRandomAccessList<string>.Empty;

            var data2 = AltBinaryRandomAccessList<string>.Cons("One", data1);
            Assert.AreEqual("One", data2.Lookup(0));

            var data3 = AltBinaryRandomAccessList<string>.Cons("Two", data2);
            Assert.AreEqual("One", data3.Lookup(1));
            Assert.AreEqual("Two", data3.Lookup(0));

            var data4 = AltBinaryRandomAccessList<string>.Cons("Three", data3);
            Assert.AreEqual("One", data4.Lookup(2));
            Assert.AreEqual("Two", data4.Lookup(1));
            Assert.AreEqual("Three", data4.Lookup(0));

            var data5 = data4.Uncons();
            Assert.AreEqual("Three", data5.Item1);

            var data6 = data5.Item2;
            Assert.AreEqual("One", data6.Lookup(1));
            Assert.AreEqual("Two", data6.Lookup(0));
        }

        [TestMethod]
        public void Test2()
        {
            var data = AltBinaryRandomAccessList<string>.Empty;
            data = AltBinaryRandomAccessList<string>.Cons("One", data);
            data = AltBinaryRandomAccessList<string>.Cons("Two", data);

            var data3 = AltBinaryRandomAccessList<string>.Update(0, "Three", data);
            Assert.AreEqual("One", data3.Lookup(1));
            Assert.AreEqual("Three", data3.Lookup(0));
        }

        [TestMethod]
        public void Test3()
        {
            var data = AltBinaryRandomAccessList<string>.Empty;
            data = AltBinaryRandomAccessList<string>.Cons("One", data);

            var data3 = AltBinaryRandomAccessList<string>.Update(0, "Three", data);
            Assert.AreEqual("Three", data3.Lookup(0));
        }

        [TestMethod]
        public void Test4()
        {
            var data = AltBinaryRandomAccessList<string>.Empty;
            data = AltBinaryRandomAccessList<string>.Cons("One", data);
            data = AltBinaryRandomAccessList<string>.Cons("Two", data);

            var data3 = AltBinaryRandomAccessList<string>.Update(1, "Three", data);
            Assert.AreEqual("Three", data3.Lookup(1));
            Assert.AreEqual("Two", data3.Lookup(0));
        }
    }
}