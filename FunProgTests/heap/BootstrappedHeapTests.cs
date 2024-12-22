// Copyright 2016 Gregory Eakin <greg@eakin.dev>
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

namespace FunProgTests.heap;

using FunProgLib.heap;

public class BootstrappedHeapTests
{
    private static string DumpElement<T>(BootstrappedHeap<T>.PrimH.Element element) where T : IComparable<T>
    {
        return BootstrappedHeap<T>.PrimH.IsEmpty(element) 
            ? "Empty" 
            : $"{{{DumpHeap(element.H1)}: {DumpElement(element.H2)}}}";
    }

    private static string DumpHeap<T>(BootstrappedHeap<T>.Heap heap) where T : IComparable<T>
    {
        return BootstrappedHeap<T>.IsEmpty(heap) 
            ? "Empty" 
            : $"[{heap.X}: {DumpElement(heap.P)}]";
    }

    [Fact]
    public void EmptyTest()
    {
        var t = BootstrappedHeap<string>.Empty;
        Assert.True(BootstrappedHeap<string>.IsEmpty(t));

        var t1 = BootstrappedHeap<string>.Insert("C", t);
        Assert.False(BootstrappedHeap<string>.IsEmpty(t1));
    }

    [Fact]
    public void Merge1Test()
    {
        var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(BootstrappedHeap<string>.Empty, heap2);
        Assert.Same(heap2, heap3);
    }

    [Fact]
    public void Merge2Test()
    {
        var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(heap1, BootstrappedHeap<string>.Empty);
        Assert.Same(heap1, heap3);
    }

    [Fact]
    public void Merge3Test()
    {
        var heap1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap2 = "z x y".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var heap3 = BootstrappedHeap<string>.Merge(heap1, heap2);
        Assert.Equal("[a: {[b: Empty]: {[c: Empty]: {[x: {[y: Empty]: {[z: Empty]: Empty}}]: Empty}}}]", DumpHeap(heap3));
    }

    [Fact]
    public void InsertTest()
    {
        var empty = BootstrappedHeap<string>.Empty;
        var heap = BootstrappedHeap<string>.Insert("A", empty);
        Assert.Equal("A", BootstrappedHeap<string>.FindMin(heap));
    }

    [Fact]
    public void FindEmptyMinTest()
    {
        Assert.Throws<ArgumentNullException>(() => BootstrappedHeap<string>.FindMin(BootstrappedHeap<string>.Empty));
    }

    [Fact]
    public void FindMinTest()
    {
        var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        Assert.Equal("a", BootstrappedHeap<string>.FindMin(ts1));
    }

    [Fact]
    public void DeleteEmptyMinTest()
    {
        Assert.Throws<ArgumentNullException>(() => BootstrappedHeap<string>.DeleteMin(BootstrappedHeap<string>.Empty));
    }

    [Fact]
    public void DeleteMinTest()
    {
        var ts1 = "c a b".Split().Aggregate(BootstrappedHeap<string>.Empty, (current, word) => BootstrappedHeap<string>.Insert(word, current));
        var ts2 = BootstrappedHeap<string>.DeleteMin(ts1);
        Assert.Equal("b", BootstrappedHeap<string>.FindMin(ts2));
    }
}