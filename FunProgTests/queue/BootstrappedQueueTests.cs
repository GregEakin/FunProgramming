// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.lists;
using System.Text;

namespace FunProgTests.queue
{
    using FunProgLib.queue;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using static utilities.ExpectedException;

    [TestClass]
    public class BootstrappedQueueTests
    {
        private static string DumpQueue<T>(BootstrappedQueue<T>.Queue queue)
        {
            return queue == null 
                ? "null" 
                : $"[{queue.LenFM}, {DumpList(queue.F)}, {DumpQueue(queue.M)}, {queue.LenR}, {DumpList(queue.R)}]";
        }

        private static string DumpList<T>(List<T>.Node list)
        {
            var result = new StringBuilder();
            result.Append("{");
            var separator = "";
            while (true)
            {
                if (list == null) break;
                result.Append(separator);
                separator = ", ";
                var head = List<T>.Head(list);
                result.Append(head);
                list = List<T>.Tail(list);
            }
            result.Append("}");
            return result.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));

            queue = BootstrappedQueue<string>.Snoc(queue, "Item");
            Assert.IsFalse(BootstrappedQueue<string>.IsEmpty(queue));

            queue = BootstrappedQueue<string>.Tail(queue);
            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptySnocTest()
        {
            var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
            Assert.AreEqual("[1, {one}, null, 0, {}]", DumpQueue(queue));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            queue = BootstrappedQueue<string>.Snoc(queue, "One");
            Assert.AreEqual("[1, {One}, null, 0, {}]", DumpQueue(queue));

            queue = BootstrappedQueue<string>.Snoc(queue, "Two");
            Assert.AreEqual("[1, {One}, null, 1, {Two}]", DumpQueue(queue));

            queue = BootstrappedQueue<string>.Snoc(queue, "Three");
            Assert.AreEqual("[3, {One}, [1, {Value is not created.}, null, 0, {}], 0, {}]", DumpQueue(queue));
        }

        [TestMethod]
        public void SnocThreeTest()
        {
            var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
            queue = BootstrappedQueue<string>.Snoc(queue, "two");
            queue = BootstrappedQueue<string>.Snoc(queue, "three");
            Assert.AreEqual("[3, {one}, [1, {Value is not created.}, null, 0, {}], 0, {}]", DumpQueue(queue));

            queue = BootstrappedQueue<string>.Tail(queue);
            Assert.AreEqual("[2, {two, three}, null, 0, {}]", DumpQueue(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BootstrappedQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
            var x = BootstrappedQueue<string>.Head(queue);
            Assert.AreEqual("One", x);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BootstrappedQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BootstrappedQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
            queue = BootstrappedQueue<string>.Tail(queue);
            Assert.AreEqual("[2, {Two, Three}, null, 2, {Three, One}]", DumpQueue(queue));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var actual = BootstrappedQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BootstrappedQueue<string>.Tail(queue);
            }

            Assert.IsTrue(BootstrappedQueue<string>.IsEmpty(queue));
        }
    }
}