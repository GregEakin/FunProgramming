// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		PhysicistQueueTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.queue
{
    using System.Linq;

    using FunProgLib.queue;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PhysicistQueueTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            Assert.IsTrue(PhysicistsQueue<string>.IsEmpty(queue));
            queue = PhysicistsQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(PhysicistsQueue<string>.IsEmpty(queue));
            queue = PhysicistsQueue<string>.Tail(queue);
            Assert.IsTrue(PhysicistsQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = PhysicistsQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = PhysicistsQueue<string>.Tail(queue);
            }
        }
    }
}