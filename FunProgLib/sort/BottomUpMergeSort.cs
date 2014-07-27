// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BottomUpMergeSort.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.sort
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class BottomUpMergeSort<T> where T : IComparable
    {
        public class Sortable
        {
            private readonly int count;

            private readonly /*susp*/ ReadOnlyCollection<ReadOnlyCollection<T>> segs;

            public Sortable(int count, ReadOnlyCollection<ReadOnlyCollection<T>> segs)
            {
                this.count = count;
                this.segs = segs;
            }

            public int Count
            {
                get { return count; }
            }

            public ReadOnlyCollection<ReadOnlyCollection<T>> Segs
            {
                get { return segs; }
            }
        }

        private static readonly ReadOnlyCollection<T> EmptyList = new ReadOnlyCollection<T>(new T[0]);

        private static readonly ReadOnlyCollection<ReadOnlyCollection<T>> EmptyListList = new ReadOnlyCollection<ReadOnlyCollection<T>>(new[] { EmptyList });

        private static readonly Sortable EmptySortable = new Sortable(0, EmptyListList);

        public static Sortable Empty
        {
            get { return /* $ */ EmptySortable; }
        }

        private static ReadOnlyCollection<T> Mrg(ReadOnlyCollection<T> xs, ReadOnlyCollection<T> ys)
        {
            if (xs.Count == 0) return ys;
            if (ys.Count == 0) return xs;
            var x = xs[0];
            var xsp = xs.Skip(1).ToList().AsReadOnly();
            var y = ys[0];
            var ysp = ys.Skip(1).ToList().AsReadOnly();
            if (x.CompareTo(y) <= 0) return Concatenate(x, Mrg(xsp, ys));
            return Concatenate(y, Mrg(xs, ysp));
        }

        private static ReadOnlyCollection<T> Concatenate(T element, IEnumerable<T> list)
        {
            var x = list.ToList();
            x.Insert(0, element);
            return x.AsReadOnly();
        }

        private static ReadOnlyCollection<ReadOnlyCollection<T>> Concatenate(ReadOnlyCollection<T> element, IEnumerable<ReadOnlyCollection<T>> list)
        {
            var x = list.ToList();
            x.Insert(0, element);
            return x.AsReadOnly();
        }

        private static ReadOnlyCollection<ReadOnlyCollection<T>> AddSeg(ReadOnlyCollection<T> seg, IReadOnlyList<ReadOnlyCollection<T>> segs, int size)
        {
            if (size % 2 == 0) return Concatenate(seg, segs);
            var head = segs[0];
            var tail = segs.Skip(1).ToList().AsReadOnly();
            return AddSeg(Mrg(seg, head), tail, size / 2);
        }

        public static Sortable Add(Sortable segs, T x)
        {
            var xs = new ReadOnlyCollection<T>(new[] { x });
            return new Sortable(segs.Count + 1, /* $ */ AddSeg(xs, /* force */ segs.Segs, segs.Count));
        }

        private static ReadOnlyCollection<T> MrgAll(ReadOnlyCollection<T> xs, IReadOnlyList<ReadOnlyCollection<T>> ys)
        {
            if (ys.Count == 0) return xs;
            var seg = ys[0];
            var segs = ys.Skip(1).ToList().AsReadOnly();
            return MrgAll(Mrg(xs, seg), segs);
        }

        public static ReadOnlyCollection<T> Sort(Sortable segs)
        {
            return MrgAll(EmptyList, /* force */ segs.Segs);
        }
    }
}