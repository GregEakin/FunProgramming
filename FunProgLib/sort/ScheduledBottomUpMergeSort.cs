// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "7.4 Bottom-Up Mergesrot with Sharing." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 94-97. Print.

namespace FunProgLib.sort
{
    using System;
    using FunProgLib.lists;
    using FunProgLib.streams;

    public static class ScheduledBottomUpMergeSort<T> where T : IComparable<T>
    {
        public sealed class Stuff
        {
            private readonly Lazy<Stream<T>.StreamCell> elementStream;

            private readonly List<Lazy<Stream<T>.StreamCell>>.Node schedule;

            public Stuff(Lazy<Stream<T>.StreamCell> elementStream, List<Lazy<Stream<T>.StreamCell>>.Node schedule)
            {
                this.elementStream = elementStream;
                this.schedule = schedule;
            }

            public Lazy<Stream<T>.StreamCell> ElementStream
            {
                get { return this.elementStream; }
            }

            public List<Lazy<Stream<T>.StreamCell>>.Node Schedule
            {
                get { return schedule; }
            }
        }

        public sealed class Sortable
        {
            private readonly int size;

            private readonly List<Stuff>.Node segs;

            public Sortable(int size, List<Stuff>.Node segs)
            {
                this.size = size;
                this.segs = segs;
            }

            public int Size
            {
                get { return this.size; }
            }

            public List<Stuff>.Node Segs
            {
                get { return segs; }
            }
        }

        private static Lazy<Stream<T>.StreamCell> Mrg(Lazy<Stream<T>.StreamCell> xs, Lazy<Stream<T>.StreamCell> ys)
        {
            if (xs == Stream<T>.DollarNil) return ys;
            if (ys == Stream<T>.DollarNil) return xs;
            if (xs.Value.Element.CompareTo(ys.Value.Element) <= 0) return Stream<T>.DollarCons(xs.Value.Element, Mrg(xs.Value.Next, ys));
            return Stream<T>.DollarCons(ys.Value.Element, Mrg(xs, ys.Value.Next));
        }

        private static List<Lazy<Stream<T>.StreamCell>>.Node Exec1(List<Lazy<Stream<T>.StreamCell>>.Node list)
        {
            if (list == null) return null;
            if (list.Element == Stream<T>.DollarNil) return Exec1(list.Next);
            return List<Lazy<Stream<T>.StreamCell>>.Cons(list.Element.Value.Next, list.Next);
        }

        private static Stuff Exec2(Stuff x)
        {
            return new Stuff(x.ElementStream, Exec1(Exec1(x.Schedule)));
        }

        private static readonly Sortable EmptySortable = new Sortable(0, null);

        public static Sortable Empty
        {
            get { return EmptySortable; }
        }

        private static List<Stuff>.Node AddSeg(Lazy<Stream<T>.StreamCell> xs, List<Stuff>.Node segs, int size, List<Lazy<Stream<T>.StreamCell>>.Node rsched)
        {
            if (size % 2 == 0) return List<Stuff>.Cons(new Stuff(xs, List<Lazy<Stream<T>.StreamCell>>.Reverse(rsched)), segs);
            var xsp = segs.Element.ElementStream;
            var xspp = Mrg(xs, xsp);
            var w = List<Lazy<Stream<T>.StreamCell>>.Cons(xspp, rsched);
            return AddSeg(xspp, segs.Next, size / 2, w);
        }

        private static List<Stuff>.Node MapExec2(List<Stuff>.Node segsp)
        {
            if (segsp == null) return null;
            return List<Stuff>.Cons(Exec2(segsp.Element), MapExec2(segsp.Next));
        }

        public static Sortable Add(T x, Sortable sortable)
        {
            var stream = Stream<T>.DollarCons(x, Stream<T>.DollarNil);
            var segsp = AddSeg(stream, sortable.Segs, sortable.Size, null);
            return new Sortable(sortable.Size + 1, MapExec2(segsp));
        }

        private static Lazy<Stream<T>.StreamCell> MrgAll(Lazy<Stream<T>.StreamCell> xs, List<Stuff>.Node ys)
        {
            if (ys == null) return xs;
            var xsp = ys.Element.ElementStream;
            var segs = ys.Next;
            return MrgAll(Mrg(xs, xsp), segs);
        }

        private static List<T>.Node StreamToList(Lazy<Stream<T>.StreamCell> xs)
        {
            if (xs == Stream<T>.DollarNil) return null;
            return List<T>.Cons(xs.Value.Element, StreamToList(xs.Value.Next));
        }

        public static List<T>.Node Sort(Sortable sortable)
        {
            return StreamToList(MrgAll(Stream<T>.DollarNil, sortable.Segs));
        }
    }
}