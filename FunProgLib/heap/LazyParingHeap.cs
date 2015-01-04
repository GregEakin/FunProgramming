// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.5 Lazy Paring Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 79-81. Print.

using System;

namespace FunProgLib.heap
{
    public static class LazyParingHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            private readonly T root;

            private readonly Heap list;

            private readonly Lazy<Heap> lazyList;

            public Heap(T root, Heap list, Lazy<Heap> lazyList)
            {
                this.root = root;
                this.list = list;
                this.lazyList = lazyList;
            }

            public T Root
            {
                get { return root; }
            }

            public Heap List
            {
                get { return list; }
            }

            public Lazy<Heap> LazyList
            {
                get { return this.lazyList; }
            }
        }

        private static readonly Lazy<Heap> EmptyHeapSusp = new Lazy<Heap>(() => null);

        private static readonly Heap EmptyHeap = null;

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmpty(Heap list)
        {
            return list == EmptyHeap;
        }

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
            return new Heap(h1.Root, EmptyHeap, new Lazy<Heap>(() => Merge(Merge(h2, h1.List), h1.LazyList.Value)));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, EmptyHeap, EmptyHeapSusp), h);
        }

        public static T FindMin(Heap h)
        {
            if (IsEmpty(h)) throw new Exception("Empty");
            return h.Root;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (IsEmpty(h)) throw new Exception("Empty");
            return Merge(h.List, h.LazyList.Value);
        }
    }
}