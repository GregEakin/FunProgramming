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

        public static readonly Lazy<Stream<Digit>.StreamCell> EmptyStream = new Lazy<Stream<Digit>.StreamCell>(() => null);

        private static readonly Heap EmptyHeap = new Heap(EmptyStream, EmptySchedule);

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmpty(Heap heap)
        {
            return heap.DigitStream == EmptyStream;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Node.CompareTo(t2.Node) <= 0)
                return new Tree(t1.Node, List<Tree>.Cons(t2, t1.TreeList));
            return new Tree(t2.Node, List<Tree>.Cons(t1, t2.TreeList));
        }

        private static Lazy<Stream<Digit>.StreamCell> InsTree(Tree t, Lazy<Stream<Digit>.StreamCell> ds)
        {
            if (ds == EmptyStream) return Stream<Digit>.Cons(new Digit(t), EmptyStream);
            if (ds.Value.Element == Zero) return Stream<Digit>.Cons(new Digit(t), ds.Value.Next);
            return Stream<Digit>.Cons(Zero, InsTree(Link(t, ds.Value.Element.One), ds.Value.Next));
        }

        private static Lazy<Stream<Digit>.StreamCell> Mrg(Lazy<Stream<Digit>.StreamCell> ds1, Lazy<Stream<Digit>.StreamCell> ds2)
        {
            if (ds2 == EmptyStream) return ds1;
            if (ds1 == EmptyStream) return ds2;
            if (ds1.Value.Element == Zero) return Stream<Digit>.Cons(ds2.Value.Element, Mrg(ds1.Value.Next, ds2.Value.Next));
            if (ds2.Value.Element == Zero) return Stream<Digit>.Cons(ds1.Value.Element, Mrg(ds1.Value.Next, ds2.Value.Next));
            return Stream<Digit>.Cons(Zero, InsTree(Link(ds1.Value.Element.One, ds2.Value.Element.One), Mrg(ds1.Value.Next, ds2.Value.Next)));
        }

        private static Lazy<Stream<Digit>.StreamCell> Normalize(Lazy<Stream<Digit>.StreamCell> ds)
        {
            if (ds == EmptyStream) return EmptyStream;
            Normalize(ds.Value.Next);
            return ds;
        }

        private static List<Lazy<Stream<Digit>.StreamCell>>.Node Exec(List<Lazy<Stream<Digit>.StreamCell>>.Node list)
        {
            if (list == null) return null;
            if (list.Element.Value.Element == Zero) return List<Lazy<Stream<Digit>.StreamCell>>.Cons(list.Element.Value.Next, list.Next);
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

        private static Stuff RemoveMinTree(Lazy<Stream<Digit>.StreamCell> ds)
        {
            if (ds == EmptyStream) throw new Exception("Empty");
            if (ds.Value.Element == Zero)
            {
                var stuff = RemoveMinTree(ds.Value.Next);
                return new Stuff(stuff.Tree, Stream<Digit>.Cons(Zero, stuff.Stream));
            }
            if (ds.Value.Next == EmptyStream) return new Stuff(ds.Value.Element.One, EmptyStream);
            var tp = RemoveMinTree(ds.Value.Next);
            if (ds.Value.Element.One.Node.CompareTo(tp.Tree.Node) <= 0) return new Stuff(ds.Value.Element.One, Stream<Digit>.Cons(Zero, ds.Value.Next));
            return new Stuff(tp.Tree, Stream<Digit>.Cons(new Digit(ds.Value.Element.One), tp.Stream));
        }

        public static T FindMin(Heap heap)
        {
            var stuff = RemoveMinTree(heap.DigitStream);
            return stuff.Tree.Node;
        }

        private static Lazy<Stream<Digit>.StreamCell> OneMap(List<Tree>.Node list)
        {
            if (list == null) return EmptyStream;
            return Stream<Digit>.Cons(new Digit(list.Element), OneMap(list.Next));
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