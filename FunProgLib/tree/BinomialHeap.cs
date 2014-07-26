// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinomialHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class BinomialHeap<T> where T : IComparable
    {
        public class Node
        {
            private readonly int rank;

            private readonly T root;

            private readonly ReadOnlyCollection<Node> list;

            public Node(int rank, T x, ReadOnlyCollection<Node> list)
            {
                this.rank = rank;
                this.root = x;
                this.list = list;
            }

            public int Rank
            {
                get { return rank; }
            }

            public T Root
            {
                get { return root; }
            }

            public IEnumerable<Node> List
            {
                get { return list; }
            }
        }

        private static readonly ReadOnlyCollection<Node> EmptyList = new ReadOnlyCollection<Node>(new Node[0]);

        public static ReadOnlyCollection<Node> Empty
        {
            get
            {
                return EmptyList;
            }
        }

        public static bool IsEmapty(ReadOnlyCollection<Node> list)
        {
            return list == EmptyList;
        }

        private static ReadOnlyCollection<Node> Concatenate(Node element, IEnumerable<Node> list)
        {
            var x = list.ToList();
            x.Insert(0, element);
            return x.AsReadOnly();
        }

        private static Node Link(Node t1, Node t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Node(t1.Rank + 1, t1.Root, Concatenate(t2, t1.List));
            return new Node(t1.Rank + 1, t2.Root, Concatenate(t1, t2.List));
        }

        private static ReadOnlyCollection<Node> InsertTree(Node t, IReadOnlyList<Node> ts)
        {
            if (ts.Count == 0) return new ReadOnlyCollection<Node>(new[] { t });
            var tp = ts[0];
            var tsp = ts.Skip(1).ToList().AsReadOnly();
            if (t.Rank < tp.Rank) return Concatenate(t, ts);
            return InsertTree(Link(t, tp), tsp);
        }

        public static ReadOnlyCollection<Node> Insert(T x, ReadOnlyCollection<Node> ts)
        {
            return InsertTree(new Node(0, x, EmptyList), ts);
        }

        private static ReadOnlyCollection<Node> Merge(ReadOnlyCollection<Node> ts1, ReadOnlyCollection<Node> ts2)
        {
            if (ts2.Count == 0) return ts1;
            if (ts1.Count == 0) return ts2;

            var t1 = ts1[0];
            var tsp1 = ts1.Skip(1).ToList().AsReadOnly();

            var t2 = ts2[0];
            var tsp2 = ts2.Skip(1).ToList().AsReadOnly();

            if (t1.Rank < t2.Rank) return Concatenate(t1, Merge(tsp1, ts2));
            if (t2.Rank < t1.Rank) return Concatenate(t2, Merge(ts1, tsp2));
            return InsertTree(Link(t1, t2), Merge(tsp1, tsp2));
        }

        private class Stuff
        {
            public Node Node { get; set; }

            public ReadOnlyCollection<Node> List { get; set; }
        }

        private static Stuff RemoveMinTree(IReadOnlyList<Node> list)
        {
            if (list.Count == 0) throw new Exception("Empty");
            if (list.Count == 1) return new Stuff { Node = list[0], List = EmptyList };
            var t = list[0];
            var ts = list.Skip(1).ToList().AsReadOnly();
            var prime = RemoveMinTree(ts);
            if (t.Root.CompareTo(prime.Node.Root) <= 0) return new Stuff { Node = t, List = ts };
            return new Stuff { Node = prime.Node, List = Concatenate(t, prime.List) };
        }

        public static T FindMin(ReadOnlyCollection<Node> ts)
        {
            var t = RemoveMinTree(ts);
            return t.Node.Root;
        }

        public static ReadOnlyCollection<Node> DeleteMin(ReadOnlyCollection<Node> ts)
        {
            var t = RemoveMinTree(ts);
            var x = t.Node.List.Reverse().ToList().AsReadOnly();
            return Merge(x, t.List);
        }
    }
}