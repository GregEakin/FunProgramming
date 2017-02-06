// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Text;
using FunProgLib.Utilities;

namespace FunProgTests.queue
{
    using FunProgLib.queue;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using static utilities.ExpectedException;

    [TestClass]
    public class HoodMelvilleQueueTests
    {
        private static string DumpQueue<T>(HoodMelvilleQueue<T>.Queue queue)
        {
            if (queue == null)
                return "null";

            var result = new StringBuilder();
            result.Append("[");
            result.Append(queue.LenF);
            result.Append(", ");
            result.Append(queue.F != null ? queue.F.ToReadableString() : "null");
            // result.Append(", ");
            // result.Append(queue.State.GetType());
            result.Append(", ");
            result.Append(queue.LenR);
            result.Append(", ");
            result.Append(queue.R != null ? queue.R.ToReadableString() : "null");
            result.Append("]");
            return result.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            Assert.AreEqual("[0, null, 0, null]", DumpQueue(queue));
        }

        [TestMethod]
        public void Test2()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
            Assert.AreEqual("[3, [One, Two, Three], 2, [Three, One]]", DumpQueue(queue));
        }

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
        public void SnocEmptyTest()
        {
            AssertThrows<NullReferenceException>(() => HoodMelvilleQueue<string>.Snoc(null, "Item"));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            queue = HoodMelvilleQueue<string>.Snoc(queue, "One");
            Assert.AreEqual("[1, [One], 0, null]", DumpQueue(queue));

            queue = HoodMelvilleQueue<string>.Snoc(queue, "Two");
            Assert.AreEqual("[1, [One], 1, [Two]]", DumpQueue(queue));

            queue = HoodMelvilleQueue<string>.Snoc(queue, "Three");
            Assert.AreEqual("[3, [One], 0, null]", DumpQueue(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => HoodMelvilleQueue<string>.Head(queue));
        }

        [TestMethod]
        public void HeadTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
            var head = HoodMelvilleQueue<string>.Head(queue);
            Assert.AreEqual("One", head);
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = HoodMelvilleQueue<string>.Empty;
            AssertThrows<ArgumentNullException>(() => HoodMelvilleQueue<string>.Tail(queue));
        }

        [TestMethod]
        public void TailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
            var tail = HoodMelvilleQueue<string>.Tail(queue);
            Assert.AreEqual("[2, [Two, Three], 2, [Three, One]]", DumpQueue(tail));
        }

        [TestMethod]
        public void PushPopTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);

            foreach (var expected in data.Split())
            {
                var actual = HoodMelvilleQueue<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = HoodMelvilleQueue<string>.Tail(queue);
            }

            Assert.IsTrue(HoodMelvilleQueue<string>.IsEmpty(queue));
        }
    }
}
