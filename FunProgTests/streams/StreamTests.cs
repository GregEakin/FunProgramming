// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.streams;

namespace FunProgTests.streams;

public class StreamTests
{
    public static string DumpStream<T>(Lazy<Stream<T>.StreamCell> lazyStream, bool expandUnCreated)
    {
        if (lazyStream == Stream<T>.DollarNil) return string.Empty;

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

    [Fact]
    public void Test1()
    {
        const string data = "One Two Three One Three";
        var stream = data.Split().Reverse().Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
        Assert.Equal("$One, $Two, $Three, $One, $Three", DumpStream(stream, true));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var stream = data.Split().Reverse().Aggregate(Stream<string>.DollarNil, (s1, t) => new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell(t, s1)));
        Assert.NotNull(stream.Value);
        Assert.NotNull(stream.Value.Next.Value);
        Assert.Equal("One, Two, $Three, $One, $Three", DumpStream(stream, true));
    }
        
    [Fact]
    public void DollarNilTest()
    {
        var ex = Assert.Throws<ArgumentException>(() => new Stream<int>.StreamCell(3, null));
        Assert.Equal("Can't be null, use Stream<T>.DollarNil instead. (Parameter 'next')", ex.Message);
    }

    [Fact]
    public void ConsTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));

        Assert.Equal("$1, $2, $3", DumpStream(stream, true));
    }

    [Fact]
    public void ReverseTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        var reverse = Stream<int>.Reverse(stream);

        Assert.Equal("1, 2, 3", DumpStream(stream, true));
        Assert.Equal("$3, $2, $1", DumpStream(reverse, true));
    }

    [Fact]
    public void AppendTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
        var reverse = Stream<int>.Reverse(stream);
        var sum = Stream<int>.Append(stream, reverse);

        Assert.Equal("1, 2, 3", DumpStream(stream, false));
        Assert.Equal("$", DumpStream(reverse, false));
        Assert.Equal("$1, $2, $3, $3, $2, $1", DumpStream(sum, true));

        // the last have of Sum are the same elements in the reverse stream.
        Assert.Same(reverse, sum.Value.Next.Value.Next.Value.Next);
    }

    [Fact]
    public void DropOneTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t1) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t1, s1)));
        var drop = Stream<int>.Drop(1, stream);

        Assert.Equal("1, $", DumpStream(stream, false));
        Assert.Equal("$2, $3", DumpStream(drop, true));

        // Now that we've displayed everything in the drop steam, 
        // all the items in the first stream are now evaluated.
        Assert.Equal("1, 2, 3", DumpStream(stream, false));
    }

    [Fact]
    public void TakeZeroTest()
    {
        var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));

        var empty = Stream<string>.Take(0, stream);
        Assert.Same(Stream<string>.DollarNil, empty);
    }

    [Fact]
    public void TakeDollarNullTest()
    {
        var empty = Stream<string>.Take(1, Stream<string>.DollarNil);
        Assert.Same(Stream<string>.DollarNil, empty);
    }

    [Fact]
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
                Assert.Same(data[j], s1.Value.Element);
                s1 = s1.Value.Next;
                j++;
            }

            var count = Math.Min(i, data.Length);
            Assert.Equal(count, j);
        }
    }

    [Fact]
    public void DropZeroTest()
    {
        var stream = new Lazy<Stream<string>.StreamCell>(() => new Stream<string>.StreamCell("X", Stream<string>.DollarNil));

        var full = Stream<string>.Drop(0, stream);
        Assert.Same(stream, full);
    }

    [Fact]
    public void DropDollarNullTest()
    {
        var empty = Stream<string>.Drop(1, Stream<string>.DollarNil);
        Assert.Same(Stream<string>.DollarNil, empty);
    }

    [Fact]
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
                Assert.Same(data[i + j], s1.Value.Element);
                s1 = s1.Value.Next;
                j++;
            }

            Assert.Equal(data.Length - i, j);
        }

        var empty = Stream<string>.Drop(data.Length, stream);
        Assert.Same(Stream<string>.DollarNil, empty);
    }

    [Fact]
    public void IncrementalConsTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));

        // Each value has to be fetched, before its expanded
        Assert.Equal("$", DumpStream(stream, false));
        Assert.NotNull(stream.Value);
        Assert.Equal("1, $", DumpStream(stream, false));
        Assert.NotNull(stream.Value.Next.Value);
        Assert.Equal("1, 2, $", DumpStream(stream, false));
        Assert.NotNull(stream.Value.Next.Value.Next.Value);
        Assert.Equal("1, 2, 3", DumpStream(stream, false));
    }

    [Fact]
    public void IncrementalReverseTest()
    {
        var data = new[] { 3, 2, 1 };
        var stream = data.Aggregate(Stream<int>.DollarNil, (s1, t) => new Lazy<Stream<int>.StreamCell>(() => new Stream<int>.StreamCell(t, s1)));
        var reverse = Stream<int>.Reverse(stream);
        Assert.Equal("1, 2, 3", DumpStream(stream, false));  // the input has to be expanded, to get its reverse

        // Each value has to be fetched, before its expanded
        Assert.Equal("$", DumpStream(reverse, false));
        Assert.NotNull(reverse.Value);
        Assert.Equal("3, $", DumpStream(reverse, false));
        Assert.NotNull(reverse.Value.Next.Value);
        Assert.Equal("3, 2, $", DumpStream(reverse, false));
        Assert.NotNull(reverse.Value.Next.Value.Next.Value);
        Assert.Equal("3, 2, 1", DumpStream(reverse, false));
    }
}