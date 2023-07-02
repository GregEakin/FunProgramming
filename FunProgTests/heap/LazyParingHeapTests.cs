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

public class LazyParingHeapTests
{
    private static string DumpHeap<T>(LazyParingHeap<T>.Heap node, bool showSusp) where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append("[");
        result.Append(node.Root);
        if (!LazyParingHeap<T>.IsEmpty(node.FunList))
        {
            result.Append(", ");
            result.Append(DumpHeap(node.FunList, showSusp));
        }
        if (showSusp || node.LazyList.IsValueCreated)
        {
            if (!LazyParingHeap<T>.IsEmpty(node.LazyList.Value))
            {
                result.Append("; ");
                result.Append(DumpHeap(node.LazyList.Value, showSusp));
            }
        }
        else
        {
            result.Append("; susp");
        }
        result.Append("]");
        return result.ToString();
    }

    [Fact]
    public void EmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        Assert.True(LazyParingHeap<int>.IsEmpty(empty));

        var heap = LazyParingHeap<int>.Insert(3, empty);
        Assert.False(LazyParingHeap<int>.IsEmpty(heap));
    }

    [Fact]
    public void MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var empty = LazyParingHeap<int>.Empty;
        var heap = LazyParingHeap<int>.Merge(heap1, empty);
        Assert.Same(heap1, heap);
    }

    [Fact]
    public void MergeTest2()
    {
        var empty = LazyParingHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(empty, heap2);
        Assert.Same(heap2, heap);
    }

    [Fact]
    public void MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[0; [1; [2, [3, [10; [11, [12]]]]]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[0, [10, [13]; [11, [12]]]; [1, [2]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[0; [1; [2, [3, [10, [13]; [11, [12]]]]]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void InsertTest1()
    {
        var empty = LazyParingHeap<int>.Empty;
        var heap = LazyParingHeap<int>.Insert(0, empty);
        Assert.Equal("[0]", DumpHeap(heap, true));
    }

    [Fact]
    public void InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Insert(2, heap1);
        Assert.Equal("[0; [1, [2]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Insert(3, heap1);
        Assert.Equal("[0, [3]; [1, [2]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void FindMinEmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => LazyParingHeap<int>.FindMin(empty));
    }

    [Fact]
    public void FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var min = LazyParingHeap<int>.FindMin(heap);
        Assert.Equal(0, min);
    }

    [Fact]
    public void DeleteMinEmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => LazyParingHeap<int>.DeleteMin(empty));
    }

    [Fact]
    public void DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        heap = LazyParingHeap<int>.DeleteMin(heap);
        Assert.Equal("[1; [2; [3; [4, [5; [6, [7]]]]]]]", DumpHeap(heap, true));
    }

    [Fact]
    public void DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = LazyParingHeap<int>.Empty;
        for (var i = 0; i < size; i++) heap = LazyParingHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!LazyParingHeap<int>.IsEmpty(heap))
        {
            var next = LazyParingHeap<int>.FindMin(heap);
            heap = LazyParingHeap<int>.DeleteMin(heap);
            Assert.True(last <= next);
            last = next;
            count++;
        }
        Assert.Equal(size, count);
    }
}