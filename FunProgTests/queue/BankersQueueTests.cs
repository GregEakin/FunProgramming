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
    using FunProgLib.streams;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;
    using static utilities.ExpectedException;

    [TestClass]
    public class BankersQueueTests
    {
        private static string DumpList<T>(Stream<T>.StreamCell list)
        {
            if (list == null) return "{}";

            var builder = new StringBuilder();
            builder.Append("{");
            builder.Append(list.Element);
            builder.Append(", ");
            builder.Append(DumpList(list.Next.Value));
            builder.Append("}");
            return builder.ToString();
        }

        private static string DumpQueue<T>(BankersQueue<T>.Queue queue)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(queue.LenF);
            builder.Append(", ");
            builder.Append(DumpList(queue.F.Value));
            builder.Append(", ");
            builder.Append(queue.LenR);
            builder.Append(", ");
            builder.Append(DumpList(queue.R.Value));
            builder.Append("]");
            return builder.ToString();
        }

        [TestMethod]
        public void NullTest()
        {
            AssertThrows<ArgumentNullException>(() => BankersQueue<string>.IsEmpty(null));
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
            AssertThrows<ArgumentNullException>(() => BankersQueue<string>.Snoc(null, "one"));
        }

        [TestMethod]
        public void EmptySnocTest()
        {
            var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
            Assert.AreEqual("[1, {one, {}}, 0, {}]", DumpQueue(queue));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
            queue = BankersQueue<string>.Snoc(queue, "two");
            Assert.AreEqual("[1, {one, {}}, 1, {two, {}}]", DumpQueue(queue));
        }

        [TestMethod]
        public void NullHeadTest()
        {
            AssertThrows<ArgumentNullException>(() => BankersQueue<string>.Head(null));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BankersQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BankersQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
            var item = BankersQueue<string>.Head(queue);
            Assert.AreEqual("One", item);
        }

        [TestMethod]
        public void NullTailTest()
        {
            AssertThrows<ArgumentNullException>(() => BankersQueue<string>.Tail(null));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BankersQueue<string>.Empty;
            AssertThrows<ArgumentException>(() => BankersQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
            var tail = BankersQueue<string>.Tail(queue);
            Assert.AreEqual("[2, {Two, {Three, {}}}, 2, {Three, {One, {}}}]", DumpQueue(tail));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);

            foreach (var expected in Data.Split())
            {
                var actual = BankersQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BankersQueue<string>.IsEmpty(queue));
        }
    }
}