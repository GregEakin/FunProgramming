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
    using FunProgLib.queue;
    using FunProgLib.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;
    using static utilities.ExpectedException;

    [TestClass]
    public class BatchedQueueTests
    {
        private static string DumpQueue<T>(BatchedQueue<T>.Queue queue)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(queue.F?.ToReadableString() ?? "null");
            builder.Append(", ");
            builder.Append(queue.R?.ToReadableString() ?? "null");
            builder.Append("]");
            return builder.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            var queue = BatchedQueue<string>.Empty;
            Assert.AreEqual("[null, null]", DumpQueue(queue));
        }

        [TestMethod]
        public void Test2()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
            Assert.AreEqual("[[One], [Three, One, Three, Two]]", DumpQueue(queue));
        }

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
        public void EmptySnocTest()
        {
            AssertThrows<NullReferenceException>(() => BatchedQueue<string>.Snoc(null, "Item"));
        }

        [TestMethod]
        public void SnocTest()
        {

        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BatchedQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BatchedQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
            var head = BatchedQueue<string>.Head(queue);
            Assert.AreEqual("One", head);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BatchedQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BatchedQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
            var tail = BatchedQueue<string>.Tail(queue);
            Assert.AreEqual("[[Two, Three, One, Three], null]", DumpQueue(tail));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var actual = BatchedQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BatchedQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BatchedQueue<string>.IsEmpty(queue));
        }
    }
}
