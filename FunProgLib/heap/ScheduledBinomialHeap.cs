// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "7.3 Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 89-93. Print.

namespace FunProgLib.heap
{
    using System;

    using lists;
    using streams;

    public static class ScheduledBinomialHeap<T> where T : IComparable<T>
    {
        public sealed class Tree
        {
            public Tree(T node, List<Tree>.Node treeList)
            {
                Node = node;
                TreeList = treeList;
            }

            public T Node { get; }

            public List<Tree>.Node TreeList { get; }
        }

        public sealed class Digit
        {
            public Digit(Tree tree)
            {
                One = tree;
            }

            public Tree One { get; }
        }

        private static readonly Digit Zero = new Digit(null);

        public sealed class Schedule
        {
            public Schedule(List<Lazy<Stream<Digit>.StreamCell>>.Node digitStreamList)
            {
                DigitStreamList = digitStreamList;
            }

            public List<Lazy<Stream<Digit>.StreamCell>>.Node DigitStreamList { get; }
        }

        public sealed class Heap
        {
            public Heap(Lazy<Stream<Digit>.StreamCell> ds, Schedule schedule)
            {
                DigitStream = ds;
                Schedule = schedule;
            }

            public Lazy<Stream<Digit>.StreamCell> DigitStream { get; }

            public Schedule Schedule { get; }
        }

        private static readonly Schedule EmptySchedule = new Schedule(null);

        public static readonly Lazy<Stream<Digit>.StreamCell> EmptyStream = Stream<Digit>.DollarNil;

        public static Heap Empty { get; } = new Heap(EmptyStream, EmptySchedule);

        public static bool IsEmpty(Heap heap)
        {
            return heap.DigitStream == Stream<Digit>.DollarNil;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Node.CompareTo(t2.Node) <= 0)
                return new Tree(t1.Node, List<Tree>.Cons(t2, t1.TreeList));
            return new Tree(t2.Node, List<Tree>.Cons(t1, t2.TreeList));
        }

        private static Lazy<Stream<Digit>.StreamCell> InsTree(Tree t, Lazy<Stream<Digit>.StreamCell> dsc)
        {
            if (dsc == Stream<Digit>.DollarNil) return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(new Digit(t), EmptyStream));
            if (dsc.Value.Element == Zero) return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(new Digit(t), dsc.Value.Next));
            return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(Zero, InsTree(Link(t, dsc.Value.Element.One), dsc.Value.Next)));
        }

        private static Lazy<Stream<Digit>.StreamCell> Mrg(Lazy<Stream<Digit>.StreamCell> d1, Lazy<Stream<Digit>.StreamCell> d2)
        {
            if (d2 == Stream<Digit>.DollarNil) return d1;
            if (d1 == Stream<Digit>.DollarNil) return d2;
            if (d1.Value.Element == Zero) return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(d2.Value.Element, Mrg(d1.Value.Next, d2.Value.Next)));
            if (d2.Value.Element == Zero) return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(d1.Value.Element, Mrg(d1.Value.Next, d2.Value.Next)));
            return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(Zero, InsTree(Link(d1.Value.Element.One, d2.Value.Element.One), Mrg(d1.Value.Next, d2.Value.Next))));
        }

        private static Lazy<Stream<Digit>.StreamCell> Normalize(Lazy<Stream<Digit>.StreamCell> ds)
        {
            if (ds == Stream<Digit>.DollarNil) return ds;
            Normalize(ds.Value.Next);
            return ds;
        }

        private static List<Lazy<Stream<Digit>.StreamCell>>.Node Exec(List<Lazy<Stream<Digit>.StreamCell>>.Node list)
        {
            if (list == null) return null;
            if (list.Element.Value != null && list.Element.Value.Element == Zero) return List<Lazy<Stream<Digit>.StreamCell>>.Cons(list.Element.Value.Next, list.Next);
            return list.Next;
        }

        public static Heap Insert(T x, Heap heap)
        {
            var dsp = InsTree(new Tree(x, null), heap.DigitStream);
            var list = List<Lazy<Stream<Digit>.StreamCell>>.Cons(dsp, heap.Schedule.DigitStreamList);
            return new Heap(dsp, new Schedule(Exec(Exec(list))));
        }

        public static Heap Merge(Heap ts1, Heap ts2)
        {
            var ds = Normalize(Mrg(ts1.DigitStream, ts2.DigitStream));
            return new Heap(ds, null);
        }

        private sealed class Stuff
        {
            public Stuff(Tree tree, Lazy<Stream<Digit>.StreamCell> stream)
            {
                Tree = tree;
                Stream = stream;
            }

            public Tree Tree { get; }

            public Lazy<Stream<Digit>.StreamCell> Stream { get; }
        }

        private static Stuff RemoveMinTree(Lazy<Stream<Digit>.StreamCell> dsc)
        {
            if (dsc == Stream<Digit>.DollarNil) throw new ArgumentException("Empty", nameof(dsc));
            if (dsc.Value.Element == Zero)
            {
                var stuff = RemoveMinTree(dsc.Value.Next);
                var lazy3 = new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(Zero, stuff.Stream));
                return new Stuff(stuff.Tree, lazy3);
            }
            if (dsc.Value.Next == EmptyStream) return new Stuff(dsc.Value.Element.One, EmptyStream);
            var tp = RemoveMinTree(dsc.Value.Next);
            if (dsc.Value.Element.One.Node.CompareTo(tp.Tree.Node) <= 0)
            {
                var lazy = new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(Zero, dsc.Value.Next));
                return new Stuff(dsc.Value.Element.One, lazy);
            }
            var lazy2 = new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(new Digit(dsc.Value.Element.One), tp.Stream));
            return new Stuff(tp.Tree, lazy2);
        }

        public static T FindMin(Heap heap)
        {
            var stuff = RemoveMinTree(heap.DigitStream);
            return stuff.Tree.Node;
        }

        private static Lazy<Stream<Digit>.StreamCell> OneMap(List<Tree>.Node list)
        {
            if (list == null) return EmptyStream;
            return new Lazy<Stream<Digit>.StreamCell>(() => new Stream<Digit>.StreamCell(new Digit(list.Element), OneMap(list.Next)));
        }

        public static Heap DeleteMin(Heap heap)
        {
            var stuff = RemoveMinTree(heap.DigitStream);
            var reverse = List<Tree>.Reverse(stuff.Tree.TreeList);
            var map = OneMap(reverse);
            var dspp = Mrg(map, stuff.Stream);
            return new Heap(Normalize(dspp), EmptySchedule);
        }
    }
}