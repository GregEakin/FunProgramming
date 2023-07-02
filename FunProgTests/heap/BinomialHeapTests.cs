// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.heap;
using FunProgTests.utilities;

namespace FunProgTests.heap;


// using Heap = FunProgLib.heap.BinomialHeap<int>;

public class BinomialHeapTests
{
    private static string DumpTree<T>(BinomialHeap<T>.Tree tree) where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append("[");
        result.Append(tree.Root);
        if (!FunProgLib.lists.FunList<BinomialHeap<T>.Tree>.IsEmpty(tree.FunList))
        {
            foreach (var node1 in tree.FunList)
            {
                result.Append(", ");
                result.Append(DumpTree(node1));
            }
        }
        result.Append("]");
        return result.ToString();
    }

    private static string DumpHeap<T>(IEnumerable<BinomialHeap<T>.Tree> list) where T : IComparable<T>
    {
        if (Equals(list, FunProgLib.lists.FunList<BinomialHeap<T>.Tree>.Empty))
            return string.Empty;

        var result = new StringBuilder();
        foreach (var node in list)
        {
            result.Append(DumpTree(node));
            result.Append("; ");
        }
        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    [Fact]
    public void BinomialTest1()
    {
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < 16; i++)
        {
            heap = BinomialHeap<int>.Insert(i, heap);
            var dumpHeap = DumpHeap(heap);
            // Console.WriteLine(dumpHeap);

            var semicolons = Counters.CountChar(dumpHeap, ';');
            Assert.Equal(Counters.CountBinaryOnes(i + 1), semicolons);
        }
    }

    [Fact]
    public void BinomialTest2()
    {
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < 0x100; i++)
        {
            heap = BinomialHeap<int>.Insert(1, heap);
            var dumpHeap = DumpHeap(heap);
            // Console.WriteLine(dumpHeap);
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
    public void EmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        Assert.True(BinomialHeap<int>.IsEmpty(empty));

        var heap = BinomialHeap<int>.Insert(0, empty);
        Assert.False(BinomialHeap<int>.IsEmpty(heap));
    }

    [Fact]
    public void InsertTest1()
    {
        var empty = BinomialHeap<int>.Empty;
        var heap = BinomialHeap<int>.Insert(0, empty);
        Assert.Equal("[0]", DumpHeap(heap));
    }

    [Fact]
    public void InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Insert(2, heap1);
        Assert.Equal("[2]; [0, [1]]", DumpHeap(heap));
    }

    [Fact]
    public void InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Insert(3, heap1);
        Assert.Equal("[0, [2, [3]], [1]]", DumpHeap(heap));
    }

    [Fact]
    public void MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var empty = BinomialHeap<int>.Empty;
        var heap = BinomialHeap<int>.Merge(heap1, empty);
        Assert.Same(heap1, heap);
    }

    [Fact]
    public void MergeTest2()
    {
        var empty = BinomialHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(empty, heap2);
        Assert.Same(heap2, heap);
    }

    [Fact]
    public void MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[12]; [10, [11]]; [0, [2, [3]], [1]]", DumpHeap(heap));
    }

    [Fact]
    public void MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[2]; [0, [1]]; [10, [12, [13]], [11]]", DumpHeap(heap));
    }

    [Fact]
    public void MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        Assert.Equal("[0, [10, [12, [13]], [11]], [2, [3]], [1]]", DumpHeap(heap));
    }

    [Fact]
    public void FindMinEmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => BinomialHeap<int>.FindMin(empty));
    }

    [Fact]
    public void FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var min = BinomialHeap<int>.FindMin(heap);
        Assert.Equal(0, min);
    }

    [Fact]
    public void DeleteMinEmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        Assert.Throws<ArgumentNullException>(() => BinomialHeap<int>.DeleteMin(empty));
    }

    [Fact]
    public void DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        heap = BinomialHeap<int>.DeleteMin(heap);
        Assert.Equal("[1]; [2, [3]]; [4, [6, [7]], [5]]", DumpHeap(heap));
    }

    [Fact]
    public void DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < size; i++) heap = BinomialHeap<int>.Insert(random.Next(size), heap);

        var last = 0;
        var count = 0;
        while (!BinomialHeap<int>.IsEmpty(heap))
        {
            var next = BinomialHeap<int>.FindMin(heap);
            heap = BinomialHeap<int>.DeleteMin(heap);
            Assert.True(last <= next);
            last = next;
            count++;
        }
        Assert.Equal(size, count);
    }
}