// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BankersQueueTests.cs
// AUTHOR:		Greg Eakin
namespace FunProgTests.queue
{
    using System.Linq;

    using FunProgLib.queue;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BankersQueueTests
    {
        [Ignore]
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

        [Ignore]
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
        }
    }
}