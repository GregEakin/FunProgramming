// Copyright 2025 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using FunProgLib.streams;

namespace FunProgTests.streams;

public class StreamTests
{
    public static string DumpStream<T>(Lazy<Stream<T>.StreamCell> lazyStream, bool expandUnCreated)
    {
        if (lazyStream == Stream<T>.DollarNil)
            return string.Empty;
        if (!expandUnCreated && !lazyStream.IsValueCreated)
            return "$";

        var result = new StringBuilder();
        if (!lazyStream.IsValueCreated)
            result.Append('$');
        result.Append(lazyStream.Value.Element);
        var rest = DumpStream(lazyStream.Value.Next, expandUnCreated);
        if (string.IsNullOrWhiteSpace(rest))
            return result.ToString();

        result.Append(", ");
        result.Append(rest);
        return result.ToString();
    }

    [Test]
    public async Task Test1()
    {
        const string data = "One Two Three One Three";
        var stream = Enumerable.Reverse(data.Split()).Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
        await Assert.That(DumpStream(stream, true)).IsEqualTo("$One, $Two, $Three, $One, $Three");
    }

    [Test]
    public async Task Test2()
    {
        const string data = "One Two Three One Three";
        var stream = Enumerable.Reverse(data.Split()).Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
        await Assert.That(stream.Value).IsNotNull();
        await Assert.That(stream.Value.Next.Value).IsNotNull();
        await Assert.That(DumpStream(stream, true)).IsEqualTo("One, Two, $Three, $One, $Three");
    }

    [Test]
    public async Task DollarNilTest()
    {
        var ex = await Assert.That(() => new Stream<int>.StreamCell(3, null)).Throws<ArgumentException>();
        await Assert.That(ex.Message).IsEqualTo("Can't be null, use Stream<T>.DollarNil instead. (Parameter 'next')");
    }

    [Test]
    public async Task ConsTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        await Assert.That(DumpStream(stream, true)).IsEqualTo("$1, $2, $3");
    }

    [Test]
    public async Task ReverseTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        var reverse = Stream<int>.Reverse(stream);
        await Assert.That(DumpStream(stream, true)).IsEqualTo("1, 2, 3");
        await Assert.That(DumpStream(reverse, true)).IsEqualTo("$3, $2, $1");
    }

    [Test]
    public async Task AppendTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
        var reverse = Stream<int>.Reverse(stream);
        var sum = Stream<int>.Append(stream, reverse);
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, 2, 3");
        await Assert.That(DumpStream(reverse, false)).IsEqualTo("$");
        await Assert.That(DumpStream(sum, true)).IsEqualTo("$1, $2, $3, $3, $2, $1");
        // the last have of Sum are the same elements in the reverse stream.
        await Assert.That(sum.Value.Next.Value.Next.Value.Next).IsSameReferenceAs(reverse);
    }

    [Test]
    public async Task DropOneTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
        var drop = Stream<int>.Drop(1, stream);
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, $");
        await Assert.That(DumpStream(drop, true)).IsEqualTo("$2, $3");
        // Now that we've displayed everything in the drop steam, 
        // all the items in the first stream are now evaluated.
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, 2, 3");
    }

    [Test]
    public async Task TakeZeroTest()
    {
        var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));
        var empty = Stream<string>.Take(0, stream);
        await Assert.That(empty).IsSameReferenceAs(Stream<string>.DollarNil);
    }

    [Test]
    public async Task TakeDollarNullTest()
    {
        var empty = Stream<string>.Take(1, Stream<string>.DollarNil);
        await Assert.That(empty).IsSameReferenceAs(Stream<string>.DollarNil);
    }

    [Test]
    public async Task TakeTest()
    {
        string[] data =
        {
            "A",
            "B",
            "C",
            "D"
        };
        var stream = Enumerable.Reverse(data).Aggregate(Stream<string>.DollarNil, (s1, t1) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t1, s1)));
        for (var i = 0; i <= data.Length + 1; i++)
        {
            var j = 0;
            var s1 = Stream<string>.Take(i, stream);
            while (s1.Value != null)
            {
                await Assert.That(s1.Value.Element).IsSameReferenceAs(data[j]);
                s1 = s1.Value.Next;
                j++;
            }

            var count = Math.Min(i, data.Length);
            await Assert.That(j).IsEqualTo(count);
        }
    }

    [Test]
    public async Task DropZeroTest()
    {
        var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));
        var full = Stream<string>.Drop(0, stream);
        await Assert.That(full).IsSameReferenceAs(stream);
    }

    [Test]
    public async Task DropDollarNullTest()
    {
        var empty = Stream<string>.Drop(1, Stream<string>.DollarNil);
        await Assert.That(empty).IsSameReferenceAs(Stream<string>.DollarNil);
    }

    [Test]
    public async Task DropTest()
    {
        string[] data =
        {
            "A",
            "B",
            "C",
            "D"
        };
        var stream = Enumerable.Reverse(data).Aggregate(Stream<string>.DollarNil, (s1, t1) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t1, s1)));
        for (var i = 0; i < data.Length; i++)
        {
            var j = 0;
            var s1 = Stream<string>.Drop(i, stream);
            while (s1 != Stream<string>.DollarNil)
            {
                await Assert.That(s1.Value.Element).IsSameReferenceAs(data[i + j]);
                s1 = s1.Value.Next;
                j++;
            }

            await Assert.That(j).IsEqualTo(data.Length - i);
        }

        var empty = Stream<string>.Drop(data.Length, stream);
        await Assert.That(empty).IsSameReferenceAs(Stream<string>.DollarNil);
    }

    [Test]
    public async Task IncrementalConsTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        // Each value has to be fetched, before its expanded
        await Assert.That(DumpStream(stream, false)).IsEqualTo("$");
        await Assert.That(stream.Value).IsNotNull();
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, $");
        await Assert.That(stream.Value.Next.Value).IsNotNull();
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, 2, $");
        await Assert.That(stream.Value.Next.Value.Next.Value).IsNotNull();
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, 2, 3");
    }

    [Test]
    public async Task IncrementalReverseTest()
    {
        var data = new[]
        {
            3,
            2,
            1
        };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        var reverse = Stream<int>.Reverse(stream);
        await Assert.That(DumpStream(stream, false)).IsEqualTo("1, 2, 3"); // the input has to be expanded, to get its reverse
        // Each value has to be fetched, before its expanded
        await Assert.That(DumpStream(reverse, false)).IsEqualTo("$");
        await Assert.That(reverse.Value).IsNotNull();
        await Assert.That(DumpStream(reverse, false)).IsEqualTo("3, $");
        await Assert.That(reverse.Value.Next.Value).IsNotNull();
        await Assert.That(DumpStream(reverse, false)).IsEqualTo("3, 2, $");
        await Assert.That(reverse.Value.Next.Value.Next.Value).IsNotNull();
        await Assert.That(DumpStream(reverse, false)).IsEqualTo("3, 2, 1");
    }
}