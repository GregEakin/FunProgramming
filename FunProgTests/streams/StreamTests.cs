// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System.Text;

namespace FunProgTests.streams
{
    using FunProgLib.streams;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using static utilities.ExpectedException;

    [TestClass]
    public class StreamTests
    {
        public static string DumpStream<T>(Lazy<Stream<T>.StreamCell> lazyStream)
        {
            if (lazyStream == Stream<T>.DollarNil) return string.Empty;

            // if (!lazyStream.IsValueCreated)
            //    return "Not Created.";

            var result = new StringBuilder();
            result.Append(lazyStream.Value.Element);
            result.Append(", ");
            result.Append(DumpStream(lazyStream.Value.Next));
            return result.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            const string data = "One Two Three One Three";
            var stream = data.Split().Reverse().Aggregate(Stream<string>.DollarNil, (s1, t) => Stream<string>.DollarCons(t, s1));
            var x = DumpStream<string>(stream);
            Assert.AreEqual("One, Two, Three, One, Three, ", x);
        }

        [TestMethod]
        public void DollarNilTest()
        {
            var ex = AssertThrows<ArgumentException>(() => new Stream<int>.StreamCell(3, null));
            Assert.AreEqual("Can't be null, use Stream<T>.DollarNil instead.\r\n" + "Parameter name: next", ex.Message);
        }

        [TestMethod]
        public void ConsTest()
        {
            var s1 = Stream<int>.DollarNil;
            var s2 = Stream<int>.DollarCons(3, s1);
            var s3 = Stream<int>.DollarCons(2, s2);
            var s4 = Stream<int>.DollarCons(1, s3);

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
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => Stream<int>.DollarCons(t, s1));

            var r = Stream<int>.Reverse(stream);

            Assert.IsTrue(stream.IsValueCreated);
            Assert.IsTrue(stream.Value.Next.IsValueCreated);
            Assert.IsTrue(stream.Value.Next.Value.Next.IsValueCreated);

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
            var data = new[] { 2, 1 };
            var s3 = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => Stream<int>.DollarCons(t1, s1));

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
            Assert.AreSame(Stream<int>.DollarNil, t.Value.Next.Value.Next.Value.Next.Value.Next);

            Assert.AreNotSame(s3, t);
            Assert.AreEqual(s3.Value.Element, t.Value.Element);
            Assert.AreSame(r, t.Value.Next.Value.Next);
        }

        [TestMethod]
        public void DropOneTest()
        {
            var data = new[] { 3, 2, 1 };
            var s4 = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => Stream<int>.DollarCons(t1, s1));
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
            var stream = Stream<string>.DollarCons("X", Stream<string>.DollarNil);

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
            var stream = data.Reverse().Aggregate(Stream<string>.DollarNil, (s1, t1) => Stream<string>.DollarCons(t1, s1));

            for (var i = 0; i <= data.Length + 1; i++)
            {
                var j = 0;
                var s1 = Stream<string>.Take(i, stream);
                while (s1.Value != null)
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
            var stream = Stream<string>.DollarCons("X", Stream<string>.DollarNil);

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
            var stream = data.Reverse().Aggregate(Stream<string>.DollarNil, (s1, t1) => Stream<string>.DollarCons(t1, s1));

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
