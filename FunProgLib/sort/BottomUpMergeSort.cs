// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BottomUpMergeSort.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.sort
{
    using System;
    using FunProgLib.lists;

    public static class BottomUpMergeSort<T> where T : IComparable
    {
        public sealed class Sortable
        {
            private readonly int size;

            private readonly Lazy<List<List<T>.Node>.Node> segs;

            public Sortable(int size, Lazy<List<List<T>.Node>.Node> segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public Lazy<List<List<T>.Node>.Node> Segs
            {
                get { return segs; }
            }
        }

        private static List<T>.Node Mrg(List<T>.Node xs, List<T>.Node ys)
        {
            if (xs == null) return ys;
            if (ys == null) return xs;
            if (xs.Element.CompareTo(ys.Element) <= 0) return List<T>.Cons(xs.Element, Mrg(xs.Next, ys));
            return List<T>.Cons(ys.Element, Mrg(xs, ys.Next));
        }

        private static readonly List<T>.Node EmptyList = null;

        private static readonly List<List<T>.Node>.Node EmptyListList = null;

        private static readonly Sortable EmptySortable = new Sortable(0, new Lazy<List<List<T>.Node>.Node>(() => EmptyListList));

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

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
            return MrgAll(EmptyList, segs.Segs.Value);
        }

        private static List<T>.Node MrgAll(List<T>.Node xs, List<List<T>.Node>.Node ys)
        {
            if (ys == null) return xs;
            return MrgAll(Mrg(xs, ys.Element), ys.Next);
        }
    }
}