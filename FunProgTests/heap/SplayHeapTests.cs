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

public class SplayHeapTests
{
    private static string DumpHeap<T>(SplayHeap<T>.Heap heap)
        where T : IComparable<T>
    {
        if (SplayHeap<T>.IsEmpty(heap))
            return "\u2205";
        var result = new StringBuilder();
        result.Append('[');

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

        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var t = SplayHeap<string>.Empty;
        await Assert.That(SplayHeap<string>.IsEmpty(t)).IsTrue();
        await Assert.That(DumpHeap(t)).IsEqualTo("\u2205");
        var t1 = SplayHeap<string>.Insert("C", t);
        await Assert.That(SplayHeap<string>.IsEmpty(t1)).IsFalse();
    }

    [Test]
    public async Task Test1()
    {
        var t = SplayHeap<string>.Empty;
        var x1 = SplayHeap<string>.Insert("C", t);
        var x2 = SplayHeap<string>.Insert("B", x1);
        await Assert.That(DumpHeap(x2)).IsEqualTo("[B, [C]]");
    }

    [Test]
    public async Task Test2()
    {
        const string words = "What's in a name? That which we call a rose by any other name would smell as sweet";
        var ts = words.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));
        await Assert.That(DumpHeap(ts)).IsEqualTo("[[[[[[a], a], any], as, [by, [[call, [in]], name, [name?]]]], other, [[rose], smell]], sweet, [[That, [[we], What's, [which]]], would]]");
    }

    [Test]
    public async Task MergeTest()
    {
        const string data1 = "What's in a name?";
        var ts1 = data1.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));
        const string data2 = "That which we call a rose by any other name would smell as sweet";
        var ts2 = data2.Split().Aggregate(SplayHeap<string>.Empty, (current, word) => SplayHeap<string>.Insert(word, current));
        var t = SplayHeap<string>.Merge(ts1, ts2);
        await Assert.That(DumpHeap(t)).IsEqualTo("[[[[a], a, [any, [as]]], by, [[call], in, [name]]], name?, [other, [[[[rose], smell], sweet, [That, [we]]], What's, [[which], would]]]]");
    }

    [Test]
    public async Task FindMinTest1()
    {
        var t = SplayHeap<int>.Empty;
        await Assert.That(() => SplayHeap<int>.FindMin(t)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest2()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var result = SplayHeap<int>.FindMin(t1);
        await Assert.That(result).IsEqualTo(5);
    }

    [Test]
    public async Task FindMinTest3()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var t2 = SplayHeap<int>.Insert(3, t1);
        var result = SplayHeap<int>.FindMin(t2);
        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task FindMinTest4()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var result = SplayHeap<int>.FindMin(t2);
        await Assert.That(result).IsEqualTo(3);
    }

    [Test]
    public async Task DeleteMinTest1()
    {
        var t = SplayHeap<int>.Empty;
        await Assert.That(() => SplayHeap<int>.DeleteMin(t)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest2()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var result = SplayHeap<int>.DeleteMin(t1);
        await Assert.That(DumpHeap(result)).IsEqualTo("\u2205");
    }

    [Test]
    public async Task DeleteMinTest3()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(5, t0);
        var t2 = SplayHeap<int>.Insert(3, t1);
        var result = SplayHeap<int>.DeleteMin(t2);
        await Assert.That(DumpHeap(result)).IsEqualTo("[5]");
    }

    [Test]
    public async Task DeleteMinTest4()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var result = SplayHeap<int>.DeleteMin(t2);
        await Assert.That(DumpHeap(result)).IsEqualTo("[5]");
    }

    [Test]
    public async Task DeleteMinTest5()
    {
        var t0 = SplayHeap<int>.Empty;
        var t1 = SplayHeap<int>.Insert(3, t0);
        var t2 = SplayHeap<int>.Insert(5, t1);
        var t3 = SplayHeap<int>.Insert(6, t2);
        var result = SplayHeap<int>.DeleteMin(t3);
        await Assert.That(DumpHeap(result)).IsEqualTo("[5, [6]]");
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = SplayHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = SplayHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!SplayHeap<int>.IsEmpty(heap))
        {
            var next = SplayHeap<int>.FindMin(heap);
            heap = SplayHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }

    [Test]
    public async Task Test3()
    {
        var heap = SplayHeap<int>.Empty;
        for (var i = 1; i < 8; i++)
            heap = SplayHeap<int>.Insert(i, heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[[[[[[[1], 2], 3], 4], 5], 6], 7]");
        var x = SplayHeap<int>.Insert(0, heap);
        await Assert.That(DumpHeap(x)).IsEqualTo("[0, [[[[1], 2, [3]], 4, [5]], 6, [7]]]");
        var y = SplayHeap<int>.DeleteMin(x);
        await Assert.That(DumpHeap(y)).IsEqualTo("[[[[1], 2, [3]], 4, [5]], 6, [7]]");
    }
}