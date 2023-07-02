// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.heap;

namespace FunProgTests.heap;

public class SplayHeapTests
{
    private static string DumpHeap<T>(SplayHeap<T>.Heap heap) where T : IComparable<T>
    {
        if (SplayHeap<T>.IsEmpty(heap)) return "\u2205";

        var result = new StringBuilder();
        result.Append("[");

        if (!SplayHeap<T>.IsEmpty(heap.A))
        {
            result.Append(DumpHeap(heap.A));
            result.Append(", ");
        }

        result.Append(heap.X);

        if (!SplayHeap<T>.IsEmpty(heap.B))
        {
            result.Append(", ");
            result.Append(DumpHeap(heap.B));
        }

        result.Append("]");
        return result.ToString();
    }

    [Fact]
    public void EmptyTest()
    {
        var t = SplayHeap<string>.Empty;
        Assert.True(SplayHeap<string>.IsEmpty(t));
        Assert.Equal("∅", DumpHeap(t));

        var t1 = SplayHeap<string>.Insert("C", t);
        Assert.False(SplayHeap<string>.IsEmpty(t1));
    }

    [Fact]
    public void Test1()
    {
        var t = SplayHeap<string>.Empty;
        var x1 = SplayHeap<string>.Insert("C", t);
        var x2 = SplayHeap<string>.Insert("B", x1);
        Assert.Equal("[B, [C]]", DumpHeap(x2));
    }

    [Fact]
    public void Test2()
    {
        const string words = "What's in a name? That which we call a rose by any other name would smell as sweet";
        var ts = words.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));
        Assert.Equal("[[[[[[a], a], any], as, [by, [[call, [in]], name, [name?]]]], other, [[rose], smell]], sweet, [[That, [[we], What's, [which]]], would]]", DumpHeap(ts));
    }

    [Fact]
    public void MergeTest()
    {
        const string data1 = "What's in a name?";
        var ts1 = data1.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));

        const string data2 = "That which we call a rose by any other name would smell as sweet";
        var ts2 = data2.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));

        var t = SplayHeap<string>.Merge(ts1, ts2);
        Assert.Equal("[[[[a], a, [any, [as]]], by, [[call], in, [name]]], name?, [other, [[[[rose], smell], sweet, [That, [we]]], What's, [[which], would]]]]", DumpHeap(t));
    }

    [Fact]
    public void FindMinTest1()
    {
        var t = SplayHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => SplayHeap<int>.FindMin(t));
    }

    [Fact]
    public void FindMinTest2()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var result = SplayHeap<int>.FindMin(t1);
        Assert.Equal(5, result);
    }

    [Fact]
    public void FindMinTest3()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var t2 = SplayHeap<int>.Insert(3, t1);
        var result = SplayHeap<int>.FindMin(t2);
        Assert.Equal(3, result);
    }

    [Fact]
    public void FindMinTest4()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var result = SplayHeap<int>.FindMin(t2);
        Assert.Equal(3, result);
    }

    [Fact]
    public void DeleteMinTest1()
    {
        var t = SplayHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => SplayHeap<int>.DeleteMin(t));
    }

    [Fact]
    public void DeleteMinTest2()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var result = SplayHeap<int>.DeleteMin(t1);
        Assert.Equal("∅", DumpHeap(result));
    }

    [Fact]
    public void DeleteMinTest3()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var t2 = SplayHeap<int>.Insert(3, t1);
        var result = SplayHeap<int>.DeleteMin(t2);
        Assert.Equal("[5]", DumpHeap(result));
    }

    [Fact]
    public void DeleteMinTest4()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var result = SplayHeap<int>.DeleteMin(t2);
        Assert.Equal("[5]", DumpHeap(result));
    }

    [Fact]
    public void DeleteMinTest5()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var t3 = SplayHeap<int>.Insert(6, t2);
        var result = SplayHeap<int>.DeleteMin(t3);
        Assert.Equal("[5, [6]]", DumpHeap(result));
    }

    [Fact]
    public void DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = SplayHeap<int>.Empty;
        for (var i = 0; i < size; i++) heap = SplayHeap<int>.Insert(random.Next(size), heap);

        var last = 0;
        var count = 0;
        while (!SplayHeap<int>.IsEmpty(heap))
        {
            var next = SplayHeap<int>.FindMin(heap);
            heap = SplayHeap<int>.DeleteMin(heap);
            Assert.True(last <= next);
            last = next;
            count++;
        }
        Assert.Equal(size, count);
    }

    [Fact]
    public void Test3()
    {
        var heap = SplayHeap<int>.Empty;
        for (var i = 1; i < 8; i++)
            heap = SplayHeap<int>.Insert(i, heap);
        Assert.Equal("[[[[[[[1], 2], 3], 4], 5], 6], 7]", DumpHeap(heap));

        var x = SplayHeap<int>.Insert(0, heap);
        Assert.Equal("[0, [[[[1], 2, [3]], 4, [5]], 6, [7]]]", DumpHeap(x));

        var y = SplayHeap<int>.DeleteMin(x);
        Assert.Equal("[[[[1], 2, [3]], 4, [5]], 6, [7]]", DumpHeap(y));
    }
}