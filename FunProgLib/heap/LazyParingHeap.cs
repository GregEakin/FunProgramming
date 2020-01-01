// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.5 Lazy Paring Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 79-81. Print.

namespace FunProgLib.heap
{
    using System;

    public static class LazyParingHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            public Heap(T root, Heap list, Lazy<Heap> lazyList)
            {
                Root = root;
                List = list;
                LazyList = lazyList;
            }

            public T Root { get; }

            public Heap List { get; }

            public Lazy<Heap> LazyList { get; }
        }

        private static readonly Lazy<Heap> EmptyHeapSusp = new Lazy<Heap>(() => null);

        public static Heap Empty { get; } = null;

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
            if (IsEmpty(h1.List)) return new Heap(h1.Root, h2, h1.LazyList);
            return new Heap(h1.Root, Empty, new Lazy<Heap>(() => Merge(Merge(h2, h1.List), h1.LazyList.Value)));
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
            return Merge(h.List, h.LazyList.Value);
        }
    }
}