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

public class LeftistHeapTests
{
    private static string DumpHeap<T>(LeftistHeap<T>.Heap heap)
        where T : IComparable<T>
    {
        if (LeftistHeap<T>.IsEmpty(heap))
            return "\u2205";
        var results = new StringBuilder();

        if (!LeftistHeap<T>.IsEmpty(heap.A))
        {
            results.Append(DumpHeap(heap.A));
        }

        results.Append(heap.X);
        //results.Append(" [");
        //results.Append(heap.r);
        //results.Append(']');
        results.Append(", ");

        if (!LeftistHeap<T>.IsEmpty(heap.B))
        {
            results.Append(DumpHeap(heap.B));
        }

        return results.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var heap = LeftistHeap<int>.Empty;
        await Assert.That(DumpHeap(heap)).IsEqualTo("\u2205");
    }

    [Test]
    public async Task EmptyIsEmptyTest()
    {
        var heap = LeftistHeap<int>.Empty;
        await Assert.That(LeftistHeap<int>.IsEmpty(heap)).IsTrue();
    }

    [Test]
    public async Task EmptyMinTest()
    {
        var heap = LeftistHeap<int>.Empty;
        await Assert.That(() => LeftistHeap<int>.FindMin(heap)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyDeleteMinTest()
    {
        var heap = LeftistHeap<int>.Empty;
        await Assert.That(() => LeftistHeap<int>.DeleteMin(heap)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task SingleElement()
    {
        var heap = LeftistHeap<int>.Empty;
        heap = LeftistHeap<int>.Insert(2, heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("2, ");
    }

    [Test]
    public async Task SingleIsEmptyTest()
    {
        var heap = LeftistHeap<int>.Empty;
        heap = LeftistHeap<int>.Insert(2, heap);
        await Assert.That(LeftistHeap<int>.IsEmpty(heap)).IsFalse();
    }

    [Test]
    public async Task SingleMinTest()
    {
        var heap = LeftistHeap<int>.Empty;
        heap = LeftistHeap<int>.Insert(2, heap);
        var x = LeftistHeap<int>.FindMin(heap);
        await Assert.That(x).IsEqualTo(2);
    }

    [Test]
    public async Task SingleDeleteMinTest()
    {
        var heap = LeftistHeap<int>.Empty;
        heap = LeftistHeap<int>.Insert(2, heap);
        heap = LeftistHeap<int>.DeleteMin(heap);
        await Assert.That(LeftistHeap<int>.IsEmpty(heap)).IsTrue();
    }

    [Test]
    public async Task DumpTreeTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        await Assert.That(DumpHeap(heap)).IsEqualTo("3, 2, 5, 1, ");
    }

    [Test]
    public async Task InsertFourTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        heap = LeftistHeap<int>.Insert(4, heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("3, 2, 5, 1, 4, ");
        await Assert.That(LeftistHeap<int>.FindMin(heap)).IsEqualTo(1);
    }

    [Test]
    public async Task InsertZeroTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        heap = LeftistHeap<int>.Insert(0, heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("3, 2, 5, 1, 0, ");
        await Assert.That(LeftistHeap<int>.FindMin(heap)).IsEqualTo(0);
    }

    [Test]
    public async Task MinTreeTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        await Assert.That(LeftistHeap<int>.FindMin(heap)).IsEqualTo(1);
    }

    [Test]
    public async Task DelMinTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        heap = LeftistHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("3, 2, 5, ");
        await Assert.That(LeftistHeap<int>.FindMin(heap)).IsEqualTo(2);
    }

    [Test]
    public async Task DelSecondMinTest()
    {
        var heap = new[]
        {
            3,
            2,
            5,
            1
        }.Aggregate(LeftistHeap<int>.Empty, (h, x) => LeftistHeap<int>.Insert(x, h));
        heap = LeftistHeap<int>.DeleteMin(heap);
        heap = LeftistHeap<int>.DeleteMin(heap);
        await Assert.That(DumpHeap(heap)).IsEqualTo("5, 3, ");
        await Assert.That(LeftistHeap<int>.FindMin(heap)).IsEqualTo(3);
    }

    [Test]
    public async Task MergeTest()
    {
        var heap1 = new[]
        {
            "How",
            "now,"
        }.Aggregate(LeftistHeap<string>.Empty, (h, x) => LeftistHeap<string>.Insert(x, h));
        var heap2 = new[]
        {
            "brown",
            "cow?"
        }.Aggregate(LeftistHeap<string>.Empty, (h, x) => LeftistHeap<string>.Insert(x, h));
        var heap = LeftistHeap<string>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap)).IsEqualTo("cow?, brown, now,, How, ");
        await Assert.That(LeftistHeap<string>.FindMin(heap)).IsEqualTo("brown");
    }

    [Test]
    public async Task DeleteLotsOfMinsTest()
    {
        var random = new Random(3456);
        var heap = LeftistHeap<int>.Empty;
        for (var i = 0; i < 100; i++)
            heap = LeftistHeap<int>.Insert(random.Next(100), heap);
        var last = 0;
        var count = 0;
        while (!LeftistHeap<int>.IsEmpty(heap))
        {
            var next = LeftistHeap<int>.FindMin(heap);
            heap = LeftistHeap<int>.DeleteMin(heap);
            await Assert.That(last <= next).IsTrue();
            last = next;
            count++;
        }

        await Assert.That(count).IsEqualTo(100);
    }
}