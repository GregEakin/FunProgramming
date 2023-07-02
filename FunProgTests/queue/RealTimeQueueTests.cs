// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.queue;
using FunProgLib.Utilities;
using FunProgTests.streams;

namespace FunProgTests.queue;

public class RealTimeQueueTests
{
    public static string DumpQueue<T>(RealTimeQueue<T>.Queue queue, bool expandUnCreated)
    {
        if (RealTimeQueue<T>.IsEmpty(queue)) return string.Empty;

        var result = new StringBuilder();
        result.Append("[{");
        result.Append(StreamTests.DumpStream(queue.F, expandUnCreated));
        result.Append("}, ");
        result.Append(queue.R?.ToReadableString() ?? "null");
        result.Append(", {");
        result.Append(StreamTests.DumpStream(queue.S, expandUnCreated));
        result.Append("}]");
        return result.ToString();
    }

    [Fact]
    public void Test1()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Equal("", DumpQueue(queue, true));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        Assert.Equal("[{One, Two, $Three}, [Three, One], {Three}]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.True(RealTimeQueue<string>.IsEmpty(queue));

        queue = RealTimeQueue<string>.Snoc(queue, "Item");
        Assert.False(RealTimeQueue<string>.IsEmpty(queue));

        queue = RealTimeQueue<string>.Tail(queue);
        Assert.True(RealTimeQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        Assert.Throws<NullReferenceException>(() => RealTimeQueue<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        queue = RealTimeQueue<string>.Snoc(queue, "One");
        Assert.Equal("[{$}, null, {$}]", DumpQueue(queue, false));

        queue = RealTimeQueue<string>.Snoc(queue, "Two");
        Assert.Equal("[{One}, [Two], {}]", DumpQueue(queue, false));

        queue = RealTimeQueue<string>.Snoc(queue, "Three");
        Assert.Equal("[{$One, $Two, $Three}, null, {One, Two, Three}]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var head = RealTimeQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var tail = RealTimeQueue<string>.Tail(queue);
        Assert.Equal("[{Two, Three}, [Three, One], {}]", DumpQueue(tail, true));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var head = RealTimeQueue<string>.Head(queue);
            Assert.Equal(expected, head);
            queue = RealTimeQueue<string>.Tail(queue);
        }

        Assert.True(RealTimeQueue<string>.IsEmpty(queue));
    }

    private const int Size = 16;

    [Fact]
    public void PerfTest()
    {
        var heap = RealTimeQueue<int>.Empty;
        for (var i = 0; i < Size; i++)
        {
            heap = RealTimeQueue<int>.Snoc(heap, i);
        }

        Console.WriteLine(DumpQueue(heap, true));

        var count = 0;
        while (!RealTimeQueue<int>.IsEmpty(heap))
        {
            var next = RealTimeQueue<int>.Head(heap);
            Assert.Equal(count, next);
            heap = RealTimeQueue<int>.Tail(heap);
            count++;
        }

        Assert.Equal(Size, count);
    }
}