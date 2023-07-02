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

public class SkewBinomialHeapTests
{
    private static string DumpList<T>(FunProgLib.lists.FunList<T>.Node tree) where T : IComparable<T>
    {
        if (FunProgLib.lists.FunList<T>.IsEmpty(tree))
            return string.Empty;

        var result = new StringBuilder();
        result.Append(", ");
        foreach (var node1 in tree)
        {
            result.Append(node1);
            result.Append(", ");
        }
        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    private static string DumpTree<T>(SkewBinomialHeap<T>.Tree tree) where T : IComparable<T>
    {
        if (tree == null) return string.Empty;
        var result = new StringBuilder();
        result.Append("[");
        //result.Append(tree.Rank);
        //result.Append(", ");
        result.Append(tree.Root);
        result.Append(DumpList(tree.FunList));
        if (!FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.IsEmpty(tree.TreeList))
            result.Append(DumpHeap(tree.TreeList));
        result.Append("]");
        return result.ToString();
    }

    private static string DumpHeap<T>(FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Node heap) where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append("[");
        while (!FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.IsEmpty(heap))
        {
            var head = FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Head(heap);
            result.Append(DumpTree(head));
            heap = FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Tail(heap);
        }
        result.Append("]");
        return result.ToString();
    }

    [Fact]
    public void EmptyTest()
    {
        var t = SkewBinomialHeap<string>.Empty;
        Assert.True(SkewBinomialHeap<string>.IsEmpty(t));

        var t1 = SkewBinomialHeap<string>.Insert("C", t);
        Assert.False(SkewBinomialHeap<string>.IsEmpty(t1));
    }

    [Fact]
    public void TestEmpty()
    {
        var t = SkewBinomialHeap<string>.Empty;
        Assert.Equal("[]", DumpHeap(t));
    }

    [Fact]
    public void Test0()
    {
        var t = SkewBinomialHeap<string>.Empty;
        var x1 = SkewBinomialHeap<string>.Insert("C", t);
        Assert.Equal("[[C]]", DumpHeap(x1));
    }

    [Fact]
    public void Test1()
    {
        var t = SkewBinomialHeap<string>.Empty;
        var x1 = SkewBinomialHeap<string>.Insert("C", t);
        var x2 = SkewBinomialHeap<string>.Insert("B", x1);
        Assert.Equal("[[B][C]]", DumpHeap(x2));
    }

    [Fact]
    public void Test2()
    {
        const string words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var t = words.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));
        Assert.Equal("[[as, sweet.[[smell]]][a, would, name, rose[[a, we, in[[name?, which[[That]]][What's]]][any, other[[by]]][call]]]]", DumpHeap(t));
    }

    [Fact]
    public void MergeTest()
    {
        const string data1 = "What's in a name?";
        var ts1 = data1.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));

        const string data2 = "That which we call a rose by any other name would smell as sweet";
        var ts2 = data2.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));

        var t = SkewBinomialHeap<string>.Merge(ts1, ts2);
        Assert.Equal("[[name?][a, in[[What's]]][a, by, rose[[any, sweet, name[[as, smell[[would]]][other]]][That, we[[which]]][call]]]]", DumpHeap(t));
    }

    [Fact]
    public void DeleteMinTest()
    {
        var t = SkewBinomialHeap<int>.Empty;
        var t1 = SkewBinomialHeap<int>.Insert(5, t);
        var t2 = SkewBinomialHeap<int>.Insert(3, t1);
        var t3 = SkewBinomialHeap<int>.Insert(6, t2);

        var t4 = SkewBinomialHeap<int>.DeleteMin(t3);
        Assert.Equal("[[6][5]]", DumpHeap(t4));
        Assert.Equal(5, SkewBinomialHeap<int>.FindMin(t4));

        Assert.Equal(3, SkewBinomialHeap<int>.FindMin(t3));
    }

    [Fact]
    public void DeleteLotsOfMinsTest()
    {
        var random = new Random(3456);
        var heap = SkewBinomialHeap<int>.Empty;
        for (var i = 0; i < 100; i++) heap = SkewBinomialHeap<int>.Insert(random.Next(100), heap);
        var last = 0;
        var count = 0;
        while (!SkewBinomialHeap<int>.IsEmpty(heap))
        {
            var next = SkewBinomialHeap<int>.FindMin(heap);
            heap = SkewBinomialHeap<int>.DeleteMin(heap);
            Assert.True(last <= next);
            last = next;
            count++;
        }
        Assert.Equal(100, count);
    }

    [Fact]
    public void DeleteLotsOfMinsTest2()
    {
        var random = new Random(1000);
        var t = SkewBinomialHeap<int>.Empty;

        var min = 0;
        for (var i = 0; i < 1000; i++)
        {
            var j = random.Next(1000);
            min = Math.Min(j, min);
            t = SkewBinomialHeap<int>.Insert(j, t);

            j = random.Next(1000);
            min = Math.Min(j, min);
            t = SkewBinomialHeap<int>.Insert(j, t);

            var k = SkewBinomialHeap<int>.FindMin(t);
            t = SkewBinomialHeap<int>.DeleteMin(t);
            Assert.True(min <= k);
            min = k;
        }

        for (var i = 0; i < 1000; i++)
        {
            var j = SkewBinomialHeap<int>.FindMin(t);
            t = SkewBinomialHeap<int>.DeleteMin(t);
            Assert.True(min <= j);
            min = j;
        }

        Assert.True(SkewBinomialHeap<int>.IsEmpty(t));
    }
}