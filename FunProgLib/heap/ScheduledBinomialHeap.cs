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

    using FunProgLib.lists;
    using FunProgLib.streams;

    public static class ScheduledBinomialHeap<T> where T : IComparable<T>
    {
        public sealed class Tree
        {
            private readonly T node;

            private readonly List<Tree>.Node treeList;

            public Tree(T node, List<Tree>.Node treeList)
            {
                this.node = node;
                this.treeList = treeList;
            }

            public T Node
            {
                get { return this.node; }
            }

            public List<Tree>.Node TreeList
            {
                get { return this.treeList; }
            }
        }

        public sealed class Digit
        {
            private readonly Tree one;

            public Digit(Tree tree)
            {
                this.one = tree;
            }

            public Tree One
            {
                get { return this.one; }
            }
        }

        private readonly static Digit Zero = new Digit(null);

        public sealed class Schedule
        {
            private readonly List<Lazy<Stream<Digit>.StreamCell>>.Node digitStreamList;

            public Schedule(List<Lazy<Stream<Digit>.StreamCell>>.Node digitStreamList)
            {
                this.digitStreamList = digitStreamList;
            }

            public List<Lazy<Stream<Digit>.StreamCell>>.Node DigitStreamList
            {
                get { return this.digitStreamList; }
            }
        }

        public sealed class Heap
        {
            private readonly Lazy<Stream<Digit>.StreamCell> digitStream;

            private readonly Schedule schedule;

            public Heap(Lazy<Stream<Digit>.StreamCell> ds, Schedule schedule)
            {
                this.digitStream = ds;
                this.schedule = schedule;
            }

            public Lazy<Stream<Digit>.StreamCell> DigitStream
            {
                get { return this.digitStream; }
            }

            public Schedule Schedule
            {
                get { return this.schedule; }
            }
        }

        private static readonly Schedule EmptySchedule = new Schedule(null);

        public static readonly Lazy<Stream<Digit>.StreamCell> EmptyStream = Stream<Digit>.DollarNil;

        private static readonly Heap EmptyHeap = new Heap(EmptyStream, EmptySchedule);

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

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
            if (dsc == Stream<Digit>.DollarNil) return Stream<Digit>.DollarCons(new Digit(t), EmptyStream);
            if (dsc.Value.X == Zero) return Stream<Digit>.DollarCons(new Digit(t), dsc.Value.S);
            return Stream<Digit>.DollarCons(Zero, InsTree(Link(t, dsc.Value.X.One), dsc.Value.S));
        }

        private static Lazy<Stream<Digit>.StreamCell> Mrg(Lazy<Stream<Digit>.StreamCell> d1, Lazy<Stream<Digit>.StreamCell> d2)
        {
            if (d2 == Stream<Digit>.DollarNil) return d1;
            if (d1 == Stream<Digit>.DollarNil) return d2;
            if (d1.Value.X == Zero) return Stream<Digit>.DollarCons(d2.Value.X, Mrg(d1.Value.S, d2.Value.S));
            if (d2.Value.X == Zero) return Stream<Digit>.DollarCons(d1.Value.X, Mrg(d1.Value.S, d2.Value.S));
            return Stream<Digit>.DollarCons(Zero, InsTree(Link(d1.Value.X.One, d2.Value.X.One), Mrg(d1.Value.S, d2.Value.S)));
        }

        private static Lazy<Stream<Digit>.StreamCell> Normalize(Lazy<Stream<Digit>.StreamCell> ds)
        {
            if (ds == Stream<Digit>.DollarNil) return ds;
            Normalize(ds.Value.S);
            return ds;
        }

        private static List<Lazy<Stream<Digit>.StreamCell>>.Node Exec(List<Lazy<Stream<Digit>.StreamCell>>.Node list)
        {
            if (list == null) return null;
            if (list.Element.Value != null && list.Element.Value.X == Zero) return List<Lazy<Stream<Digit>.StreamCell>>.Cons(list.Element.Value.S, list.Next);
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
            private readonly Tree tree;

            private readonly Lazy<Stream<Digit>.StreamCell> stream;

            public Stuff(Tree tree, Lazy<Stream<Digit>.StreamCell> stream)
            {
                this.tree = tree;
                this.stream = stream;
            }

            public Tree Tree
            {
                get { return this.tree; }
            }

            public Lazy<Stream<Digit>.StreamCell> Stream
            {
                get { return this.stream; }
            }
        }

        private static Stuff RemoveMinTree(Lazy<Stream<Digit>.StreamCell> dsc)
        {
            if (dsc == Stream<Digit>.DollarNil) throw new Exception("Empty");
            if (dsc.Value.X == Zero)
            {
                var stuff = RemoveMinTree(dsc.Value.S);
                var lazy3 = Stream<Digit>.DollarCons(Zero, stuff.Stream);
                return new Stuff(stuff.Tree, lazy3);
            }
            if (dsc.Value.S == EmptyStream) return new Stuff(dsc.Value.X.One, EmptyStream);
            var tp = RemoveMinTree(dsc.Value.S);
            if (dsc.Value.X.One.Node.CompareTo(tp.Tree.Node) <= 0)
            {
                var lazy = Stream<Digit>.DollarCons(Zero, dsc.Value.S);
                return new Stuff(dsc.Value.X.One, lazy);
            }
            var lazy2 = Stream<Digit>.DollarCons(new Digit(dsc.Value.X.One), tp.Stream);
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
            return Stream<Digit>.DollarCons(new Digit(list.Element), OneMap(list.Next));
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