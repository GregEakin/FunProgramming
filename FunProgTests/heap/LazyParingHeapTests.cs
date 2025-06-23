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

public class LazyParingHeapTests
{
    private static string DumpHeap<T>(LazyParingHeap<T>.Heap node, bool showSusp)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
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

        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        await Assert.That(LazyParingHeap<int>.IsEmpty(empty)).IsTrue();
        var heap = LazyParingHeap<int>.Insert(3, empty);
        await Assert.That(LazyParingHeap<int>.IsEmpty(heap)).IsFalse();
    }

    [Test]
    public async Task MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var empty = LazyParingHeap<int>.Empty;
        var heap = LazyParingHeap<int>.Merge(heap1, empty);
        await Assert.That(heap).IsSameReferenceAs(heap1);
    }

    [Test]
    public async Task MergeTest2()
    {
        var empty = LazyParingHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(empty, heap2);
        await Assert.That(heap).IsSameReferenceAs(heap2);
    }

    [Test]
    public async Task MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0; [1; [2, [3, [10; [11, [12]]]]]]]");
    }

    [Test]
    public async Task MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0, [10, [13]; [11, [12]]]; [1, [2]]]");
    }

    [Test]
    public async Task MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0; [1; [2, [3, [10, [13]; [11, [12]]]]]]]");
    }

    [Test]
    public async Task InsertTest1()
    {
        var empty = LazyParingHeap<int>.Empty;
        var heap = LazyParingHeap<int>.Insert(0, empty);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0]");
    }

    [Test]
    public async Task InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Insert(2, heap1);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0; [1, [2]]]");
    }

    [Test]
    public async Task InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var heap = LazyParingHeap<int>.Insert(3, heap1);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[0, [3]; [1, [2]]]");
    }

    [Test]
    public async Task FindMinEmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        await Assert.That(() => LazyParingHeap<int>.FindMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        var min = LazyParingHeap<int>.FindMin(heap);
        await Assert.That(min).IsEqualTo(0);
    }

    [Test]
    public async Task DeleteMinEmptyTest()
    {
        var empty = LazyParingHeap<int>.Empty;
        await Assert.That(() => LazyParingHeap<int>.DeleteMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyParingHeap<int>.Empty, (current, i) => LazyParingHeap<int>.Insert(i, current));
        heap = LazyParingHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("[1; [2; [3; [4, [5; [6, [7]]]]]]]");
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = LazyParingHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = LazyParingHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!LazyParingHeap<int>.IsEmpty(heap))
        {
            var next = LazyParingHeap<int>.FindMin(heap);
            heap = LazyParingHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }
}