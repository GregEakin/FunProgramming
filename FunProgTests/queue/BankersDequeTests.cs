// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.queue
{
    using System;
    using System.Linq;

    using FunProgLib.queue;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BankersDequeTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = BankersDeque<string>.Empty;
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));

            queue = BankersDeque<string>.Cons("Head", queue);
            Assert.IsFalse(BankersDeque<string>.IsEmpty(queue));
            queue = BankersDeque<string>.Tail(queue);
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));

            queue = BankersDeque<string>.Snoc(queue, "Tail");
            Assert.IsFalse(BankersDeque<string>.IsEmpty(queue));
            queue = BankersDeque<string>.Init(queue);
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void ConsTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Cons("Last", queue);
            queue = BankersDeque<string>.Cons("Head", queue);

            var head = BankersDeque<string>.Head(queue);
            Assert.AreEqual("Head", head);

            var last = BankersDeque<string>.Last(queue);
            Assert.AreEqual("Last", last);
        }

        [TestMethod]
        public void ConsHeadTailTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersDeque<string>.Empty, (queue1, s) => BankersDeque<string>.Cons(s, queue1));

            foreach (var expected in Data.Split().Reverse())
            {
                var actual = BankersDeque<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersDeque<string>.Tail(queue);
            }

            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Snoc(queue, "Head");
            queue = BankersDeque<string>.Snoc(queue, "Last");

            var head = BankersDeque<string>.Head(queue);
            Assert.AreEqual("Head", head);

            var last = BankersDeque<string>.Last(queue);
            Assert.AreEqual("Last", last);
        }

        [TestMethod]
        public void SnocLastInitTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersDeque<string>.Empty, BankersDeque<string>.Snoc);

            var dat = Data.Split().Reverse();
            foreach (var expected in dat)
            {
                var actual = BankersDeque<string>.Last(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersDeque<string>.Init(queue);
            }

            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var queue = BankersDeque<string>.Empty;
            var head = BankersDeque<string>.Head(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Tail(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyLastTest()
        {
            var queue = BankersDeque<string>.Empty;
            var head = BankersDeque<string>.Last(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyInitTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Init(queue);
        }
    }
}