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
    public class BankersDequeTests
    {
        private static string DumpQueue<T>(BankersDeque<T>.Queue queue, bool expandUnCreated)
        {
            return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
        }

        [TestMethod]
        public void EmptyTest()
        {
            var queue = BankersDeque<string>.Empty;
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));

            queue = BankersDeque<string>.Cons("Head", queue);
            Assert.IsFalse(BankersDeque<string>.IsEmpty(queue));
            queue = BankersDeque<string>.Tail(queue);
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));

            queue = BankersDeque<string>.Snoc(queue, "Tail");
            Assert.IsFalse(BankersDeque<string>.IsEmpty(queue));
            queue = BankersDeque<string>.Init(queue);
            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void ConsTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Cons("Last", queue);
            queue = BankersDeque<string>.Cons("Head", queue);

            Assert.AreEqual("[1, {$Head}, 1, {$Last}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void ConsHeadTailTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BankersDeque<string>.Empty, (queue1, s) => BankersDeque<string>.Cons(s, queue1));

            foreach (var expected in data.Split().Reverse())
            {
                var actual = BankersDeque<string>.Head(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersDeque<string>.Tail(queue);
            }

            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void SnocTest()
        {
            var queue = BankersDeque<string>.Empty;
            queue = BankersDeque<string>.Snoc(queue, "Head");
            queue = BankersDeque<string>.Snoc(queue, "Last");

            Assert.AreEqual("[1, {$Head}, 1, {$Last}]", DumpQueue(queue, true));
        }

        [TestMethod]
        public void SnocLastInitTest()
        {
            const string data = "One Two Three One Three";
            var queue = data.Split().Aggregate(BankersDeque<string>.Empty, BankersDeque<string>.Snoc);

            var dat = data.Split().Reverse();
            foreach (var expected in dat)
            {
                var actual = BankersDeque<string>.Last(queue);
                Assert.AreEqual(expected, actual);
                queue = BankersDeque<string>.Init(queue);
            }

            Assert.IsTrue(BankersDeque<string>.IsEmpty(queue));
        }

        [TestMethod]
        public void EmptyHeadTest()
        {
            var queue = BankersDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BankersDeque<string>.Head(queue));
        }

        [TestMethod]
        public void EmptyTailTest()
        {
            var queue = BankersDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BankersDeque<string>.Tail(queue));
        }

        [TestMethod]
        public void EmptyLastTest()
        {
            var queue = BankersDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BankersDeque<string>.Last(queue));
        }

        [TestMethod]
        public void EmptyInitTest()
        {
            var queue = BankersDeque<string>.Empty;
            AssertThrows<ArgumentNullException>(() => BankersDeque<string>.Init(queue));
        }
    }
}