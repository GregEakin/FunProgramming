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

namespace FunProgTests.queue;
    
public class BatchedQueueTests
{
    private static string DumpQueue<T>(BatchedQueue<T>.Queue queue)
    {
        var builder = new StringBuilder();
        builder.Append("[");
        builder.Append(queue.F?.ToReadableString() ?? "null");
        builder.Append(", ");
        builder.Append(queue.R?.ToReadableString() ?? "null");
        builder.Append("]");
        return builder.ToString();
    }

    [Fact]
    public void Test1()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Equal("[null, null]", DumpQueue(queue));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        Assert.Equal("[[One], [Three, One, Three, Two]]", DumpQueue(queue));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.True(BatchedQueue<string>.IsEmpty(queue));

        queue = BatchedQueue<string>.Snoc(queue, "Item");
        Assert.False(BatchedQueue<string>.IsEmpty(queue));

        queue = BatchedQueue<string>.Tail(queue);
        Assert.True(BatchedQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        Assert.Throws<NullReferenceException>(() => BatchedQueue<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {

    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BatchedQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var head = BatchedQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BatchedQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var tail = BatchedQueue<string>.Tail(queue);
        Assert.Equal("[[Two, Three, One, Three], null]", DumpQueue(tail));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var actual = BatchedQueue<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = BatchedQueue<string>.Tail(queue);
        }

        Assert.True(BatchedQueue<string>.IsEmpty(queue));
    }
}