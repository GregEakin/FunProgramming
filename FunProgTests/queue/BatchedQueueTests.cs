// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BatchedQueueTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.queue
{
    using System.Linq;

    using FunProgLib.queue;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split(null).Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);

            foreach (var expected in Data.Split(null))
            {
                var actual = BatchedQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BatchedQueue<string>.Tail(queue);
            }
        }
    }
}