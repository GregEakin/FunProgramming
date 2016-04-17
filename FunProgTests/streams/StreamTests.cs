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
    using System.Linq;

    using FunProgLib.streams;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StreamTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DollarNilTest()
        {
            var s = new Stream<int>.StreamCell(3, null);
        }

        [TestMethod]
        public void ConsTest()
        {
            var s1 = Stream<int>.DollarNil;
            var s2 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(3, s1));
            var s3 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(2, s2));
            var s4 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(1, s3));

            Assert.IsFalse(s4.IsValueCreated);
            Assert.IsNotNull(s4.Value);
            Assert.AreEqual(1, s4.Value.Element);
            Assert.IsFalse(s4.Value.Next.IsValueCreated);
            Assert.IsNotNull(s4.Value.Next.Value);
            Assert.AreEqual(2, s4.Value.Next.Value.Element);
            Assert.IsFalse(s4.Value.Next.Value.Next.IsValueCreated);
            Assert.IsNotNull(s4.Value.Next.Value.Next.Value);
            Assert.AreEqual(3, s4.Value.Next.Value.Next.Value.Element);
            Assert.AreSame(Stream<int>.DollarNil, s4.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void ReverseTest()
        {
            var s1 = Stream<int>.DollarNil;
            var s2 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(3, s1));
            var s3 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(2, s2));
            var s4 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(1, s3));

            var r = Stream<int>.Reverse(s4);

            Assert.IsTrue(s4.IsValueCreated);
            Assert.IsTrue(s4.Value.Next.IsValueCreated);
            Assert.IsTrue(s4.Value.Next.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.AreEqual(3, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(2, r.Value.Next.Value.Element);
            Assert.IsFalse(r.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, r.Value.Next.Value.Next.Value.Element);
            Assert.AreSame(Stream<int>.DollarNil, r.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void AppendTest()
        {
            var s1 = Stream<int>.DollarNil;
            var s2 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(2, s1));
            var s3 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(1, s2));
            var r = Stream<int>.Reverse(s3);
            var t = Stream<int>.Append(s3, r);

            Assert.IsTrue(s3.IsValueCreated);
            Assert.IsTrue(s3.Value.Next.IsValueCreated);

            Assert.IsFalse(r.IsValueCreated);
            Assert.IsFalse(r.Value.Next.IsValueCreated);

            Assert.IsFalse(t.IsValueCreated);
            Assert.AreEqual(1, t.Value.Element);
            Assert.IsFalse(t.Value.Next.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Element);
            Assert.IsTrue(t.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(2, t.Value.Next.Value.Next.Value.Element);
            Assert.IsFalse(t.Value.Next.Value.Next.Value.Next.IsValueCreated);
            Assert.AreEqual(1, t.Value.Next.Value.Next.Value.Next.Value.Element);
            Assert.AreEqual(Stream<int>.DollarNil, t.Value.Next.Value.Next.Value.Next.Value.Next);

            Assert.AreNotSame(s3, t);
            Assert.AreEqual(s3.Value.Element, t.Value.Element);
            Assert.AreSame(r, t.Value.Next.Value.Next);
        }

        [TestMethod]
        public void DropOneTest()
        {
            var s1 = Stream<int>.DollarNil;
            var s2 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(3, s1));
            var s3 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(2, s2));
            var s4 = new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(1, s3));
            var r = Stream<int>.Drop(1, s4);

            Assert.IsTrue(s4.IsValueCreated);
            Assert.IsFalse(s4.Value.Next.IsValueCreated);
            Assert.IsFalse(s4.Value.Next.Value.Next.IsValueCreated);

            Assert.IsTrue(r.IsValueCreated);
            Assert.AreEqual(2, r.Value.Element);
            Assert.IsFalse(r.Value.Next.IsValueCreated);
            Assert.AreEqual(3, r.Value.Next.Value.Element);
            Assert.AreSame(Stream<int>.DollarNil, r.Value.Next.Value.Next);
        }

        [TestMethod]
        public void TakeZeroTest()
        {
            var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));

            var empty = Stream<string>.Take(0, stream);
            Assert.AreSame(Stream<string>.DollarNil, empty);
        }

        [TestMethod]
        public void TakeDollarNullTest()
        {
            var empty = Stream<string>.Take(1, Stream<string>.DollarNil);
            Assert.AreSame(Stream<string>.DollarNil, empty);
        }

        [TestMethod]
        public void TakeTest()
        {
            string[] data = { "A", "B", "C", "D" };

            var stream = Stream<string>.DollarNil;
            foreach (var item in data.Reverse())
            {
                var x = item;
                var s2 = stream;
                stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(x, s2));
            }

            for (var i = 0; i <= data.Length + 1; i++)
            {
                var j = 0;
                var s1 = Stream<string>.Take(i, stream);
                while (s1 != Stream<string>.DollarNil)
                {
                    Assert.AreSame(data[j], s1.Value.Element);
                    s1 = s1.Value.Next;
                    j++;
                }

                var count = Math.Min(i, data.Length);
                Assert.AreEqual(count, j);
            }
        }

        [TestMethod]
        public void DropZeroTest()
        {
            var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));

            var full = Stream<string>.Drop(0, stream);
            Assert.AreSame(stream, full);
        }

        [TestMethod]
        public void DropDollarNullTest()
        {
            var empty = Stream<string>.Drop(1, Stream<string>.DollarNil);
            Assert.AreSame(Stream<string>.DollarNil, empty);
        }

        [TestMethod]
        public void DropTest()
        {
            string[] data = { "A", "B", "C", "D" };

            var stream = Stream<string>.DollarNil;
            foreach (var item in data.Reverse())
            {
                var x = item;
                var s2 = stream;
                stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(x, s2));
            }

            for (var i = 0; i < data.Length; i++)
            {
                var j = 0;
                var s1 = Stream<string>.Drop(i, stream);
                while (s1 != Stream<string>.DollarNil)
                {
                    Assert.AreSame(data[i + j], s1.Value.Element);
                    s1 = s1.Value.Next;
                    j++;
                }

                Assert.AreEqual(data.Length - i, j);
            }

            var empty = Stream<string>.Drop(data.Length, stream);
            Assert.AreSame(Stream<string>.DollarNil, empty);
        }
    }
}
