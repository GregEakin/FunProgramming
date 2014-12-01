// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.streams
{
    using FunProgLib.streams;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StreamTests
    {
        [TestMethod]
        public void ConsTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, Stream<int>.DollarCons(3, null)));

            Assert.IsFalse(s.IsValueCreated);
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(1, s.Value.Element);
            Assert.IsFalse(s.Value.Next.IsValueCreated);
            Assert.IsNotNull(s.Value.Next.Value);
            Assert.AreEqual(2, s.Value.Next.Value.Element);
            Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);
            Assert.IsNotNull(s.Value.Next.Value.Next.Value);
            Assert.AreEqual(3, s.Value.Next.Value.Next.Value.Element);
            Assert.IsNull(s.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void ReverseTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, Stream<int>.DollarCons(3, null)));
            var r = Stream<int>.Reverse(s);

            //Assert.IsFalse(s.IsValueCreated);
            //Assert.IsFalse(s.Value.Next.IsValueCreated);
            //Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(3, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(2, r.Value.Next.Value.Element);
            Assert.IsFalse(r.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, r.Value.Next.Value.Next.Value.Element);
            Assert.AreEqual(Stream<int>.DollarNil, r.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void CatTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, null));
            var r = Stream<int>.Reverse(s);
            var t = Stream<int>.Append(s, r);

            // Assert.IsFalse(s.IsValueCreated);
            // Assert.IsFalse(s.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.IsFalse(r.Value.Next.IsValueCreated);

            Assert.IsFalse(t.IsValueCreated);
            Assert.AreEqual(1, t.Value.Element);
            Assert.IsFalse(t.Value.Next.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Element);
            // Assert.IsFalse(t.Value.S.Value.S.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Next.Value.Element);
            Assert.IsFalse(t.Value.Next.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, t.Value.Next.Value.Next.Value.Next.Value.Element);
            Assert.AreEqual(Stream<int>.DollarNil, t.Value.Next.Value.Next.Value.Next.Value.Next);

            Assert.AreNotSame(s, t);
            Assert.AreEqual(s.Value.Element, t.Value.Element);
            Assert.AreSame(r, t.Value.Next.Value.Next);
        }

        [TestMethod]
        public void DropTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, Stream<int>.DollarCons(3, null)));
            var r = Stream<int>.Drop(1, s);

            // Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.Next.IsValueCreated);
            Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

            //Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(2, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(3, r.Value.Next.Value.Element);
            Assert.IsNull(r.Value.Next.Value.Next);
        }
    }
}