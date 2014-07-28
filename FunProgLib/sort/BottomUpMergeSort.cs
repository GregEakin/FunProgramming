// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BottomUpMergeSort.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.sort
{
    using System;
    using FunProgLib.persistence;

    public static class BottomUpMergeSort<T> where T : IComparable
    {
        public class Sortable
        {
            private readonly int size;

            private readonly /*susp*/ Lazy<List<List<T>.ListStructure>.ListStructure> segs;

            public Sortable(int size, Lazy<List<List<T>.ListStructure>.ListStructure> segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public Lazy<List<List<T>.ListStructure>.ListStructure> Segs
            {
                get { return segs; }
            }
        }

        private static readonly List<T>.ListStructure EmptyList = null;

        private static readonly List<List<T>.ListStructure>.ListStructure EmptyListList = null;

        private static readonly Sortable EmptySortable = new Sortable(0, /* $ */ new Lazy<List<List<T>.ListStructure>.ListStructure>(() => EmptyListList));

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

        private static List<T>.ListStructure Mrg(List<T>.ListStructure xs, List<T>.ListStructure ys)
        {
            if (xs == null) return ys;
            if (ys == null) return xs;
            if (xs.Element.CompareTo(ys.Element) <= 0) return List<T>.Cons(Mrg(xs.Next, ys), xs.Element);
            return List<T>.Cons(Mrg(xs, ys.Next), ys.Element);
        }

        private static Func<List<List<T>.ListStructure>.ListStructure> AddSeg(List<T>.ListStructure seg, List<List<T>.ListStructure>.ListStructure segs, int size)
        {
            if (size % 2 == 0) return () => List<List<T>.ListStructure>.Cons(segs, seg);
            return AddSeg(Mrg(seg, segs.Element), segs.Next, size / 2);
        }

        public static Sortable Add(Sortable segs, T x)
        {
            var xs = List<T>.Cons(List<T>.Empty, x);
            return new Sortable(segs.Size + 1, /* $ */ new Lazy<List<List<T>.ListStructure>.ListStructure>(AddSeg(xs, /* force */ segs.Segs.Value, segs.Size)));
        }

        private static List<T>.ListStructure MrgAll(List<T>.ListStructure xs, List<List<T>.ListStructure>.ListStructure ys)
        {
            if (ys == null) return xs;
            return MrgAll(Mrg(xs, ys.Element), ys.Next);
        }

        public static List<T>.ListStructure Sort(Sortable segs)
        {
            return MrgAll(EmptyList, /* force */ segs.Segs.Value);
        }
    }
}