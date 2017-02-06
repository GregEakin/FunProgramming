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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Text;
    using static streams.StreamTests;
    using static utilities.ExpectedException;

    [TestClass]
    public class RealTimeDequeTests
    {
        private static string DumpQueue<T>(RealTimeDeque<T>.Queue queue, bool expandUnCreated)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(queue.LenF);
            builder.Append(", {");
            builder.Append(DumpStream(queue.F, expandUnCreated));
            builder.Append("}, {");
            builder.Append(DumpStream(queue.Sf, expandUnCreated));
            builder.Append("}, ");
            builder.Append(queue.LenR);
            builder.Append(", {");
            builder.Append(DumpStream(queue.R, expandUnCreated));
            builder.Append("}, {");
            builder.Append(DumpStream(queue.Sr, expandUnCreated));
            builder.Append("}]");
            return builder.ToString();
        }

        [TestMethod]
        public void EmptyTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            Assert.IsTrue(RealTimeDeque<string>.IsEmpty(queue));

            queue = RealTimeDeque<string>.Cons("Head", queue);
            Assert.IsFalse(RealTimeDeque<string>.IsEmpty(queue));
            queue = RealTimeDeque<string>.Tail(queue);
            Assert.IsTrue(RealTimeDeque<string>.IsEmpty(queue));

            queue = RealTimeDeque<string>.Snoc(queue, "Tail");
            Assert.IsFalse(RealTimeDeque<string>.IsEmpty(queue));
            queue = RealTimeDeque<string>.Init(queue);
            Assert.IsTrue(RealTimeDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void ConsTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            queue = RealTimeDeque<string>.Cons("Last", queue);
            queue = RealTimeDeque<string>.Cons("Head", queue);

            Assert.AreEqual("[1, {$Head}, {Head}, 1, {$Last}, {Last}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void ConsHeadTailTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));

            foreach (var expected in Data.Split().Reverse())
            {
                var actual = RealTimeDeque<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = RealTimeDeque<string>.Tail(queue);
            }

            Assert.IsTrue(RealTimeDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            queue = RealTimeDeque<string>.Snoc(queue, "Head");
            queue = RealTimeDeque<string>.Snoc(queue, "Last");

            Assert.AreEqual("[1, {$Head}, {Head}, 1, {$Last}, {Last}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void SnocLastInitTest()
        {
            const string Data = "One Two Three One Three";
            var queue = Data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);

            var dat = Data.Split().Reverse();
            foreach (var expected in dat)
            {
                var actual = RealTimeDeque<string>.Last(queue);
                Assert.AreEqual(expected, actual);
                queue = RealTimeDeque<string>.Init(queue);
            }

            Assert.IsTrue(RealTimeDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeDeque<string>.Head(queue));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeDeque<string>.Tail(queue));
        }

        [TestMethod]
        public void EmptyLastTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeDeque<string>.Last(queue));
        }

        [TestMethod]
        public void EmptyInitTest()
        {
            var queue = RealTimeDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => RealTimeDeque<string>.Init(queue));
        }
        
        [TestMethod]
        public void IncrementalTest()
        {
            const string data = "One Two Three Four Five";
            var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
            Assert.AreEqual("[3, {$}, {$}, 2, {$}, {$}]", DumpQueue(queue, false));

            // After looking at the first element, the rest of the queue should be not created.
            var head = RealTimeDeque<string>.Head(queue);
            Assert.AreEqual("One", head);
            Assert.AreEqual("[3, {One, $}, {One, $}, 2, {$}, {$}]", DumpQueue(queue, false));
        }
    }
}