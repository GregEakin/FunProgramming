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
using FunProgLib.lists;
using FunProgLib.streams;

namespace FunProgTests.heap;

public class ScheduledBinomialHeapTests
{
    private static string DumpTree<T>(ScheduledBinomialHeap<T>.Tree tree)
        where T : IComparable<T>
    {
        if (tree == null)
            return string.Empty;
        var result = new StringBuilder();
        result.Append('[');
        result.Append(tree.Node);
        if (tree.TreeList != FunList<ScheduledBinomialHeap<T>.Tree>.Empty)
        {
            result.Append(": ");
            foreach (var node1 in tree.TreeList)
            {
                result.Append(DumpTree(node1));
                result.Append(", ");
            }

            result.Remove(result.Length - 2, 2);
        }

        result.Append(']');
        return result.ToString();
    }

    private static string DumpDigitStream<T>(Lazy<Stream<ScheduledBinomialHeap<T>.Digit>.StreamCell> stream)
        where T : IComparable<T>
    {
        if (stream == ScheduledBinomialHeap<T>.EmptyStream)
            return string.Empty;
        if (!stream.IsValueCreated)
            return " -$- ";
        if (stream == Stream<ScheduledBinomialHeap<T>.Digit>.DollarNil)
            return string.Empty;
        return $"{DumpTree(stream.Value.Element.One)}{DumpDigitStream(stream.Value.Next)}";
    }

    private static string DumpHeap<T>(ScheduledBinomialHeap<T>.Heap heap)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        if (heap.DigitStream != Stream<ScheduledBinomialHeap<T>.Digit>.DollarNil)
        {
            result.Append(DumpDigitStream(heap.DigitStream));
            result.Append(", ");
            result.Remove(result.Length - 2, 2);
        }

        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var t = ScheduledBinomialHeap<string>.Empty;
        await Assert.That(ScheduledBinomialHeap<string>.IsEmpty(t)).IsTrue();
        var t1 = ScheduledBinomialHeap<string>.Insert("C", t);
        await Assert.That(ScheduledBinomialHeap<string>.IsEmpty(t1)).IsFalse();
    }

    [Test]
    public async Task TestEmpty()
    {
        var t = ScheduledBinomialHeap<string>.Empty;
        await Assert.That(DumpHeap(t)).IsEqualTo("[]");
    }

    [Test]
    public async Task Test0()
    {
        var t = ScheduledBinomialHeap<string>.Empty;
        var x1 = ScheduledBinomialHeap<string>.Insert("C", t);
        await Assert.That(DumpHeap(x1)).IsEqualTo("[[C]]");
    }

    [Test]
    public async Task Test1()
    {
        var t = ScheduledBinomialHeap<string>.Empty;
        var x1 = ScheduledBinomialHeap<string>.Insert("C", t);
        var x2 = ScheduledBinomialHeap<string>.Insert("B", x1);
        await Assert.That(DumpHeap(x2)).IsEqualTo("[[B: [C]]]");
    }

    [Test]
    public async Task Test2()
    {
        const string words = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var t = words.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));
        await Assert.That(DumpHeap(t)).IsEqualTo("[[as: [sweet.]] -$- ]");
        var x = ScheduledBinomialHeap<string>.Merge(t, ScheduledBinomialHeap<string>.Empty);
        await Assert.That(DumpHeap(x)).IsEqualTo("[[as: [sweet.]][a: [a: [call: [That: [which]], [we]], [in: [What's]], [name?]], [name: [smell: [would]], [other]], [any: [by]], [rose]]]");
    }

    [Test]
    public async Task MergeTest()
    {
        const string data1 = "What's in a name?";
        var ts1 = data1.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));
        const string data2 = "That which we call a rose by any other name would smell as sweet";
        var ts2 = data2.Split().Aggregate(ScheduledBinomialHeap<string>.Empty, (current, word) => ScheduledBinomialHeap<string>.Insert(word, current));
        var t = ScheduledBinomialHeap<string>.Merge(ts1, ts2);
        await Assert.That(DumpHeap(t)).IsEqualTo("[[as: [sweet]][a: [a: [call: [That: [which]], [we]], [any: [by]], [rose]], [name: [smell: [would]], [other]], [in: [What's]], [name?]]]");
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var t = ScheduledBinomialHeap<int>.Empty;
        var t1 = ScheduledBinomialHeap<int>.Insert(5, t);
        var t2 = ScheduledBinomialHeap<int>.Insert(3, t1);
        var t3 = ScheduledBinomialHeap<int>.Insert(6, t2);
        var t4 = ScheduledBinomialHeap<int>.DeleteMin(t3);
        await Assert.That(DumpHeap(t4)).IsEqualTo("[[5: [6]]]");
        await Assert.That(ScheduledBinomialHeap<int>.FindMin(t4)).IsEqualTo(5);
        await Assert.That(ScheduledBinomialHeap<int>.FindMin(t3)).IsEqualTo(3);
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = ScheduledBinomialHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = ScheduledBinomialHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!ScheduledBinomialHeap<int>.IsEmpty(heap))
        {
            var next = ScheduledBinomialHeap<int>.FindMin(heap);
            heap = ScheduledBinomialHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }

    [Test]
    public async Task DeleteLotsOfMinsTest2()
    {
        const int size = 1000;
        var random = new Random(6435);
        var heap = ScheduledBinomialHeap<int>.Empty;
        var min = size;
        for (var i = 0; i < size; i++)
        {
            var j = random.Next(size);
            min = Math.Min(j, min);
            heap = ScheduledBinomialHeap<int>.Insert(j, heap);
            j = random.Next(size);
            min = Math.Min(j, min);
            heap = ScheduledBinomialHeap<int>.Insert(j, heap);
            var k = ScheduledBinomialHeap<int>.FindMin(heap);
            heap = ScheduledBinomialHeap<int>.DeleteMin(heap);
            await Assert.That(min <= k).IsTrue();
            min = k;
        }

        for (var i = 0; i < size; i++)
        {
            var j = ScheduledBinomialHeap<int>.FindMin(heap);
            heap = ScheduledBinomialHeap<int>.DeleteMin(heap);
            await Assert.That(min <= j).IsTrue();
            min = j;
        }

        await Assert.That(ScheduledBinomialHeap<int>.IsEmpty(heap)).IsTrue();
    }
}