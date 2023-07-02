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

public class HoodMelvilleQueueTests
{
    private static string DumpQueue<T>(HoodMelvilleQueue<T>.Queue queue)
    {
        if (queue == null)
            return "null";

        var result = new StringBuilder();
        result.Append("[");
        result.Append(queue.LenF);
        result.Append(", ");
        result.Append(queue.F?.ToReadableString() ?? "null");
        // result.Append(", ");
        // result.Append(queue.State.GetType());
        result.Append(", ");
        result.Append(queue.LenR);
        result.Append(", ");
        result.Append(queue.R?.ToReadableString() ?? "null");
        result.Append("]");
        return result.ToString();
    }

    [Fact]
    public void Test1()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        Assert.Equal("[0, null, 0, null]", DumpQueue(queue));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        Assert.Equal("[3, [One, Two, Three], 2, [Three, One]]", DumpQueue(queue));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        Assert.True(HoodMelvilleQueue<string>.IsEmpty(queue));

        queue = HoodMelvilleQueue<string>.Snoc(queue, "Item");
        Assert.False(HoodMelvilleQueue<string>.IsEmpty(queue));

        queue = HoodMelvilleQueue<string>.Tail(queue);
        Assert.True(HoodMelvilleQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void SnocEmptyTest()
    {
        Assert.Throws<NullReferenceException>(() => HoodMelvilleQueue<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        queue = HoodMelvilleQueue<string>.Snoc(queue, "One");
        Assert.Equal("[1, [One], 0, null]", DumpQueue(queue));

        queue = HoodMelvilleQueue<string>.Snoc(queue, "Two");
        Assert.Equal("[1, [One], 1, [Two]]", DumpQueue(queue));

        queue = HoodMelvilleQueue<string>.Snoc(queue, "Three");
        Assert.Equal("[3, [One], 0, null]", DumpQueue(queue));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => HoodMelvilleQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        var head = HoodMelvilleQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => HoodMelvilleQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        var tail = HoodMelvilleQueue<string>.Tail(queue);
        Assert.Equal("[2, [Two, Three], 2, [Three, One]]", DumpQueue(tail));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var actual = HoodMelvilleQueue<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = HoodMelvilleQueue<string>.Tail(queue);
        }

        Assert.True(HoodMelvilleQueue<string>.IsEmpty(queue));
    }
}