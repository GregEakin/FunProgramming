// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.queue
{
    using FunProgLib.queue;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using static streams.StreamTests;
    using static utilities.ExpectedException;

    [TestClass]
    public class BankersQueueTests
    {
        private static string DumpQueue<T>(BankersQueue<T>.Queue queue, bool expandUnCreated)
        {
            return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
        }

        [TestMethod]
        public void NullTest()
        {
            AssertThrows<NullReferenceException>(() => BankersQueue<string>.IsEmpty(null));
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
        public void NullSnocTest()
        {
            var ex = AssertThrows<NullReferenceException>(() => BankersQueue<string>.Snoc(null, "one"));
            Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
        }

        [TestMethod]
        public void EmptySnocTest()
        {
            var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
            Assert.AreEqual("[1, {$one}, 0, {}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
            queue = BankersQueue<string>.Snoc(queue, "two");
            Assert.AreEqual("[1, {$one}, 1, {$two}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void NullHeadTest()
        {
            var ex = AssertThrows<NullReferenceException>(() => BankersQueue<string>.Head(null));
            Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BankersQueue<string>.Empty;
            var ex = AssertThrows<ArgumentNullException>(() => BankersQueue<string>.Head(queue));
            Assert.AreEqual("Value cannot be null.\r\n" + "Parameter name: queue", ex.Message);
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
            var item = BankersQueue<string>.Head(queue);
            Assert.AreEqual("One", item);
        }

        [TestMethod]
        public void NullTailTest()
        {
            var ex = AssertThrows<NullReferenceException>(() => BankersQueue<string>.Tail(null));
            Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BankersQueue<string>.Empty;
            var ex = AssertThrows<ArgumentNullException>(() => BankersQueue<string>.Tail(queue));
            Assert.AreEqual("Value cannot be null.\r\n" + "Parameter name: queue", ex.Message);
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
            var tail = BankersQueue<string>.Tail(queue);
            Assert.AreEqual("[2, {$Two, $Three}, 2, {$Three, $One}]", DumpQueue(tail, true));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var actual = BankersQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BankersQueue<string>.IsEmpty(queue));
        }
    }
}