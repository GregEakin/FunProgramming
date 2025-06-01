// Copyright 2014 Gregory Eakin <greg@eakin.dev>
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
//
// Okasaki, Chris. "6.5 Lazy Paring Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 79-81. Print.

namespace FunProgLib.heap;

public static class LazyParingHeap<T> where T : IComparable<T>
{
    public sealed class Heap
    {
        public Heap(T root, Heap list, Lazy<Heap> lazyList)
        {
            Root = root;
            FunList = list;
            LazyList = lazyList;
        }

        public T Root { get; }

        public Heap FunList { get; }

        public Lazy<Heap> LazyList { get; }
    }

    private static readonly Lazy<Heap> EmptyHeapSusp = new(() => null);

    public static Heap Empty => null;

    public static bool IsEmpty(Heap list) => list == Empty;

    public static Heap Merge(Heap h1, Heap h2)
    {
        if (IsEmpty(h2)) return h1;
        if (IsEmpty(h1)) return h2;

        if (h1.Root.CompareTo(h2.Root) <= 0) return Link(h1, h2);
        return Link(h2, h1);
    }

    private static Heap Link(Heap h1, Heap h2)
    {
        if (IsEmpty(h1.FunList)) return new Heap(h1.Root, h2, h1.LazyList);
        return new Heap(h1.Root, Empty, new Lazy<Heap>(() => Merge(Merge(h2, h1.FunList), h1.LazyList.Value)));
    }

    public static Heap Insert(T x, Heap h) => Merge(new Heap(x, Empty, EmptyHeapSusp), h);

    public static T FindMin(Heap h)
    {
        if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
        return h.Root;
    }

    public static Heap DeleteMin(Heap h)
    {
        if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
        return Merge(h.FunList, h.LazyList.Value);
    }
}