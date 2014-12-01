// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.2 Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 20-24. Print.

namespace FunProgLib.heap
{
    using System;

    using FunProgLib.lists;

    public static class BinomialHeap<T> where T : IComparable<T>
    {
        public sealed class Tree
        {
            private readonly int rank;

            private readonly T root;

            private readonly List<Tree>.Node list;

            public Tree(int rank, T root, List<Tree>.Node list)
            {
                this.rank = rank;
                this.root = root;
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

            public List<Tree>.Node List
            {
                get { return list; }
            }
        }

        private static readonly List<Tree>.Node EmptyList = null; // new List<Tree>.Node(EmptyList, null);

        public static List<Tree>.Node Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmapty(List<Tree>.Node list)
        {
            return list == EmptyList;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, List<Tree>.Cons(t2, t1.List));
            return new Tree(t1.Rank + 1, t2.Root, List<Tree>.Cons(t1, t2.List));
        }

        private static List<Tree>.Node InsertTree(Tree t, List<Tree>.Node ts)
        {
            if (ts == EmptyList) return new List<Tree>.Node(t, EmptyList);
            if (t.Rank < ts.Element.Rank) return List<Tree>.Cons(t, ts);
            return InsertTree(Link(t, ts.Element), ts.Next);
        }

        public static List<Tree>.Node Insert(T x, List<Tree>.Node ts)
        {
            return InsertTree(new Tree(0, x, EmptyList), ts);
        }

        public static List<Tree>.Node Merge(List<Tree>.Node ts1, List<Tree>.Node ts2)
        {
            if (ts2 == EmptyList) return ts1;
            if (ts1 == EmptyList) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return List<Tree>.Cons(ts1.Element, Merge(ts1.Next, ts2));
            if (ts2.Element.Rank < ts1.Element.Rank) return List<Tree>.Cons(ts2.Element, Merge(ts1, ts2.Next));
            return InsertTree(Link(ts1.Element, ts2.Element), Merge(ts1.Next, ts2.Next));
        }

        private class TreeParts
        {
            private readonly Tree tree;

            private readonly List<Tree>.Node list;

            public TreeParts(Tree tree, List<Tree>.Node list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return tree; }
            }

            public List<Tree>.Node List
            {
                get { return list; }
            }
        }

        private static TreeParts RemoveMinTree(List<Tree>.Node list)
        {
            if (list == EmptyList) throw new Exception("Empty");
            if (list.Next == EmptyList) return new TreeParts(list.Element, EmptyList);
            var prime = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(prime.Tree.Root) <= 0) return new TreeParts(list.Element, list.Next);
            return new TreeParts(prime.Tree, List<Tree>.Cons(list.Element, prime.List));
        }

        public static T FindMin(List<Tree>.Node ts)
        {
            var t = RemoveMinTree(ts);
            return t.Tree.Root;
        }

        public static List<Tree>.Node DeleteMin(List<Tree>.Node ts)
        {
            var t = RemoveMinTree(ts);
            var x = List<Tree>.Reverse(t.Tree.List);
            return Merge(x, t.List);
        }
    }
}