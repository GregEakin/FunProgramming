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
using FunProgLib.queue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.queue
{
    [TestClass]
    public class BatchedQueueTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = BatchedQueue<string>.Empty;
            Assert.IsTrue(BatchedQueue<string>.IsEmpty(queue));
            queue = BatchedQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(BatchedQueue<string>.IsEmpty(queue));
            queue = BatchedQueue<string>.Tail(queue);
            Assert.IsTrue(BatchedQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var queue = BatchedQueue<string>.Empty;
            var item = BatchedQueue<string>.Head(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var queue = BatchedQueue<string>.Empty;
            var item = BatchedQueue<string>.Tail(queue);
        }
        
        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = BatchedQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BatchedQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BatchedQueue<string>.IsEmpty(queue));
        }
    }
}