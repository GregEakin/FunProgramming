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

public class ParingHeapTests
{
    private static string DumpHeap<T>(ParingHeap<T>.Heap node)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        result.Append(node.Root);
        if (!FunProgLib.lists.FunList<ParingHeap<T>.Heap>.IsEmpty(node.FunList) && node.FunList.Any())
        {
            result.Append(": ");
            foreach (var node1 in node.FunList)
                result.Append(DumpHeap(node1));
        }

        result.Append(']');
        return result.ToString();
    }

    private static string DumpHeapList<T>(IEnumerable<ParingHeap<T>.Heap> list)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        if (Equals(list, FunProgLib.lists.FunList<ParingHeap<T>.Heap>.Empty))
        {
            foreach (var node in list)
            {
                result.Append(DumpHeap(node));
            }

            result.Append(", ");
        }

        result.Remove(result.Length - 2, 2);
        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var empty = ParingHeap<int>.Empty;
        await Assert.That(ParingHeap<int>.IsEmpty(empty)).IsTrue();
        var heap = ParingHeap<int>.Insert(3, empty);
        await Assert.That(ParingHeap<int>.IsEmpty(heap)).IsFalse();
    }

    [Test]
    public async Task MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var empty = ParingHeap<int>.Empty;
        var heap = ParingHeap<int>.Merge(heap1, empty);
        await Assert.That(heap).IsSameReferenceAs(heap1);
    }

    [Test]
    public async Task MergeTest2()
    {
        var empty = ParingHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Merge(empty, heap2);
        await Assert.That(heap).IsSameReferenceAs(heap2);
    }

    [Test]
    public async Task MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0: [10: [12][11]][3][2][1]]");
    }

    [Test]
    public async Task MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0: [10: [13][12][11]][2][1]]");
    }

    [Test]
    public async Task MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0: [10: [13][12][11]][3][2][1]]");
    }

    [Test]
    public async Task InsertTest1()
    {
        var empty = ParingHeap<int>.Empty;
        var heap = ParingHeap<int>.Insert(0, empty);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0]");
    }

    [Test]
    public async Task InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Insert(2, heap1);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0: [2][1]]");
    }

    [Test]
    public async Task InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var heap = ParingHeap<int>.Insert(3, heap1);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0: [3][2][1]]");
    }

    [Test]
    public async Task FindMinEmptyTest()
    {
        var empty = ParingHeap<int>.Empty;
        await Assert.That(() => ParingHeap<int>.FindMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        var min = ParingHeap<int>.FindMin(heap);
        await Assert.That(min).IsEqualTo(0);
    }

    [Test]
    public async Task DeleteMinEmptyTest()
    {
        var empty = ParingHeap<int>.Empty;
        await Assert.That(() => ParingHeap<int>.DeleteMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(ParingHeap<int>.Empty, (current, i) => ParingHeap<int>.Insert(i, current));
        heap = ParingHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[1: [6: [7]][4: [5]][2: [3]]]");
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = ParingHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = ParingHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!ParingHeap<int>.IsEmpty(heap))
        {
            var next = ParingHeap<int>.FindMin(heap);
            heap = ParingHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }
}