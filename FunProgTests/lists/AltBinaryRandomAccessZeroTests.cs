// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Linq;
using FunProgLib.lists;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.lists
{
    [TestClass]
    public class AltBinaryRandomAccessZeroTests
    {
        private readonly AltBinaryRandomAccessList<string> list = AltBinaryRandomAccessList<string>.Empty.Cons("One").Cons("Zero");

        [TestMethod]
        public void EmptyTest()
        {
            Assert.IsFalse(list.IsEmpty);
        }

        [TestMethod]
        public void ConsTest()
        {
            var list1 = list.Cons("Test");
            // Assert.IsNotInstanceOfType(list1, typeof(AltBinaryRandomAccessZero<string>));
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
        public void LookupTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));

            Assert.AreEqual("now,", data.Lookup(2));
            Assert.AreEqual("now,", data.Lookup(2));
        }

        [TestMethod]
        public void FupdateTest()
        {
            var tl = list.Fupdate(x => "test", 0);
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
        public void RoseTest()
        {
            const string Data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
            var data = Data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => current.Cons(word));
            Assert.AreEqual("sweet.", data.Lookup(0));
            Assert.AreEqual("What's", data.Lookup(17));
        }
    }
}