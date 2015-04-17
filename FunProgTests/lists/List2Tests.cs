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
    public class List2Tests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            var list = List2<string>.Empty;
            Assert.IsTrue(list.IsEmpty);
            list = list.Cons("A");
            Assert.IsFalse(list.IsEmpty);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var list = List2<string>.Empty;
            var hd = list.Head;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var list = List2<string>.Empty;
            var tl = list.Tail;
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            const string Data = "a b c";
            var list = Data.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));
            CollectionAssert.AreEqual(new[] { "c", "b", "a" }, ((List2<string>)list).ToArray());
        }

        [TestMethod]
        public void ReverseEmptyListTest()
        {
            var list = ((List2<string>)List2<string>.Empty).Reverse;
            Assert.IsTrue(list.IsEmpty);
        }

        [TestMethod]
        public void ReverseSingleListTest()
        {
            var list = new List2<string>("Wow");
            var reverse = list.Reverse;
            Assert.AreSame(list, reverse);
        }

        [TestMethod]
        public void ReverseListTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));
            var list = ((List2<string>)data).Reverse;
            CollectionAssert.AreEqual(new[] { "How", "now,", "brown", "cow?" }, ((List2<string>)list).ToArray());
        }

        [TestMethod]
        public void CatBothEmptyTest()
        {
            var list = ((List2<string>)List2<string>.Empty).Cat(List2<string>.Empty);
            Assert.IsTrue(list.IsEmpty);
        }

        [TestMethod]
        public void CatLeftEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));

            var list = ((List2<string>)List2<string>.Empty).Cat(data);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatRightEmptyTest()
        {
            const string Data = "How now, brown cow?";
            var data = Data.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));

            var list = ((List2<string>)data).Cat(List2<string>.Empty);
            Assert.AreSame(data, list);
        }

        [TestMethod]
        public void CatTest()
        {
            const string Data1 = "How now,";
            var data1 = Data1.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));

            const string Data2 = "brown cow?";
            var data2 = Data2.Split().Aggregate(List2<string>.Empty, (current, word) => current.Cons(word));

            var list = ((List2<string>)data1).Cat(data2);
            CollectionAssert.AreEqual(new[] { "now,", "How", "cow?", "brown" }, ((List2<string>)list).ToArray());
        }
    }
}