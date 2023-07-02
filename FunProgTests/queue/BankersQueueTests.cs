// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

namespace FunProgTests.queue;

using FunProgLib.queue;
using static streams.StreamTests;
    
public class BankersQueueTests
{
    private static string DumpQueue<T>(BankersQueue<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
    }

    [Fact]
    public void NullTest()
    {
        Assert.Throws<NullReferenceException>(() => BankersQueue<string>.IsEmpty(null));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = BankersQueue<string>.Empty;
        Assert.True(BankersQueue<string>.IsEmpty(queue));

        queue = BankersQueue<string>.Snoc(queue, "Item");
        Assert.False(BankersQueue<string>.IsEmpty(queue));

        queue = BankersQueue<string>.Tail(queue);
        Assert.True(BankersQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void NullSnocTest()
    {
        var ex = Assert.Throws<NullReferenceException>(() => BankersQueue<string>.Snoc(null, "one"));
        Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
    }

    [Fact]
    public void EmptySnocTest()
    {
        var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
        Assert.Equal("[1, {$one}, 0, {}]", DumpQueue(queue, true));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
        queue = BankersQueue<string>.Snoc(queue, "two");
        Assert.Equal("[1, {$one}, 1, {$two}]", DumpQueue(queue, true));
    }

    [Fact]
    public void NullHeadTest()
    {
        var ex = Assert.Throws<NullReferenceException>(() => BankersQueue<string>.Head(null));
        Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = BankersQueue<string>.Empty;
        var ex = Assert.Throws<ArgumentNullException>(() => BankersQueue<string>.Head(queue));
        Assert.Equal("Value cannot be null. (Parameter 'queue')", ex.Message);
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
        var item = BankersQueue<string>.Head(queue);
        Assert.Equal("One", item);
    }

    [Fact]
    public void NullTailTest()
    {
        var ex = Assert.Throws<NullReferenceException>(() => BankersQueue<string>.Tail(null));
        Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = BankersQueue<string>.Empty;
        var ex = Assert.Throws<ArgumentNullException>(() => BankersQueue<string>.Tail(queue));
        Assert.Equal("Value cannot be null. (Parameter 'queue')", ex.Message);
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
        var tail = BankersQueue<string>.Tail(queue);
        Assert.Equal("[2, {$Two, $Three}, 2, {$Three, $One}]", DumpQueue(tail, true));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var actual = BankersQueue<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = BankersQueue<string>.Tail(queue);
        }

        Assert.True(BankersQueue<string>.IsEmpty(queue));
    }
}