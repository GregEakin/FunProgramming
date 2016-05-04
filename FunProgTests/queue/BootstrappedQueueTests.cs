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
    using static FunProgTests.utilities.ExpectedException;

    [TestClass]
    public class BootstrappedQueueTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));
            queue = BootstrappedQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(BootstrappedQueue<string>.IsEmpty(queue));
            queue = BootstrappedQueue<string>.Tail(queue);
            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BootstrappedQueue<string>.Head(queue));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BootstrappedQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = BootstrappedQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BootstrappedQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));
        }
    }
}