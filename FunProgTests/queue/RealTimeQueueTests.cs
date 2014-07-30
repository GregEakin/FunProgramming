// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		RealTimeQueueTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.queue
{
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