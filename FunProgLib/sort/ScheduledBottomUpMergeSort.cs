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

using System;
using System.Threading.Tasks;
using FunProgLib.lists;
using FunProgLib.streams;

namespace FunProgLib.sort
{
    public static class ScheduledBottomUpMergeSort<T> where T : IComparable<T>
    {
        public sealed class Stuff
        {
            private readonly Lazy<Task<Stream<T>.StreamCell>> elementStream;

            private readonly List<Lazy<Task<Stream<T>.StreamCell>>>.Node schedule;

            public Stuff(Lazy<Task<Stream<T>.StreamCell>> elementStream, List<Lazy<Task<Stream<T>.StreamCell>>>.Node schedule)
            {
                this.elementStream = elementStream;
                this.schedule = schedule;
            }

            public Lazy<Task<Stream<T>.StreamCell>> ElementStream
            {
                get { return elementStream; }
            }

            public List<Lazy<Task<Stream<T>.StreamCell>>>.Node Schedule
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
                get { return size; }
            }

            public List<Stuff>.Node Segs
            {
                get { return segs; }
            }
        }

        private static Lazy<Task<Stream<T>.StreamCell>> Mrg(Lazy<Task<Stream<T>.StreamCell>> xs, Lazy<Task<Stream<T>.StreamCell>> ys)
        {
            if (xs == Stream<T>.DollarNil) return ys;
            if (ys == Stream<T>.DollarNil) return xs;
            if (xs.Value.Result.Element.CompareTo(ys.Value.Result.Element) <= 0) return new Lazy<Task<Stream<T>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<T>.StreamCell(xs.Value.Result.Element, Mrg(xs.Value.Result.Next, ys))));
            return new Lazy<Task<Stream<T>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<T>.StreamCell(ys.Value.Result.Element, Mrg(xs, ys.Value.Result.Next))));
        }

        private static List<Lazy<Task<Stream<T>.StreamCell>>>.Node Exec1(List<Lazy<Task<Stream<T>.StreamCell>>>.Node list)
        {
            if (list == null) return null;
            if (list.Element == Stream<T>.DollarNil) return Exec1(list.Next);
            return List<Lazy<Task<Stream<T>.StreamCell>>>.Cons(list.Element.Value.Result.Next, list.Next);
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

        private static List<Stuff>.Node AddSeg(Lazy<Task<Stream<T>.StreamCell>> xs, List<Stuff>.Node segs, int size, List<Lazy<Task<Stream<T>.StreamCell>>>.Node rsched)
        {
            if (size % 2 == 0) return List<Stuff>.Cons(new Stuff(xs, List<Lazy<Task<Stream<T>.StreamCell>>>.Reverse(rsched)), segs);
            var xsp = segs.Element.ElementStream;
            var xspp = Mrg(xs, xsp);
            var w = List<Lazy<Task<Stream<T>.StreamCell>>>.Cons(xspp, rsched);
            return AddSeg(xspp, segs.Next, size / 2, w);
        }

        private static List<Stuff>.Node MapExec2(List<Stuff>.Node segsp)
        {
            if (segsp == null) return null;
            return List<Stuff>.Cons(Exec2(segsp.Element), MapExec2(segsp.Next));
        }

        public static Sortable Add(T x, Sortable sortable)
        {
            var stream = new Lazy<Task<Stream<T>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<T>.StreamCell(x, Stream<T>.DollarNil)));
            var segsp = AddSeg(stream, sortable.Segs, sortable.Size, null);
            return new Sortable(sortable.Size + 1, MapExec2(segsp));
        }

        private static Lazy<Task<Stream<T>.StreamCell>> MrgAll(Lazy<Task<Stream<T>.StreamCell>> xs, List<Stuff>.Node ys)
        {
            if (ys == null) return xs;
            var xsp = ys.Element.ElementStream;
            var segs = ys.Next;
            return MrgAll(Mrg(xs, xsp), segs);
        }

        private static List<T>.Node StreamToList(Lazy<Task<Stream<T>.StreamCell>> xs)
        {
            if (xs == Stream<T>.DollarNil) return null;
            return List<T>.Cons(xs.Value.Result.Element, StreamToList(xs.Value.Result.Next));
        }

        public static List<T>.Node Sort(Sortable sortable)
        {
            return StreamToList(MrgAll(Stream<T>.DollarNil, sortable.Segs));
        }
    }
}