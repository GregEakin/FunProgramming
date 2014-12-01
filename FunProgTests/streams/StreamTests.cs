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
            Assert.AreEqual(1, s.Value.X);
            Assert.IsFalse(s.Value.S.IsValueCreated);
            Assert.IsNotNull(s.Value.S.Value);
            Assert.AreEqual(2, s.Value.S.Value.X);
            Assert.IsFalse(s.Value.S.Value.S.IsValueCreated);
            Assert.IsNotNull(s.Value.S.Value.S.Value);
            Assert.AreEqual(3, s.Value.S.Value.S.Value.X);
            Assert.IsNull(s.Value.S.Value.S.Value.S);
        }

        [TestMethod]
        public void ReverseTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, Stream<int>.DollarCons(3, null)));
            var r = Stream<int>.Reverse(s);

            Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.S.IsValueCreated);
            Assert.IsFalse(s.Value.S.Value.S.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(3, r.Value.X);
            Assert.IsFalse(r.Value.S.IsValueCreated);
            Assert.AreEqual(2, r.Value.S.Value.X);
            Assert.IsFalse(r.Value.S.Value.S.IsValueCreated);
            Assert.AreEqual(1, r.Value.S.Value.S.Value.X);
            Assert.IsNull(r.Value.S.Value.S.Value.S);
        }

        [TestMethod]
        public void CatTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, null));
            var r = Stream<int>.Reverse(s);
            var t = Stream<int>.Append(s, r);

            // Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.S.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.IsFalse(r.Value.S.IsValueCreated);

            Assert.IsFalse(t.IsValueCreated);
            Assert.AreEqual(1, t.Value.X);
            Assert.IsFalse(t.Value.S.IsValueCreated);
            Assert.AreEqual(2, t.Value.S.Value.X);
            // Assert.IsFalse(t.Value.S.Value.S.IsValueCreated);
            Assert.AreEqual(2, t.Value.S.Value.S.Value.X);
            Assert.IsFalse(t.Value.S.Value.S.Value.S.IsValueCreated);
            Assert.AreEqual(1, t.Value.S.Value.S.Value.S.Value.X);
            Assert.IsNull(t.Value.S.Value.S.Value.S.Value.S);

            Assert.AreNotSame(s, t);
            Assert.AreEqual(s.Value.X, t.Value.X);
            Assert.AreSame(r, t.Value.S.Value.S);
        }

        [TestMethod]
        public void DropTest()
        {
            var s = Stream<int>.DollarCons(1, Stream<int>.DollarCons(2, Stream<int>.DollarCons(3, null)));
            var r = Stream<int>.Drop(1, s);

            Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.S.IsValueCreated);
            Assert.IsFalse(s.Value.S.Value.S.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(2, r.Value.X);
            Assert.IsFalse(r.Value.S.IsValueCreated);
            Assert.AreEqual(3, r.Value.S.Value.X);
            Assert.IsNull(r.Value.S.Value.S);
        }
    }
}