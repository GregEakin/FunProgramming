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
    public class RealTimeQueueTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            Assert.IsTrue(RealTimeQueue<string>.IsEmpty(queue));
            queue = RealTimeQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(RealTimeQueue<string>.IsEmpty(queue));
            queue = RealTimeQueue<string>.Tail(queue);
            Assert.IsTrue(RealTimeQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyHeadTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            var item = RealTimeQueue<string>.Head(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyTailTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            var item = RealTimeQueue<string>.Tail(queue);
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = RealTimeQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = RealTimeQueue<string>.Tail(queue);
            }

            Assert.IsTrue(RealTimeQueue<string>.IsEmpty(queue));
        }
    }
}