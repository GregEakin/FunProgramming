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

public class BootstrappedHeapTests
{
    private static string DumpElement<T>(BootstrappedHeap<T>.PrimH.Element element)
        where T : IComparable<T>
    {
        return BootstrappedHeap<T>.PrimH.IsEmpty(element) ? "Empty" : $"{{{DumpHeap(element.H1)}: {DumpElement(element.H2)}}}";
    }

    private static string DumpHeap<T>(BootstrappedHeap<T>.Heap heap)
        where T : IComparable<T>
    {
        return BootstrappedHeap<T>.IsEmpty(heap) ? "Empty" : $"[{heap.X}: {DumpElement(heap.P)}]";
    }

    [Test]
    public async Task EmptyTest()
    {
        var t = BootstrappedHeap<string>.Empty;
        await Assert.That(BootstrappedHeap<string>.IsEmpty(t)).IsTrue();
        var t1 = BootstrappedHeap<string>.Insert("C", t);
        await Assert.That(BootstrappedHeap<string>.IsEmpty(t1)).IsFalse();
    }

    [Test]
    public async Task Merge1Test()
    {
        var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(BootstrappedHeap<string>.Empty, heap2);
        await Assert.That(heap3).IsSameReferenceAs(heap2);
    }

    [Test]
    public async Task Merge2Test()
    {
        var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(heap1, BootstrappedHeap<string>.Empty);
        await Assert.That(heap3).IsSameReferenceAs(heap1);
    }

    [Test]
    public async Task Merge3Test()
    {
        var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(heap1, heap2);
        await Assert.That(DumpHeap(heap3)).IsEqualTo("[a: {[b: Empty]: {[c: Empty]: {[x: {[y: Empty]: {[z: Empty]: Empty}}]: Empty}}}]");
    }

    [Test]
    public async Task InsertTest()
    {
        var empty = BootstrappedHeap<string>.Empty;
        var heap = BootstrappedHeap<string>.Insert("A", empty);
        await Assert.That(BootstrappedHeap<string>.FindMin(heap)).IsEqualTo("A");
    }

    [Test]
    public async Task FindEmptyMinTest()
    {
        await Assert.That(() => BootstrappedHeap<string>.FindMin(BootstrappedHeap<string>.Empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task FindMinTest()
    {
        var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        await Assert.That(BootstrappedHeap<string>.FindMin(ts1)).IsEqualTo("a");
    }

    [Test]
    public async Task DeleteEmptyMinTest()
    {
        await Assert.That(() => BootstrappedHeap<string>.DeleteMin(BootstrappedHeap<string>.Empty)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task DeleteMinTest()
    {
        var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var ts2 = BootstrappedHeap<string>.DeleteMin(ts1);
        await Assert.That(BootstrappedHeap<string>.FindMin(ts2)).IsEqualTo("b");
    }
}