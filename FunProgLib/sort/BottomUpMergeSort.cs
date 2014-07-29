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

            private readonly /*susp*/ Lazy<LinkList<LinkList<T>.List>.List> segs;

            public Sortable(int size, Lazy<LinkList<LinkList<T>.List>.List> segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public Lazy<LinkList<LinkList<T>.List>.List> Segs
            {
                get { return segs; }
            }
        }

        private static readonly LinkList<T>.List EmptyList = null;

        private static readonly LinkList<LinkList<T>.List>.List EmptyListList = null;

        private static readonly Sortable EmptySortable = new Sortable(0, /* $ */ new Lazy<LinkList<LinkList<T>.List>.List>(() => EmptyListList));

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

        private static LinkList<T>.List Mrg(LinkList<T>.List xs, LinkList<T>.List ys)
        {
            if (xs == null) return ys;
            if (ys == null) return xs;
            if (xs.Element.CompareTo(ys.Element) <= 0) return LinkList<T>.Cons(xs.Element, Mrg(xs.Next, ys));
            return LinkList<T>.Cons(ys.Element, Mrg(xs, ys.Next));
        }

        private static Func<LinkList<LinkList<T>.List>.List> AddSeg(LinkList<T>.List seg, LinkList<LinkList<T>.List>.List segs, int size)
        {
            if (size % 2 == 0) return () => LinkList<LinkList<T>.List>.Cons(seg, segs);
            return AddSeg(Mrg(seg, segs.Element), segs.Next, size / 2);
        }

        public static Sortable Add(T x, Sortable segs)
        {
            var xs = LinkList<T>.Cons(x, LinkList<T>.Empty);
            return new Sortable(segs.Size + 1, /* $ */ new Lazy<LinkList<LinkList<T>.List>.List>(AddSeg(xs, /* force */ segs.Segs.Value, segs.Size)));
        }

        private static LinkList<T>.List MrgAll(LinkList<T>.List xs, LinkList<LinkList<T>.List>.List ys)
        {
            if (ys == null) return xs;
            return MrgAll(Mrg(xs, ys.Element), ys.Next);
        }

        public static LinkList<T>.List Sort(Sortable segs)
        {
            return MrgAll(EmptyList, /* force */ segs.Segs.Value);
        }
    }
}