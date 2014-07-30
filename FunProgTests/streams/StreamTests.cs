// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		StreamTests.cs
// AUTHOR:		Greg Eakin
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
            var s = Stream<int>.Cons(1, Stream<int>.Cons(2, Stream<int>.Cons(3, null)));

            Assert.IsFalse(s.IsValueCreated);
            Assert.AreEqual(1, s.Value.Element);
            Assert.IsFalse(s.Value.Next.IsValueCreated);
            Assert.AreEqual(2, s.Value.Next.Value.Element);
            Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(3, s.Value.Next.Value.Next.Value.Element);
            Assert.IsNull(s.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void ReverseTest()
        {
            var s = Stream<int>.Cons(1, Stream<int>.Cons(2, Stream<int>.Cons(3, null)));
            var r = Stream<int>.Reverse(s);

            Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.Next.IsValueCreated);
            Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(3, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(2, r.Value.Next.Value.Element);
            Assert.IsFalse(r.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, r.Value.Next.Value.Next.Value.Element);
            Assert.IsNull(r.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void CatTest()
        {
            var s = Stream<int>.Cons(1, Stream<int>.Cons(2, null));
            var r = Stream<int>.Reverse(s);
            var t = Stream<int>.Cat(s, r);

            // Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.IsFalse(r.Value.Next.IsValueCreated);

            Assert.IsFalse(t.IsValueCreated);
            Assert.AreEqual(1, t.Value.Element);
            Assert.IsFalse(t.Value.Next.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Element);
            // Assert.IsFalse(t.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Next.Value.Element);
            Assert.IsFalse(t.Value.Next.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, t.Value.Next.Value.Next.Value.Next.Value.Element);
            Assert.IsNull(t.Value.Next.Value.Next.Value.Next.Value.Next);

            Assert.AreNotSame(s, t);
            Assert.AreEqual(s.Value.Element, t.Value.Element);
            Assert.AreSame(r, t.Value.Next.Value.Next);
        }

        [TestMethod]
        public void DropTest()
        {
            var s = Stream<int>.Cons(1, Stream<int>.Cons(2, Stream<int>.Cons(3, null)));
            var r = Stream<int>.Drop(1, s);

            Assert.IsFalse(s.IsValueCreated);
            Assert.IsFalse(s.Value.Next.IsValueCreated);
            Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(2, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(3, r.Value.Next.Value.Element);
            Assert.IsNull(r.Value.Next.Value.Next);
        }
    }
}