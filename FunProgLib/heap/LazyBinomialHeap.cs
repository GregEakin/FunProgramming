// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LazyBinomialHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.heap
{
    using System;

    using FunProgLib.persistence;

    public static class LazyBinomialHeap<T> where T : IComparable
    {
        public class Tree
        {
            private readonly int rank;

            private readonly T root;

            private readonly LinkList<Tree>.List list;

            public Tree(int rank, T x, LinkList<Tree>.List list)
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

            public LinkList<Tree>.List List
            {
                get { return list; }
            }
        }

        // type Heap = Lazy<LinkList<Tree>.List /* susp */

        private static readonly LinkList<Tree>.List EmptyList = LinkList<Tree>.Empty;

        private static readonly Lazy<LinkList<Tree>.List> EmptyHeap = /* $ */ new Lazy<LinkList<Tree>.List>(() => EmptyList);

        public static Lazy<LinkList<Tree>.List> Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmapty(/* $ */ Lazy<LinkList<Tree>.List> heap)
        {
            return heap.Value == EmptyList;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, LinkList<Tree>.Cons(t2, t1.List));
            return new Tree(t1.Rank + 1, t2.Root, LinkList<Tree>.Cons(t1, t2.List));
        }

        private static LinkList<Tree>.List InsTree(Tree t, LinkList<Tree>.List ts)
        {
            if (ts == EmptyList) return LinkList<Tree>.Cons(t, EmptyList);
            if (t.Rank < ts.Element.Rank) return LinkList<Tree>.Cons(t, ts);
            return InsTree(Link(t, ts.Element), ts.Next);
        }

        private static LinkList<Tree>.List Mrg(LinkList<Tree>.List ts1, LinkList<Tree>.List ts2)
        {
            if (ts2 == EmptyList) return ts1;
            if (ts1 == EmptyList) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return LinkList<Tree>.Cons(ts1.Element, Mrg(ts1.Next, ts2));
            if (ts2.Element.Rank < ts1.Element.Rank) return LinkList<Tree>.Cons(ts2.Element, Mrg(ts1, ts2.Next));
            return InsTree(Link(ts1.Element, ts2.Element), Mrg(ts1.Next, ts2.Next));
        }

        public static /* lazy */ Lazy<LinkList<Tree>.List> Insert(T x, /* $ */ Lazy<LinkList<Tree>.List> ts)
        {
            return /* $ */ new Lazy<LinkList<Tree>.List>(() => InsTree(new Tree(0, x, EmptyList), ts.Value));
        }

        public static /* lazy */ Lazy<LinkList<Tree>.List> Merge(/* $ */ Lazy<LinkList<Tree>.List> ts1, /* $ */ Lazy<LinkList<Tree>.List> ts2)
        {
            return /* $ */ new Lazy<LinkList<Tree>.List>(() => Mrg(ts1.Value, ts2.Value));
        }

        private class TreeParts
        {
            private readonly Tree tree;

            private readonly LinkList<Tree>.List list;

            public TreeParts(Tree tree, LinkList<Tree>.List list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return this.tree; }
            }

            public LinkList<Tree>.List List
            {
                get { return this.list; }
            }
        }

        private static TreeParts RemoveMinTree(LinkList<Tree>.List list)
        {
            if (list == EmptyList) throw new Exception("Empty");
            if (list.Next == EmptyList) return new TreeParts(list.Element, EmptyList);
            var prime = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(prime.Tree.Root) <= 0) return new TreeParts(list.Element, list.Next);
            return new TreeParts(prime.Tree, LinkList<Tree>.Cons(list.Element, prime.List));
        }

        public static T FindMin(/* $ */ Lazy<LinkList<Tree>.List> ts)
        {
            var t = RemoveMinTree(ts.Value);
            return t.Tree.Root;

        }

        public static /* lazy */ Lazy<LinkList<Tree>.List> DeleteMin(/* $ */ Lazy<LinkList<Tree>.List> ts)
        {
            var t = RemoveMinTree(ts.Value);
            var x = LinkList<Tree>.Reverse(t.Tree.List);
            return /* $ */ new Lazy<LinkList<Tree>.List>(() => Mrg(x, t.List));
        }
    }
}