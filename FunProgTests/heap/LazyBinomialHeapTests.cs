// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgTests.utilities;
using FunProgLib.heap;
using FunProgLib.lists;

namespace FunProgTests.heap;

public class LazyBinomialHeapTests
{
    public static string DumpNode<T>(LazyBinomialHeap<T>.Tree tree) where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append("[");
        result.Append(tree.Root);
        if (!FunList<LazyBinomialHeap<T>.Tree>.IsEmpty(tree.FunList))
        {
            foreach (var node in tree.FunList)
            {
                result.Append(", ");
                result.Append(DumpNode(node));
            }
        }
        result.Append("]");
        return result.ToString();
    }

    public static string DumpHeap<T>(Lazy<FunList<LazyBinomialHeap<T>.Tree>.Node> list, bool expandUnCreated) where T : IComparable<T>
    {
        if (!list.IsValueCreated && !expandUnCreated)
            return "$";

        var result = new StringBuilder();
        if (!list.IsValueCreated)
            result.Append("$");
        foreach (var node in list.Value)
        {
            result.Append(DumpNode(node));
            result.Append("; ");
        }
        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    [Fact]
    public void BinomialTest1()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < 16; i++)
        {
            heap = LazyBinomialHeap<int>.Insert(i, heap);
            var dumpHeap = DumpHeap(heap, true);
            // Console.WriteLine(dumpHeap, true);

            var semicolons = Counters.CountChar(dumpHeap, ';');
            Assert.Equal(Counters.CountBinaryOnes(i + 1), semicolons);
        }
    }

    [Fact]
    public void BinomialTest2()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < 0x100; i++)
        {
            heap = LazyBinomialHeap<int>.Insert(1, heap);
            var dumpHeap = DumpHeap(heap, true);
            //Console.WriteLine(dumpHeap, true);
            var blocks = dumpHeap.Split(';');

            var j = 0;
            var p = 0;
            for (var k = i + 1; k > 0; k >>= 1, j++)
            {
                if (k % 2 == 0) continue;
                var q = (int)Math.Pow(2, j);
                var block = blocks[p++];
                Assert.Equal(q, Counters.CountChar(block, '1') - 1);
            }
        }
    }

    [Fact]
    public void MonolithicTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var x1 = LazyBinomialHeap<int>.Insert(3, empty);
        var x2 = LazyBinomialHeap<int>.Insert(2, x1);
        Assert.False(x1.IsValueCreated);
        Assert.False(x2.IsValueCreated);

        // Once we look at one element, the entire list will be forced created.
        Assert.NotNull(x2.Value);
        Assert.True(x1.IsValueCreated);
        Assert.True(x2.IsValueCreated);
    }

    [Fact]
    public void EmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        Assert.True(LazyBinomialHeap<int>.IsEmpty(empty));

        var heap = LazyBinomialHeap<int>.Insert(1, empty);
        Assert.False(LazyBinomialHeap<int>.IsEmpty(heap));
    }

    [Fact]
    public void InsertTest1()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var heap = LazyBinomialHeap<int>.Insert(0, empty);
        Assert.Equal("$[0]", DumpHeap(heap, true));
    }

    [Fact]
    public void InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Insert(2, heap1);
        Assert.Equal("$[2]; [0, [1]]", DumpHeap(heap, true));
    }

    [Fact]
    public void InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Insert(3, heap1);
        Assert.Equal("$[0, [2, [3]], [1]]", DumpHeap(heap, true));
    }

    [Fact]
    public void MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var empty = LazyBinomialHeap<int>.Empty;
        var heap = LazyBinomialHeap<int>.Merge(heap1, empty);
        Assert.Same(heap1.Value, heap.Value);
    }

    [Fact]
    public void MergeTest2()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(empty, heap2);
        Assert.Same(heap2.Value, heap.Value);
    }

    [Fact]
    public void MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("$[12]; [10, [11]]; [0, [2, [3]], [1]]", DumpHeap(heap, true));
    }

    [Fact]
    public void MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("$[2]; [0, [1]]; [10, [12, [13]], [11]]", DumpHeap(heap, true));
    }

    [Fact]
    public void MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("$[0, [10, [12, [13]], [11]], [2, [3]], [1]]", DumpHeap(heap, true));
    }

    [Fact]
    public void FindMinEmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => LazyBinomialHeap<int>.FindMin(empty));
    }

    [Fact]
    public void FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var min = LazyBinomialHeap<int>.FindMin(heap);
        Assert.Equal(0, min);
    }

    [Fact]
    public void DeleteMinEmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => LazyBinomialHeap<int>.DeleteMin(empty));
    }

    [Fact]
    public void DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        heap = LazyBinomialHeap<int>.DeleteMin(heap);
        Assert.Equal("$[1]; [2, [3]]; [4, [6, [7]], [5]]", DumpHeap(heap, true));
    }

    [Fact]
    public void DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < size; i++) heap = LazyBinomialHeap<int>.Insert(random.Next(size), heap);
        Assert.False(heap.IsValueCreated);

        var last = 0;
        var count = 0;
        while (!LazyBinomialHeap<int>.IsEmpty(heap))
        {
            var next = LazyBinomialHeap<int>.FindMin(heap);
            heap = LazyBinomialHeap<int>.DeleteMin(heap);
            Assert.True(last <= next);
            last = next;
            count++;
        }

        Assert.Equal(size, count);
    }
}