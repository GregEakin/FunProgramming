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

    using lists;

    public static class ParingHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            public Heap(T root, List<Heap>.Node list)
            {
                Root = root;
                List = list;
            }

            public T Root { get; }

            public List<Heap>.Node List { get; }
        }

        public static Heap Empty { get; } = null;

        public static bool IsEmpty(Heap list)
        {
            return list == Empty;
        }

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (h2 == Empty) return h1;
            if (h1 == Empty) return h2;

            if (h1.Root.CompareTo(h2.Root) <= 0) return new Heap(h1.Root, List<Heap>.Cons(h2, h1.List));
            return new Heap(h2.Root, List<Heap>.Cons(h1, h2.List));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, List<Heap>.Empty), h);
        }

        private static Heap MergePairs(List<Heap>.Node hs)
        {
            if (List<Heap>.IsEmpty(hs)) return Empty;
            if (List<Heap>.IsEmpty(hs.Next)) return hs.Element;
            return Merge(Merge(hs.Element, hs.Next.Element), MergePairs(hs.Next.Next));
        }

        public static T FindMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
            return h.Root;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
            return MergePairs(h.List);
        }
    }
}