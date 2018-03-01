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
    using System;
    using System.Linq;
    using System.Text;
    using static utilities.ExpectedException;

    [TestClass]
    public class StreamTests
    {
        public static string DumpStream<T>(Lazy<Stream<T>.StreamCell> lazyStream, bool expandUnCreated)
        {
            if (lazyStream == Stream<T>.DollarNil) return string.Empty;

            if (!expandUnCreated && !lazyStream.IsValueCreated)                
               return "$";

            var result = new StringBuilder();
            if (!lazyStream.IsValueCreated)
                result.Append("$");
            result.Append(lazyStream.Value.Element);
            var rest = DumpStream(lazyStream.Value.Next, expandUnCreated);
            if (string.IsNullOrWhiteSpace(rest))
                return result.ToString();

            result.Append(", ");
            result.Append(rest);
            return result.ToString();
        }

        [TestMethod]
        public void Test1()
        {
            const string data = "One Two Three One Three";
            var stream = data.Split().Reverse().Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
            Assert.AreEqual("$One, $Two, $Three, $One, $Three", DumpStream(stream, true));
        }

        [TestMethod]
        public void Test2()
        {
            const string data = "One Two Three One Three";
            var stream = data.Split().Reverse().Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
            Assert.IsNotNull(stream.Value);
            Assert.IsNotNull(stream.Value.Next.Value);
            Assert.AreEqual("One, Two, $Three, $One, $Three", DumpStream(stream, true));
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
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));

            Assert.AreEqual("$1, $2, $3", DumpStream(stream, true));
        }

        [TestMethod]
        public void ReverseTest()
        {
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
            var reverse = Stream<int>.Reverse(stream);

            Assert.AreEqual("1, 2, 3", DumpStream(stream, true));
            Assert.AreEqual("$3, $2, $1", DumpStream(reverse, true));
        }

        [TestMethod]
        public void AppendTest()
        {
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
            var reverse = Stream<int>.Reverse(stream);
            var sum = Stream<int>.Append(stream, reverse);

            Assert.AreEqual("1, 2, 3", DumpStream(stream, false));
            Assert.AreEqual("$", DumpStream(reverse, false));
            Assert.AreEqual("$1, $2, $3, $3, $2, $1", DumpStream(sum, true));

            // the last have of Sum are the same elements in the reverse stream.
            Assert.AreSame(reverse, sum.Value.Next.Value.Next.Value.Next);
        }

        [TestMethod]
        public void DropOneTest()
        {
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
            var drop = Stream<int>.Drop(1, stream);

            Assert.AreEqual("1, $", DumpStream(stream, false));
            Assert.AreEqual("$2, $3", DumpStream(drop, true));

            // Now that we've displayed everything in the drop steam, 
            // all the items in the first stream are now evaluated.
            Assert.AreEqual("1, 2, 3", DumpStream(stream, false));
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
            var stream = data.Reverse().Aggregate(Stream<string>.DollarNil, (s1, t1) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t1, s1)));

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
            var stream = data.Reverse().Aggregate(Stream<string>.DollarNil, (s1, t1) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t1, s1)));

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

        [TestMethod]
        public void IncrementalConsTest()
        {
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));

            // Each value has to be fetched, before its expanded
            Assert.AreEqual("$", DumpStream(stream, false));
            Assert.IsNotNull(stream.Value);
            Assert.AreEqual("1, $", DumpStream(stream, false));
            Assert.IsNotNull(stream.Value.Next.Value);
            Assert.AreEqual("1, 2, $", DumpStream(stream, false));
            Assert.IsNotNull(stream.Value.Next.Value.Next.Value);
            Assert.AreEqual("1, 2, 3", DumpStream(stream, false));
        }

        [TestMethod]
        public void IncrementalReverseTest()
        {
            var data = new[] { 3, 2, 1 };
            var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
            var reverse = Stream<int>.Reverse(stream);
            Assert.AreEqual("1, 2, 3", DumpStream(stream, false));  // the input has to be expanded, to get its reverse

            // Each value has to be fetched, before its expanded
            Assert.AreEqual("$", DumpStream(reverse, false));
            Assert.IsNotNull(reverse.Value);
            Assert.AreEqual("3, $", DumpStream(reverse, false));
            Assert.IsNotNull(reverse.Value.Next.Value);
            Assert.AreEqual("3, 2, $", DumpStream(reverse, false));
            Assert.IsNotNull(reverse.Value.Next.Value.Next.Value);
            Assert.AreEqual("3, 2, 1", DumpStream(reverse, false));
        }
    }
}
