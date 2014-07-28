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

            private readonly /*susp*/ Lazy<LinkList<LinkList<T>.ListStructure>.ListStructure> segs;

            public Sortable(int size, Lazy<LinkList<LinkList<T>.ListStructure>.ListStructure> segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public Lazy<LinkList<LinkList<T>.ListStructure>.ListStructure> Segs
            {
                get { return segs; }
            }
        }

        private static readonly LinkList<T>.ListStructure EmptyList = null;

        private static readonly LinkList<LinkList<T>.ListStructure>.ListStructure EmptyListList = null;

        private static readonly Sortable EmptySortable = new Sortable(0, /* $ */ new Lazy<LinkList<LinkList<T>.ListStructure>.ListStructure>(() => EmptyListList));

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

        private static LinkList<T>.ListStructure Mrg(LinkList<T>.ListStructure xs, LinkList<T>.ListStructure ys)
        {
            if (xs == null) return ys;
            if (ys == null) return xs;
            if (xs.Element.CompareTo(ys.Element) <= 0) return LinkList<T>.Cons(Mrg(xs.Next, ys), xs.Element);
            return LinkList<T>.Cons(Mrg(xs, ys.Next), ys.Element);
        }

        private static Func<LinkList<LinkList<T>.ListStructure>.ListStructure> AddSeg(LinkList<T>.ListStructure seg, LinkList<LinkList<T>.ListStructure>.ListStructure segs, int size)
        {
            if (size % 2 == 0) return () => LinkList<LinkList<T>.ListStructure>.Cons(segs, seg);
            return AddSeg(Mrg(seg, segs.Element), segs.Next, size / 2);
        }

        public static Sortable Add(Sortable segs, T x)
        {
            var xs = LinkList<T>.Cons(LinkList<T>.Empty, x);
            return new Sortable(segs.Size + 1, /* $ */ new Lazy<LinkList<LinkList<T>.ListStructure>.ListStructure>(AddSeg(xs, /* force */ segs.Segs.Value, segs.Size)));
        }

        private static LinkList<T>.ListStructure MrgAll(LinkList<T>.ListStructure xs, LinkList<LinkList<T>.ListStructure>.ListStructure ys)
        {
            if (ys == null) return xs;
            return MrgAll(Mrg(xs, ys.Element), ys.Next);
        }

        public static LinkList<T>.ListStructure Sort(Sortable segs)
        {
            return MrgAll(EmptyList, /* force */ segs.Segs.Value);
        }
    }
}