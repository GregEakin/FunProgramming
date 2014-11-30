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
    public class RealTimeDequeTests
    {
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