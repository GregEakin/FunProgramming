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
using FunProgTests.utilities;
// using Heap = FunProgLib.heap.BinomialHeap<int>;

namespace FunProgTests.heap;

public class BinomialHeapTests
{
    private static string DumpTree<T>(BinomialHeap<T>.Tree tree)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        result.Append(tree.Root);
        if (!FunProgLib.lists.FunList<BinomialHeap<T>.Tree>.IsEmpty(tree.FunList))
        {
            foreach (var node1 in tree.FunList)
            {
                result.Append(", ");
                result.Append(DumpTree(node1));
            }
        }

        result.Append(']');
        return result.ToString();
    }

    private static string DumpHeap<T>(IEnumerable<BinomialHeap<T>.Tree> list)
        where T : IComparable<T>
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

    [Test]
    public async Task BinomialTest1()
    {
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < 16; i++)
        {
            heap = BinomialHeap<int>.Insert(i, heap);
            var dumpHeap = DumpHeap(heap);
            var semicolons = Counters.CountChar(dumpHeap, ';');
            await Assert.That(semicolons).IsEqualTo(Counters.CountBinaryOnes(i + 1) - 1);
        }
    }

    [Test]
    public async Task BinomialTest2()
    {
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < 0x100; i++)
        {
            heap = BinomialHeap<int>.Insert(1, heap);
            var dumpHeap = DumpHeap(heap);
            var blocks = dumpHeap.Split(';');
            var j = 0;
            var p = 0;
            for (var k = i + 1; k > 0; k >>= 1, j++)
            {
                if (k % 2 == 0)
                    continue;
                var q = (int)Math.Pow(2, j);
                var block = blocks[p++];
                await Assert.That(Counters.CountChar(block, '1')).IsEqualTo(q);
            }
        }
    }

    [Test]
    public async Task EmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        await Assert.That(BinomialHeap<int>.IsEmpty(empty)).IsTrue();
        var heap = BinomialHeap<int>.Insert(0, empty);
        await Assert.That(BinomialHeap<int>.IsEmpty(heap)).IsFalse().IsFalse();
    }

    [Test]
    public async Task InsertTest1()
    {
        var empty = BinomialHeap<int>.Empty;
        var heap = BinomialHeap<int>.Insert(0, empty);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0]");
    }

    [Test]
    public async Task InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Insert(2, heap1);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[2]; [0, [1]]");
    }

    [Test]
    public async Task InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Insert(3, heap1);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0, [2, [3]], [1]]");
    }

    [Test]
    public async Task MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var empty = BinomialHeap<int>.Empty;
        var heap = BinomialHeap<int>.Merge(heap1, empty);
        await Assert.That(heap).IsSameReferenceAs(heap1);
    }

    [Test]
    public async Task MergeTest2()
    {
        var empty = BinomialHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(empty, heap2);
        await Assert.That(heap).IsSameReferenceAs(heap2);
    }

    [Test]
    public async Task MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[12]; [10, [11]]; [0, [2, [3]], [1]]");
    }

    [Test]
    public async Task MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[2]; [0, [1]]; [10, [12, [13]], [11]]");
    }

    [Test]
    public async Task MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var heap = BinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[0, [10, [12, [13]], [11]], [2, [3]], [1]]");
    }

    [Test]
    public async Task FindMinEmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        await Assert.That(() => BinomialHeap<int>.FindMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        var min = BinomialHeap<int>.FindMin(heap);
        await Assert.That(min).IsEqualTo(0);
    }

    [Test]
    public async Task DeleteMinEmptyTest()
    {
        var empty = BinomialHeap<int>.Empty;
        await Assert.That(() => BinomialHeap<int>.DeleteMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(BinomialHeap<int>.Empty, (current, i) => BinomialHeap<int>.Insert(i, current));
        heap = BinomialHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("[1]; [2, [3]]; [4, [6, [7]], [5]]");
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = BinomialHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = BinomialHeap<int>.Insert(random.Next(size), heap);
        var last = 0;
        var count = 0;
        while (!BinomialHeap<int>.IsEmpty(heap))
        {
            var next = BinomialHeap<int>.FindMin(heap);
            heap = BinomialHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }
}