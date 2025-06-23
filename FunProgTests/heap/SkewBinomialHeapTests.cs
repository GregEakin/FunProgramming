// Copyright 2025 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using FunProgLib.heap;

namespace FunProgTests.heap;

public class SkewBinomialHeapTests
{
    private static string DumpList<T>(FunProgLib.lists.FunList<T>.Node tree)
        where T : IComparable<T>
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

    private static string DumpTree<T>(SkewBinomialHeap<T>.Tree tree)
        where T : IComparable<T>
    {
        if (tree == null)
            return string.Empty;
        var result = new StringBuilder();
        result.Append('[');
        //result.Append(tree.Rank);
        //result.Append(", ");
        result.Append(tree.Root);
        result.Append(DumpList(tree.FunList));
        if (!FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.IsEmpty(tree.TreeList))
            result.Append(DumpHeap(tree.TreeList));
        result.Append(']');
        return result.ToString();
    }

    private static string DumpHeap<T>(FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Node heap)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        while (!FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.IsEmpty(heap))
        {
            var head = FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Head(heap);
            result.Append(DumpTree(head));
            heap = FunProgLib.lists.FunList<SkewBinomialHeap<T>.Tree>.Tail(heap);
        }

        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var t = SkewBinomialHeap<string>.Empty;
        await Assert.That(SkewBinomialHeap<string>.IsEmpty(t)).IsTrue();
        var t1 = SkewBinomialHeap<string>.Insert("C", t);
        await Assert.That(SkewBinomialHeap<string>.IsEmpty(t1)).IsFalse();
    }

    [Test]
    public async Task TestEmpty()
    {
        var t = SkewBinomialHeap<string>.Empty;
        await Assert.That(DumpHeap(t)).IsEqualTo("[]");
    }

    [Test]
    public async Task Test0()
    {
        var t = SkewBinomialHeap<string>.Empty;
        var x1 = SkewBinomialHeap<string>.Insert("C", t);
        await Assert.That(DumpHeap(x1)).IsEqualTo("[[C]]");
    }

    [Test]
    public async Task Test1()
    {
        var t = SkewBinomialHeap<string>.Empty;
        var x1 = SkewBinomialHeap<string>.Insert("C", t);
        var x2 = SkewBinomialHeap<string>.Insert("B", x1);
        await Assert.That(DumpHeap(x2)).IsEqualTo("[[B][C]]");
    }

    [Test]
    public async Task Test2()
    {
        const string words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var t = words.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));
        await Assert.That(DumpHeap(t)).IsEqualTo("[[as, sweet.[[smell]]][a, would, name, rose[[a, we, in[[name?, which[[That]]][What's]]][any, other[[by]]][call]]]]");
    }

    [Test]
    public async Task MergeTest()
    {
        const string data1 = "What's in a name?";
        var ts1 = data1.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));
        const string data2 = "That which we call a rose by any other name would smell as sweet";
        var ts2 = data2.Split().Aggregate(SkewBinomialHeap<string>.Empty, (current, word) => SkewBinomialHeap<string>.Insert(word, current));
        var t = SkewBinomialHeap<string>.Merge(ts1, ts2);
        await Assert.That(DumpHeap(t)).IsEqualTo("[[name?][a, in[[What's]]][a, by, rose[[any, sweet, name[[as, smell[[would]]][other]]][That, we[[which]]][call]]]]");
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var t = SkewBinomialHeap<int>.Empty;
        var t1 = SkewBinomialHeap<int>.Insert(5, t);
        var t2 = SkewBinomialHeap<int>.Insert(3, t1);
        var t3 = SkewBinomialHeap<int>.Insert(6, t2);
        var t4 = SkewBinomialHeap<int>.DeleteMin(t3);
        await Assert.That(DumpHeap(t4)).IsEqualTo("[[6][5]]");
        await Assert.That(SkewBinomialHeap<int>.FindMin(t4)).IsEqualTo(5);
        await Assert.That(SkewBinomialHeap<int>.FindMin(t3)).IsEqualTo(3);
    }

    [Test]
    public async Task DeleteLotsOfMinimumsTest()
    {
        var random = new Random(3456);
        var heap = SkewBinomialHeap<int>.Empty;
        for (var i = 0; i < 100; i++)
            heap = SkewBinomialHeap<int>.Insert(random.Next(100), heap);
        var last = 0;
        var count = 0;
        while (!SkewBinomialHeap<int>.IsEmpty(heap))
        {
            var next = SkewBinomialHeap<int>.FindMin(heap);
            heap = SkewBinomialHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(100);
    }

    [Test]
    public async Task DeleteLotsOfMinimumsTest2()
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
            await Assert.That(min <= k).IsTrue();
            min = k;
        }

        for (var i = 0; i < 1000; i++)
        {
            var j = SkewBinomialHeap<int>.FindMin(t);
            t = SkewBinomialHeap<int>.DeleteMin(t);
            await Assert.That(min <= j).IsTrue();
            min = j;
        }

        await Assert.That(SkewBinomialHeap<int>.IsEmpty(t)).IsTrue();
    }
}