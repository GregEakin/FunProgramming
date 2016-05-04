﻿// Fun Programming Data Structures 1.0
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
    using static FunProgTests.utilities.ExpectedException;

    [TestClass]
    public class BankersQueueTests
    {
        [TestMethod]
        public void NullTest()
        {
            // Assert.IsTrue(BankersQueue<string>.IsEmpty(null));
        }

        [TestMethod]
        public void EmptyTest()
        {
            var queue = BankersQueue<string>.Empty;
            Assert.IsTrue(BankersQueue<string>.IsEmpty(queue));
            queue = BankersQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(BankersQueue<string>.IsEmpty(queue));
            queue = BankersQueue<string>.Tail(queue);
            Assert.IsTrue(BankersQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BankersQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BankersQueue<string>.Head(queue));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BankersQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BankersQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = BankersQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BankersQueue<string>.IsEmpty(queue));
        }
    }
}