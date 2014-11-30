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

    [TestClass]
    public class BankersDequeTests
    {
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
        { }

        [TestMethod]
        public void HeadTest()
        { }

        [TestMethod]
        public void TailTest()
        { }

        [TestMethod]
        public void SnocTest()
        { }

        [TestMethod]
        public void LastTest()
        { }

        [TestMethod]
        public void InitTest()
        { }
    }
}