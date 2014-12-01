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
    using System;

    using FunProgLib.streams;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StreamTests
    {
        [TestMethod]
        public void ConsTest()
        {
            var s = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.Cons(1, new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.Cons(2, new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.Cons(3, null))))));

            Assert.IsFalse(s.IsValueCreated);
            var v1 = s.Value as Stream<int>.Cons;
            Assert.IsNotNull(v1);
            Assert.AreEqual(1, v1.X);
            Assert.IsFalse(v1.S.IsValueCreated);
            var v2 = v1.S.Value as Stream<int>.Cons;
            Assert.IsNotNull(v2);
            Assert.AreEqual(2, v2.X);
            Assert.IsFalse(v2.S.IsValueCreated);
            var v3 = v2.S.Value as Stream<int>.Cons;
            Assert.IsNotNull(v3);
            Assert.AreEqual(3, v3.X);
            Assert.IsNull(v3.S);
        }

        //[TestMethod]
        //public void ReverseTest()
        //{
        //    var s = Stream<int>.Cons(1, Stream<int>.Cons(2, Stream<int>.Cons(3, null)));
        //    var r = Stream<int>.Reverse(s);

        //    Assert.IsFalse(s.IsValueCreated);
        //    Assert.IsFalse(s.Value.Next.IsValueCreated);
        //    Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

        //    Assert.IsFalse(r.IsValueCreated);
        //    Assert.AreEqual(3, r.Value.Element);
        //    Assert.IsFalse(r.Value.Next.IsValueCreated);
        //    Assert.AreEqual(2, r.Value.Next.Value.Element);
        //    Assert.IsFalse(r.Value.Next.Value.Next.IsValueCreated);
        //    Assert.AreEqual(1, r.Value.Next.Value.Next.Value.Element);
        //    Assert.IsNull(r.Value.Next.Value.Next.Value.Next);
        //}

        //[TestMethod]
        //public void CatTest()
        //{
        //    var s = Stream<int>.Cons(1, Stream<int>.Cons(2, null));
        //    var r = Stream<int>.Reverse(s);
        //    var t = Stream<int>.Append(s, r);

        //    // Assert.IsFalse(s.IsValueCreated);
        //    Assert.IsFalse(s.Value.Next.IsValueCreated);

        //    Assert.IsFalse(r.IsValueCreated);
        //    Assert.IsFalse(r.Value.Next.IsValueCreated);

        //    Assert.IsFalse(t.IsValueCreated);
        //    Assert.AreEqual(1, t.Value.Element);
        //    Assert.IsFalse(t.Value.Next.IsValueCreated);
        //    Assert.AreEqual(2, t.Value.Next.Value.Element);
        //    // Assert.IsFalse(t.Value.Next.Value.Next.IsValueCreated);
        //    Assert.AreEqual(2, t.Value.Next.Value.Next.Value.Element);
        //    Assert.IsFalse(t.Value.Next.Value.Next.Value.Next.IsValueCreated);
        //    Assert.AreEqual(1, t.Value.Next.Value.Next.Value.Next.Value.Element);
        //    Assert.IsNull(t.Value.Next.Value.Next.Value.Next.Value.Next);

        //    Assert.AreNotSame(s, t);
        //    Assert.AreEqual(s.Value.Element, t.Value.Element);
        //    Assert.AreSame(r, t.Value.Next.Value.Next);
        //}

        //[TestMethod]
        //public void DropTest()
        //{
        //    var s = Stream<int>.Cons(1, Stream<int>.Cons(2, Stream<int>.Cons(3, null)));
        //    var r = Stream<int>.Drop(1, s);

        //    Assert.IsFalse(s.IsValueCreated);
        //    Assert.IsFalse(s.Value.Next.IsValueCreated);
        //    Assert.IsFalse(s.Value.Next.Value.Next.IsValueCreated);

        //    Assert.IsFalse(r.IsValueCreated);
        //    Assert.AreEqual(2, r.Value.Element);
        //    Assert.IsFalse(r.Value.Next.IsValueCreated);
        //    Assert.AreEqual(3, r.Value.Next.Value.Element);
        //    Assert.IsNull(r.Value.Next.Value.Next);
        //}
    }
}