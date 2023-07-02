// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.queue;

namespace FunProgTests.queue;

using static streams.StreamTests;

public class RealTimeDequeTests
{
    private static string DumpQueue<T>(RealTimeDeque<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {{{DumpStream(queue.Sf, expandUnCreated)}}}, "
               + $"{queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}, {{{DumpStream(queue.Sr, expandUnCreated)}}}]";
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        Assert.True(RealTimeDeque<string>.IsEmpty(queue));

        queue = RealTimeDeque<string>.Cons("Head", queue);
        Assert.False(RealTimeDeque<string>.IsEmpty(queue));
        queue = RealTimeDeque<string>.Tail(queue);
        Assert.True(RealTimeDeque<string>.IsEmpty(queue));

        queue = RealTimeDeque<string>.Snoc(queue, "Tail");
        Assert.False(RealTimeDeque<string>.IsEmpty(queue));
        queue = RealTimeDeque<string>.Init(queue);
        Assert.True(RealTimeDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptyConsTest()
    {
        Assert.Throws<NullReferenceException>(() => RealTimeDeque<string>.Cons("Item", null));
    }

    [Fact]
    public void ConsTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        queue = RealTimeDeque<string>.Cons("Last", queue);
        Assert.Equal("[1, {$}, {}, 0, {}, {}]", DumpQueue(queue, false));

        queue = RealTimeDeque<string>.Cons("Head", queue);
        Assert.Equal("[1, {$Head}, {Head}, 1, {$Last}, {Last}]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeDeque<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        var head = RealTimeDeque<string>.Head(queue);
        Assert.Equal("Three", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeDeque<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        var tail = RealTimeDeque<string>.Tail(queue);
        Assert.Equal("[1, {One}, {}, 3, {One, Two, $Three}, {Three}]", DumpQueue(tail, true));
    }

    [Fact]
    public void ConsHeadTailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));

        foreach (var expected in data.Split().Reverse())
        {
            var actual = RealTimeDeque<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = RealTimeDeque<string>.Tail(queue);
        }

        Assert.True(RealTimeDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void IncrementalHeadTest()
    {
        const string data = "One Two Three Four Five";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        Assert.Equal("[2, {$}, {$}, 3, {$}, {$}]", DumpQueue(queue, false));

        // After looking at the first element, the rest of the queue should be not created.
        var head = RealTimeDeque<string>.Head(queue);
        Assert.Equal("Five", head);
        Assert.Equal("[2, {Five, $}, {Five, $}, 3, {$}, {$}]", DumpQueue(queue, false));
    }

    [Fact]
    public void EmptySoncTest()
    {
        Assert.Throws<NullReferenceException>(() => RealTimeDeque<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        queue = RealTimeDeque<string>.Snoc(queue, "Head");
        Assert.Equal("[0, {}, {}, 1, {$}, {}]", DumpQueue(queue, false));

        queue = RealTimeDeque<string>.Snoc(queue, "Last");
        Assert.Equal("[1, {$Head}, {Head}, 1, {$Last}, {Last}]", DumpQueue(queue, true));
    }

    [Fact]
    public void LastTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        var last = RealTimeDeque<string>.Last(queue);
        Assert.Equal("Three", last);
    }

    [Fact]
    public void EmptyLastTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeDeque<string>.Last(queue));
    }

    [Fact]
    public void InitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        var init = RealTimeDeque<string>.Init(queue);
        Assert.Equal("[3, {One, Two, $Three}, {Three}, 1, {One}, {}]", DumpQueue(init, true));
    }

    [Fact]
    public void EmptyInitTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeDeque<string>.Init(queue));
    }

    [Fact]
    public void SnocLastInitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);

        var dat = data.Split().Reverse();
        foreach (var expected in dat)
        {
            var actual = RealTimeDeque<string>.Last(queue);
            Assert.Equal(expected, actual);
            queue = RealTimeDeque<string>.Init(queue);
        }

        Assert.True(RealTimeDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void IncrementalLastTest()
    {
        const string data = "One Two Three Four Five";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        Assert.Equal("[3, {$}, {$}, 2, {$}, {$}]", DumpQueue(queue, false));

        // After looking at the last element, the rest of the queue should be not created.
        var last = RealTimeDeque<string>.Last(queue);
        Assert.Equal("Five", last);
        Assert.Equal("[3, {$}, {$}, 2, {Five, $}, {Five, $}]", DumpQueue(queue, false));
    }
}