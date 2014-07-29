// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LazyParingHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.heap
{
    using System;

    public static class LazyParingHeap<T> where T : IComparable
    {
        public class Heap
        {
            private readonly T root;

            private readonly Heap list;

            private readonly Lazy<Heap> list2;  /* susp */

            public Heap(T root, Heap list, Lazy<Heap> list2)
            {
                this.root = root;
                this.list = list;
                this.list2 = list2;
            }

            public T Root
            {
                get { return root; }
            }

            public Heap List
            {
                get { return list; }
            }

            public Lazy<Heap> List2
            {
                get { return list2; }
            }
        }

        private static readonly Lazy<Heap> EmptyHeapSusp = /* $ */ new Lazy<Heap>(() => null);

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
            if (IsEmpty(h1.List)) return new Heap(h1.Root, h2, h1.List2);
            return new Heap(h1.Root, EmptyHeap, new Lazy<Heap>(() => Merge(Merge(h2, h1.List), h1.List2.Value)));
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
            return Merge(h.List, h.List2.Value);
        }
    }
}