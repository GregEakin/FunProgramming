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
    using lists;
    using streams;

    public static class ScheduledBottomUpMergeSort<T> where T : IComparable<T>
    {
        public sealed class Schedule
        {
            public Schedule(Lazy<Stream<T>.StreamCell> stream, List<Lazy<Stream<T>.StreamCell>>.Node scheduleList)
            {
                Stream = stream;
                ScheduleList = scheduleList;
            }

            public Lazy<Stream<T>.StreamCell> Stream { get; }

            public List<Lazy<Stream<T>.StreamCell>>.Node ScheduleList { get; }
        }

        public sealed class Sortable
        {
            public Sortable(int size, List<Schedule>.Node segs)
            {
                Size = size;
                Segs = segs;
            }

            public int Size { get; }

            public List<Schedule>.Node Segs { get; }
        }

        private static Lazy<Stream<T>.StreamCell> Mrg(Lazy<Stream<T>.StreamCell> xs, Lazy<Stream<T>.StreamCell> ys)
        {
            if (xs == Stream<T>.DollarNil) return ys;
            if (ys == Stream<T>.DollarNil) return xs;
            if (xs.Value.Element.CompareTo(ys.Value.Element) <= 0)
                return Stream<T>.DollarCons(xs.Value.Element, Mrg(xs.Value.Next, ys));
            return Stream<T>.DollarCons(ys.Value.Element, Mrg(xs, ys.Value.Next));
        }

        private static List<Lazy<Stream<T>.StreamCell>>.Node Exec1(List<Lazy<Stream<T>.StreamCell>>.Node list)
        {
            if (list == null) return null;
            if (list.Element == Stream<T>.DollarNil) return Exec1(list.Next);
            return List<Lazy<Stream<T>.StreamCell>>.Cons(list.Element.Value.Next, list.Next);
        }

        private static Schedule Exec2(Schedule x) => new Schedule(x.Stream, Exec1(Exec1(x.ScheduleList)));

        public static Sortable Empty { get; } = new Sortable(0, null);

        private static List<Schedule>.Node AddSeg(Lazy<Stream<T>.StreamCell> xs, List<Schedule>.Node segs, int size, List<Lazy<Stream<T>.StreamCell>>.Node rsched)
        {
            if (size % 2 == 0) return List<Schedule>.Cons(new Schedule(xs, List<Lazy<Stream<T>.StreamCell>>.Reverse(rsched)), segs);
            var xsp = segs.Element.Stream;
            var xspp = Mrg(xs, xsp);
            var w = List<Lazy<Stream<T>.StreamCell>>.Cons(xspp, rsched);
            return AddSeg(xspp, segs.Next, size / 2, w);
        }

        private static List<Schedule>.Node MapExec2(List<Schedule>.Node segsp)
        {
            if (segsp == null) return null;
            return List<Schedule>.Cons(Exec2(segsp.Element), MapExec2(segsp.Next));
        }

        public static Sortable Add(T x, Sortable sortable)
        {
            var stream = Stream<T>.DollarCons(x, Stream<T>.DollarNil);
            var segsp = AddSeg(stream, sortable.Segs, sortable.Size, null);
            return new Sortable(sortable.Size + 1, MapExec2(segsp));
        }

        private static Lazy<Stream<T>.StreamCell> MrgAll(Lazy<Stream<T>.StreamCell> xs, List<Schedule>.Node ys)
        {
            if (ys == null) return xs;
            var xsp = ys.Element.Stream;
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
