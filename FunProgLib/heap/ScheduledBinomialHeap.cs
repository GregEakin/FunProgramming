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

using System;
using System.Threading.Tasks;
using FunProgLib.lists;
using FunProgLib.streams;

namespace FunProgLib.heap
{
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
                get { return node; }
            }

            public List<Tree>.Node TreeList
            {
                get { return treeList; }
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
                get { return one; }
            }
        }

        private readonly static Digit Zero = new Digit(null);

        public sealed class Schedule
        {
            private readonly List<Lazy<Task<Stream<Digit>.StreamCell>>>.Node digitStreamList;

            public Schedule(List<Lazy<Task<Stream<Digit>.StreamCell>>>.Node digitStreamList)
            {
                this.digitStreamList = digitStreamList;
            }

            public List<Lazy<Task<Stream<Digit>.StreamCell>>>.Node DigitStreamList
            {
                get { return digitStreamList; }
            }
        }

        public sealed class Heap
        {
            private readonly Lazy<Task<Stream<Digit>.StreamCell>> digitStream;

            private readonly Schedule schedule;

            public Heap(Lazy<Task<Stream<Digit>.StreamCell>> ds, Schedule schedule)
            {
                this.digitStream = ds;
                this.schedule = schedule;
            }

            public Lazy<Task<Stream<Digit>.StreamCell>> DigitStream
            {
                get { return digitStream; }
            }

            public Schedule Schedule
            {
                get { return schedule; }
            }
        }

        private static readonly Schedule EmptySchedule = new Schedule(null);

        public static readonly Lazy<Task<Stream<Digit>.StreamCell>> EmptyStream = Stream<Digit>.DollarNil;

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

        private static Lazy<Task<Stream<Digit>.StreamCell>> InsTree(Tree t, Lazy<Task<Stream<Digit>.StreamCell>> dsc)
        {
            if (dsc == Stream<Digit>.DollarNil) return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(new Digit(t), EmptyStream)));
            if (dsc.Value.Result.Element == Zero) return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(new Digit(t), dsc.Value.Result.Next)));
            return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(Zero, InsTree(Link(t, dsc.Value.Result.Element.One), dsc.Value.Result.Next))));
        }

        private static Lazy<Task<Stream<Digit>.StreamCell>> Mrg(Lazy<Task<Stream<Digit>.StreamCell>> d1, Lazy<Task<Stream<Digit>.StreamCell>> d2)
        {
            if (d2 == Stream<Digit>.DollarNil) return d1;
            if (d1 == Stream<Digit>.DollarNil) return d2;
            if (d1.Value.Result.Element == Zero) return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(d2.Value.Result.Element, Mrg(d1.Value.Result.Next, d2.Value.Result.Next))));
            if (d2.Value.Result.Element == Zero) return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(d1.Value.Result.Element, Mrg(d1.Value.Result.Next, d2.Value.Result.Next))));
            return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(Zero, InsTree(Link(d1.Value.Result.Element.One, d2.Value.Result.Element.One), Mrg(d1.Value.Result.Next, d2.Value.Result.Next)))));
        }

        private static Lazy<Task<Stream<Digit>.StreamCell>> Normalize(Lazy<Task<Stream<Digit>.StreamCell>> ds)
        {
            if (ds == Stream<Digit>.DollarNil) return ds;
            Normalize(ds.Value.Result.Next);
            return ds;
        }

        private static List<Lazy<Task<Stream<Digit>.StreamCell>>>.Node Exec(List<Lazy<Task<Stream<Digit>.StreamCell>>>.Node list)
        {
            if (list == null) return null;
            if (list.Element.Value != null && list.Element.Value.Result.Element == Zero) return List<Lazy<Task<Stream<Digit>.StreamCell>>>.Cons(list.Element.Value.Result.Next, list.Next);
            return list.Next;
        }

        public static Heap Insert(T x, Heap heap)
        {
            var dsp = InsTree(new Tree(x, null), heap.DigitStream);
            var list = List<Lazy<Task<Stream<Digit>.StreamCell>>>.Cons(dsp, heap.Schedule.DigitStreamList);
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

            private readonly Lazy<Task<Stream<Digit>.StreamCell>> stream;

            public Stuff(Tree tree, Lazy<Task<Stream<Digit>.StreamCell>> stream)
            {
                this.tree = tree;
                this.stream = stream;
            }

            public Tree Tree
            {
                get { return tree; }
            }

            public Lazy<Task<Stream<Digit>.StreamCell>> Stream
            {
                get { return stream; }
            }
        }

        private static Stuff RemoveMinTree(Lazy<Task<Stream<Digit>.StreamCell>> dsc)
        {
            if (dsc == Stream<Digit>.DollarNil) throw new Exception("Empty");
            if (dsc.Value.Result.Element == Zero)
            {
                var stuff = RemoveMinTree(dsc.Value.Result.Next);
                var lazy3 = new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(Zero, stuff.Stream)));
                return new Stuff(stuff.Tree, lazy3);
            }
            if (dsc.Value.Result.Next == EmptyStream) return new Stuff(dsc.Value.Result.Element.One, EmptyStream);
            var tp = RemoveMinTree(dsc.Value.Result.Next);
            if (dsc.Value.Result.Element.One.Node.CompareTo(tp.Tree.Node) <= 0)
            {
                var lazy = new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(Zero, dsc.Value.Result.Next)));
                return new Stuff(dsc.Value.Result.Element.One, lazy);
            }
            var lazy2 = new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(new Digit(dsc.Value.Result.Element.One), tp.Stream)));
            return new Stuff(tp.Tree, lazy2);
        }

        public static T FindMin(Heap heap)
        {
            var stuff = RemoveMinTree(heap.DigitStream);
            return stuff.Tree.Node;
        }

        private static Lazy<Task<Stream<Digit>.StreamCell>> OneMap(List<Tree>.Node list)
        {
            if (list == null) return EmptyStream;
            return new Lazy<Task<Stream<Digit>.StreamCell>>(() => Task.Factory.StartNew(() => new Stream<Digit>.StreamCell(new Digit(list.Element), OneMap(list.Next))));
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