// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.lists;

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
    public class PhysicistQueueTests
    {
        private static string DumpLazyList<T>(Lazy<List<T>.Node> lazyNode, bool expandUnCreated)
        {
            // TODO: check for lazyNode == null;

            if (!expandUnCreated && !lazyNode.IsValueCreated)
                return "$";

            var result = new StringBuilder();
            if (!lazyNode.IsValueCreated)
                result.Append("$");
            result.Append(lazyNode.Value != null ? lazyNode.Value.ToReadableString() : "null");
            return result.ToString();
        }

        private static string DumpQueue<T>(PhysicistsQueue<T>.Queue queue, bool expandUnCreated)
        {
            if (queue == null) return string.Empty;

            var builder = new StringBuilder();
            builder.Append("[");
            builder.Append(queue.W != null ? queue.W.ToReadableString() : "null");
            builder.Append(", ");
            builder.Append(queue.Lenf);
            builder.Append(", ");
            builder.Append(DumpLazyList(queue.F, expandUnCreated));
            builder.Append(", ");
            builder.Append(queue.Lenr);
            builder.Append(", ");
            builder.Append(queue.R != null ? queue.R.ToReadableString() : "null");
            builder.Append("]");
            return builder.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            // pre-create the null in the empty value, to get the unit tests working
            var empty = PhysicistsQueue<string>.Empty;
            Assert.IsNull(empty.F.Value);

            var queue = PhysicistsQueue<string>.Empty;
            Assert.AreEqual("[null, 0, null, 0, null]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void Test2()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
            Assert.AreEqual("[[One], 3, $[One, Two, Three], 2, [Three, One]]", DumpQueue(queue, true));
        }

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
        public void EmptySnocTest()
        {
            var ex = AssertThrows<NullReferenceException>(() => PhysicistsQueue<string>.Snoc(null, "Item"));
            Assert.AreEqual("Object reference not set to an instance of an object.", ex.Message);
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            queue = PhysicistsQueue<string>.Snoc(queue, "One");
            Assert.AreEqual("[[One], 1, [One], 0, null]", DumpQueue(queue, false));

            queue = PhysicistsQueue<string>.Snoc(queue, "Two");
            Assert.AreEqual("[[One], 1, [One], 1, [Two]]", DumpQueue(queue, false));

            queue = PhysicistsQueue<string>.Snoc(queue, "Three");
            Assert.AreEqual("[[One], 3, $[One, Two, Three], 0, null]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => PhysicistsQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
            var head = PhysicistsQueue<string>.Head(queue);
            Assert.AreEqual("One", head);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = PhysicistsQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => PhysicistsQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
            var tail = PhysicistsQueue<string>.Tail(queue);
            Assert.AreEqual("[[Two, Three], 2, [Two, Three], 2, [Three, One]]", DumpQueue(tail, true));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var head = PhysicistsQueue<string>.Head(queue);
                Assert.AreEqual(expected, head);
                queue = PhysicistsQueue<string>.Tail(queue);
            }

            Assert.IsTrue(PhysicistsQueue<string>.IsEmpty(queue));
        }
    }
}