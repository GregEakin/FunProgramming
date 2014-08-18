// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ScheduledBottomUpMergeSort.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.sort
{
    using System;
    using FunProgLib.persistence;
    using FunProgLib.streams;

    public static class ScheduledBottomUpMergeSort<T> where T : IComparable
    {
        public sealed class Stuff
        {
            private readonly Lazy<Stream<T>.StreamCell> elementStream;

            private readonly LinkList<Lazy<Stream<T>.StreamCell>>.List schedule;

            public Stuff(Lazy<Stream<T>.StreamCell> elementStream, LinkList<Lazy<Stream<T>.StreamCell>>.List schedule)
            {
                this.elementStream = elementStream;
                this.schedule = schedule;
            }

            public Lazy<Stream<T>.StreamCell> ElementStream
            {
                get { return this.elementStream; }
            }

            public LinkList<Lazy<Stream<T>.StreamCell>>.List Schedule
            {
                get { return schedule; }
            }
        }

        public sealed class Sortable
        {
            private readonly int size;

            private readonly LinkList<Stuff>.List segs;

            public Sortable(int size, LinkList<Stuff>.List segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public LinkList<Stuff>.List Segs
            {
                get { return segs; }
            }
        }

        private static Lazy<Stream<T>.StreamCell> Mrg(Lazy<Stream<T>.StreamCell> xs, Lazy<Stream<T>.StreamCell> ys)
        {
            if (xs == EmptyStream) return ys;
            if (ys == EmptyStream) return xs;
            if (xs.Value.Element.CompareTo(ys.Value.Element) <= 0) return Stream<T>.Cons(xs.Value.Element, Mrg(xs.Value.Next, ys));
            return Stream<T>.Cons(ys.Value.Element, Mrg(ys.Value.Next, xs));
        }

        private static LinkList<Lazy<Stream<T>.StreamCell>>.List Exec1(LinkList<Lazy<Stream<T>.StreamCell>>.List list)
        {
            if (list == null) return null;
            if (list.Element == EmptyStream) return Exec1(list.Next);
            return LinkList<Lazy<Stream<T>.StreamCell>>.Cons(list.Element.Value.Next, list.Next);
        }

        private static Stuff Exec2(Stuff x)
        {
            return new Stuff(x.ElementStream, Exec1(Exec1(x.Schedule)));
        }

        private static readonly Lazy<Stream<T>.StreamCell> EmptyStream = new Lazy<Stream<T>.StreamCell>(() => null);

        private static readonly Sortable EmptySortable = new Sortable(0, null);

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

        private static LinkList<Stuff>.List AddSeg(Lazy<Stream<T>.StreamCell> xs, LinkList<Stuff>.List segs, int size, LinkList<Lazy<Stream<T>.StreamCell>>.List rsched)
        {
            if (size % 2 == 0) return LinkList<Stuff>.Cons(new Stuff(xs, LinkList<Lazy<Stream<T>.StreamCell>>.Reverse(rsched)), segs);
            var xsp = segs.Element.ElementStream;
            var xspp = Mrg(xs, xsp);
            var w = LinkList<Lazy<Stream<T>.StreamCell>>.Cons(xspp, rsched);
            return AddSeg(xspp, segs.Next, size / 2, w);
        }

        private static LinkList<Stuff>.List MapExec2(LinkList<Stuff>.List segsp)
        {
            if (segsp == null) return null;
            return LinkList<Stuff>.Cons(Exec2(segsp.Element), MapExec2(segsp.Next));
        }

        public static Sortable Add(T x, Sortable sortable)
        {
            var stream = Stream<T>.Cons(x, EmptyStream);
            var segsp = AddSeg(stream, sortable.Segs, sortable.Size, null);
            return new Sortable(sortable.Size + 1, MapExec2(segsp));
        }

        private static Lazy<Stream<T>.StreamCell> MrgAll(Lazy<Stream<T>.StreamCell> xs, LinkList<Stuff>.List ys)
        {
            if (ys == null) return xs;
            var xsp = ys.Element.ElementStream;
            var segs = ys.Next;
            return MrgAll(Mrg(xs, xsp), segs);
        }

        private static LinkList<T>.List StreamToList(Lazy<Stream<T>.StreamCell> xs)
        {
            if (xs == EmptyStream) return null;
            return LinkList<T>.Cons(xs.Value.Element, StreamToList(xs.Value.Next));
        }

        public static LinkList<T>.List Sort(Sortable sortable)
        {
            return StreamToList(MrgAll(EmptyStream, sortable.Segs));
        }
    }
}