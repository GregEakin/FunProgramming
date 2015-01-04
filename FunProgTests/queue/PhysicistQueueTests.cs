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
        [ExpectedException(typeof(Exception))]
        public void EmptyHeadTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            var item = PhysicistsQueue<string>.Head(queue);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyTailTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            var item = PhysicistsQueue<string>.Tail(queue);
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

            Assert.IsTrue(PhysicistsQueue<string>.IsEmpty(queue));
        }
    }
}