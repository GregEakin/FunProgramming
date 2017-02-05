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
    using static utilities.ExpectedException;

    [TestClass]
    public class HoodMelvilleQueueTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            Assert.IsTrue(HoodMelvilleQueue<string>.IsEmpty(queue));
            queue = HoodMelvilleQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(HoodMelvilleQueue<string>.IsEmpty(queue));
            queue = HoodMelvilleQueue<string>.Tail(queue);
            Assert.IsTrue(HoodMelvilleQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => HoodMelvilleQueue<string>.Head(queue));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => HoodMelvilleQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = HoodMelvilleQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = HoodMelvilleQueue<string>.Tail(queue);
            }

            Assert.IsTrue(HoodMelvilleQueue<string>.IsEmpty(queue));
        }
    }
}