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

using FunProgTests.utilities;
using FunProgLib.heap;
using FunProgLib.lists;

namespace FunProgTests.heap;

public class LazyBinomialHeapTests
{
    public static string DumpNode<T>(LazyBinomialHeap<T>.Tree tree)
        where T : IComparable<T>
    {
        var result = new StringBuilder();
        result.Append('[');
        result.Append(tree.Root);
        if (!FunList<LazyBinomialHeap<T>.Tree>.IsEmpty(tree.FunList))
        {
            foreach (var node in tree.FunList)
            {
                result.Append(", ");
                result.Append(DumpNode(node));
            }
        }

        result.Append(']');
        return result.ToString();
    }

    public static string DumpHeap<T>(Lazy<FunList<LazyBinomialHeap<T>.Tree>.Node> list, bool expandUnCreated)
        where T : IComparable<T>
    {
        if (!list.IsValueCreated && !expandUnCreated)
            return "$";

        var result = new StringBuilder();
        if (!list.IsValueCreated)
            result.Append('$');

        if (list.Value == null)
            return result.ToString();

        foreach (var node in list.Value)
        {
            result.Append(DumpNode(node));
            result.Append("; ");
        }

        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    [Test]
    public async Task DumpHeapTest()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        heap = LazyBinomialHeap<int>.Insert(1, heap);
        heap = LazyBinomialHeap<int>.Insert(2, heap);
        heap = LazyBinomialHeap<int>.Insert(3, heap);
        var dumpHeap = DumpHeap(heap, true);
        await Assert.That(dumpHeap).IsEqualTo("$[3]; [1, [2]]");
    }

    [Test]
    public async Task DumpEmptyHeapTest()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        await Assert.That(heap.Value).IsNull();
        var dumpHeap = DumpHeap(heap, true);
        await Assert.That(dumpHeap).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task BinomialTest1()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < 16; i++)
        {
            heap = LazyBinomialHeap<int>.Insert(i, heap);
            var dumpHeap = DumpHeap(heap, true);
            var semicolons = Counters.CountChar(dumpHeap, ';');
            await Assert.That(semicolons).IsEqualTo(Counters.CountBinaryOnes(i + 1) - 1);
        }
    }

    [Test]
    public async Task BinomialTest2()
    {
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < 0x100; i++)
        {
            heap = LazyBinomialHeap<int>.Insert(1, heap);
            var dumpHeap = DumpHeap(heap, true);
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
    public async Task MonolithicTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var x1 = LazyBinomialHeap<int>.Insert(3, empty);
        var x2 = LazyBinomialHeap<int>.Insert(2, x1);
        await Assert.That(x1.IsValueCreated).IsFalse();
        await Assert.That(x2.IsValueCreated).IsFalse();
        // Once we look at one element, the entire list will be forced created.
        await Assert.That(x2.Value).IsNotNull();
        await Assert.That(x1.IsValueCreated).IsTrue();
        await Assert.That(x2.IsValueCreated).IsTrue();
    }

    [Test]
    public async Task EmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        await Assert.That(LazyBinomialHeap<int>.IsEmpty(empty)).IsTrue();
        var heap = LazyBinomialHeap<int>.Insert(1, empty);
        await Assert.That(LazyBinomialHeap<int>.IsEmpty(heap)).IsFalse();
    }

    [Test]
    public async Task InsertTest1()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var heap = LazyBinomialHeap<int>.Insert(0, empty);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[0]");
    }

    [Test]
    public async Task InsertTest2()
    {
        var heap1 = Enumerable.Range(0, 2).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Insert(2, heap1);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[2]; [0, [1]]");
    }

    [Test]
    public async Task InsertTest3()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Insert(3, heap1);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[0, [2, [3]], [1]]");
    }

    [Test]
    public async Task MergeTest1()
    {
        var heap1 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var empty = LazyBinomialHeap<int>.Empty;
        var heap = LazyBinomialHeap<int>.Merge(heap1, empty);
        await Assert.That(heap.Value).IsSameReferenceAs(heap1.Value);
    }

    [Test]
    public async Task MergeTest2()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        var heap2 = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(empty, heap2);
        await Assert.That(heap.Value).IsSameReferenceAs(heap2.Value);
    }

    [Test]
    public async Task MergeTest3()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[12]; [10, [11]]; [0, [2, [3]], [1]]");
    }

    [Test]
    public async Task MergeTest4()
    {
        var heap1 = Enumerable.Range(0, 3).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[2]; [0, [1]]; [10, [12, [13]], [11]]");
    }

    [Test]
    public async Task MergeTest5()
    {
        var heap1 = Enumerable.Range(0, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap2 = Enumerable.Range(10, 4).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var heap = LazyBinomialHeap<int>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[0, [10, [12, [13]], [11]], [2, [3]], [1]]");
    }

    [Test]
    public async Task FindMinEmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        await Assert.That(() => LazyBinomialHeap<int>.FindMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        var min = LazyBinomialHeap<int>.FindMin(heap);
        await Assert.That(min).IsEqualTo(0);
    }

    [Test]
    public async Task DeleteMinEmptyTest()
    {
        var empty = LazyBinomialHeap<int>.Empty;
        await Assert.That(() => LazyBinomialHeap<int>.DeleteMin(empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var heap = Enumerable.Range(0, 8).Aggregate(LazyBinomialHeap<int>.Empty, (current, i) => LazyBinomialHeap<int>.Insert(i, current));
        heap = LazyBinomialHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap, true)).IsEqualTo("$[1]; [2, [3]]; [4, [6, [7]], [5]]");
    }

    [Test]
    public async Task DeleteLotsOfMinimumsTest()
    {
        const int size = 1000;
        var random = new Random(3456);
        var heap = LazyBinomialHeap<int>.Empty;
        for (var i = 0; i < size; i++)
            heap = LazyBinomialHeap<int>.Insert(random.Next(size), heap);
        await Assert.That(heap.IsValueCreated).IsFalse();
        var last = 0;
        var count = 0;
        while (!LazyBinomialHeap<int>.IsEmpty(heap))
        {
            var next = LazyBinomialHeap<int>.FindMin(heap);
            heap = LazyBinomialHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(size);
    }
}