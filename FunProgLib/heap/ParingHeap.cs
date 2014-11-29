// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "5.5 Pairing Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 52-54. Print.

namespace FunProgLib.heap
{
    using System;

    using FunProgLib.lists;

    public static class ParingHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            private readonly T root;

            private readonly List<Heap>.Node list;

            public Heap(T root, List<Heap>.Node list)
            {
                this.root = root;
                this.list = list;
            }

            public T Root
            {
                get { return root; }
            }

            public List<Heap>.Node List
            {
                get { return list; }
            }
        }

        private static readonly List<Heap>.Node EmptyList = null;

        private static readonly Heap EmptyHeap = null;

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmapty(Heap list)
        {
            return list == EmptyHeap;
        }

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (h2 == EmptyHeap) return h1;
            if (h1 == EmptyHeap) return h2;

            if (h1.Root.CompareTo(h2.Root) <= 0) return new Heap(h1.Root, List<Heap>.Cons(h2, h1.List));
            return new Heap(h2.Root, List<Heap>.Cons(h1, h2.List));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, EmptyList), h);
        }

        private static Heap MergePairs(List<Heap>.Node hs)
        {
            if (hs == EmptyList) return EmptyHeap;
            if (hs.Next == EmptyList) return hs.Element;
            return Merge(Merge(hs.Element, hs.Next.Element), MergePairs(hs.Next.Element.List));
        }

        public static T FindMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return h.Root;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return MergePairs(h.List);
        }
    }
}