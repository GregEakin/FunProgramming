// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.lists;
using FunProgLib.queue;
using FunProgLib.Utilities;

namespace FunProgTests.queue;

public class PhysicistQueueTests
{
    private static string DumpLazyList<T>(Lazy<FunList<T>.Node> lazyNode, bool expandUnCreated)
    {
        // TODO: check for lazyNode == null;

        if (!expandUnCreated && !lazyNode.IsValueCreated)
            return "$";

        var result = new StringBuilder();
        if (!lazyNode.IsValueCreated)
            result.Append("$");
        result.Append(lazyNode.Value?.ToReadableString() ?? "null");
        return result.ToString();
    }

    private static string DumpQueue<T>(PhysicistsQueue<T>.Queue queue, bool expandUnCreated)
    {
        if (queue == null) return string.Empty;

        var builder = new StringBuilder();
        builder.Append("[");
        builder.Append(queue.W?.ToReadableString() ?? "null");
        builder.Append(", ");
        builder.Append(queue.Lenf);
        builder.Append(", ");
        builder.Append(DumpLazyList(queue.F, expandUnCreated));
        builder.Append(", ");
        builder.Append(queue.Lenr);
        builder.Append(", ");
        builder.Append(queue.R?.ToReadableString() ?? "null");
        builder.Append("]");
        return builder.ToString();
    }

    [Fact]
    public void Test1()
    {
        // pre-create the null in the empty value, to get the unit tests working
        var empty = PhysicistsQueue<string>.Empty;
        Assert.Null(empty.F.Value);

        var queue = PhysicistsQueue<string>.Empty;
        Assert.Equal("[null, 0, null, 0, null]", DumpQueue(queue, true));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        Assert.Equal("[[One], 3, $[One, Two, Three], 2, [Three, One]]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        Assert.True(PhysicistsQueue<string>.IsEmpty(queue));

        queue = PhysicistsQueue<string>.Snoc(queue, "Item");
        Assert.False(PhysicistsQueue<string>.IsEmpty(queue));

        queue = PhysicistsQueue<string>.Tail(queue);
        Assert.True(PhysicistsQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        var ex = Assert.Throws<NullReferenceException>(() => PhysicistsQueue<string>.Snoc(null, "Item"));
        Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
    }

    [Fact]
    public void SnocTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        queue = PhysicistsQueue<string>.Snoc(queue, "One");
        Assert.Equal("[[One], 1, [One], 0, null]", DumpQueue(queue, false));

        queue = PhysicistsQueue<string>.Snoc(queue, "Two");
        Assert.Equal("[[One], 1, [One], 1, [Two]]", DumpQueue(queue, false));

        queue = PhysicistsQueue<string>.Snoc(queue, "Three");
        Assert.Equal("[[One], 3, $[One, Two, Three], 0, null]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => PhysicistsQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        var head = PhysicistsQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => PhysicistsQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        var tail = PhysicistsQueue<string>.Tail(queue);
        Assert.Equal("[[Two, Three], 2, [Two, Three], 2, [Three, One]]", DumpQueue(tail, true));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var head = PhysicistsQueue<string>.Head(queue);
            Assert.Equal(expected, head);
            queue = PhysicistsQueue<string>.Tail(queue);
        }

        Assert.True(PhysicistsQueue<string>.IsEmpty(queue));
    }
}