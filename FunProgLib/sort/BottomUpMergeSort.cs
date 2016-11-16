// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.3 Example: Bottom-Up  Mergesort with Sharing." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 144-7. Print.

namespace FunProgLib.sort
{
    using System;
    using lists;

    public static class BottomUpMergeSort<T> where T : IComparable<T>
    {
        public sealed class Sortable
        {
            public Sortable(int size, Lazy<List<List<T>.Node>.Node> segs)
            {
                Size = size;
                Segs = segs;
            }

            public int Size { get; }

            public Lazy<List<List<T>.Node>.Node> Segs { get; }
        }

        private static List<T>.Node Mrg(List<T>.Node xs, List<T>.Node ys)
        {
            if (xs == null) return ys;
            if (ys == null) return xs;
            if (xs.Element.CompareTo(ys.Element) <= 0) return List<T>.Cons(xs.Element, Mrg(xs.Next, ys));
            return List<T>.Cons(ys.Element, Mrg(xs, ys.Next));
        }

        public static Sortable Empty { get; } = new Sortable(0, new Lazy<List<List<T>.Node>.Node>(() => List<List<T>.Node>.Empty));

        public static Sortable Add(T x, Sortable segs)
        {
            var xs = List<T>.Cons(x, List<T>.Empty);
            return new Sortable(segs.Size + 1, new Lazy<List<List<T>.Node>.Node>(AddSeg(xs, segs.Segs.Value, segs.Size)));
        }

        private static Func<List<List<T>.Node>.Node> AddSeg(List<T>.Node seg, List<List<T>.Node>.Node segs, int size)
        {
            if (size % 2 == 0) return () => List<List<T>.Node>.Cons(seg, segs);
            return AddSeg(Mrg(seg, segs.Element), segs.Next, size / 2);
        }

        public static List<T>.Node Sort(Sortable segs)
        {
            return MrgAll(List<T>.Empty, segs.Segs.Value);
        }

        private static List<T>.Node MrgAll(List<T>.Node xs, List<List<T>.Node>.Node ys)
        {
            if (ys == null) return xs;
            return MrgAll(Mrg(xs, ys.Element), ys.Next);
        }
    }
}