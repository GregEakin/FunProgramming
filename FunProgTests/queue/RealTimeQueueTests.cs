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
    using FunProgTests.streams;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;
    using static utilities.ExpectedException;

    [TestClass]
    public class RealTimeQueueTests
    {
        public static string DumpQueue<T>(RealTimeQueue<T>.Queue queue, bool expandUnCreated)
        {
            if (RealTimeQueue<T>.IsEmpty(queue)) return string.Empty;

            var result = new StringBuilder();
            result.Append("[{");
            result.Append(StreamTests.DumpStream(queue.F, expandUnCreated));
            result.Append("}, ");
            result.Append(queue.R != null ? queue.R.ToReadableString() : "null");
            result.Append(", {");
            result.Append(StreamTests.DumpStream(queue.S, expandUnCreated));
            result.Append("}]");
            return result.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            var queue = RealTimeQueue<string>.Empty;
            Assert.AreEqual("", DumpQueue(queue, true));
        }

        [TestMethod]
        public void Test2()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
            Assert.AreEqual("[{One, Two, $Three}, [Three, One], {Three}]", DumpQueue(queue, true));
        }

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
        public void EmptySnocTest()
        {
            AssertThrows<NullReferenceException>(() => RealTimeQueue<string>.Snoc(null, "Item"));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            queue = RealTimeQueue<string>.Snoc(queue, "One");
            Assert.AreEqual("[{$}, null, {$}]", DumpQueue(queue, false));

            queue = RealTimeQueue<string>.Snoc(queue, "Two");
            Assert.AreEqual("[{One}, [Two], {}]", DumpQueue(queue, false));

            queue = RealTimeQueue<string>.Snoc(queue, "Three");
            Assert.AreEqual("[{$One, $Two, $Three}, null, {One, Two, Three}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
            var head = RealTimeQueue<string>.Head(queue);
            Assert.AreEqual("One", head);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = RealTimeQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
            var tail = RealTimeQueue<string>.Tail(queue);
            Assert.AreEqual("[{Two, Three}, [Three, One], {}]", DumpQueue(tail, true));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var head = RealTimeQueue<string>.Head(queue);
                Assert.AreEqual(expected, head);
                queue = RealTimeQueue<string>.Tail(queue);
            }

            Assert.IsTrue(RealTimeQueue<string>.IsEmpty(queue));
        }
    }
}