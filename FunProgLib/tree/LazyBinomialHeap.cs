// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		LazyBinomialHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    using FunProgLib.persistence;

    public static class LazyBinomialHeap<T> where T : IComparable
    {
        public class Tree
        {
            private readonly int rank;

            private readonly T root;

            private readonly LinkList<Tree>.ListStructure list;

            public Tree(int rank, T x, LinkList<Tree>.ListStructure list)
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

            public LinkList<Tree>.ListStructure List
            {
                get { return list; }
            }
        }

        // type Heap = Lazy<LinkList<Tree>.ListStructure /* susp */

        private static readonly LinkList<Tree>.ListStructure EmptyList = LinkList<Tree>.Empty;

        private static readonly Lazy<LinkList<Tree>.ListStructure> EmptyHeap = /* $ */ new Lazy<LinkList<Tree>.ListStructure>(() => EmptyList);

        public static Lazy<LinkList<Tree>.ListStructure> Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmapty(/* $ */ Lazy<LinkList<Tree>.ListStructure> heap)
        {
            return heap.Value == EmptyList;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, LinkList<Tree>.Cons(t1.List, t2));
            return new Tree(t1.Rank + 1, t2.Root, LinkList<Tree>.Cons(t2.List, t1));
        }

        private static LinkList<Tree>.ListStructure InsTree(Tree t, LinkList<Tree>.ListStructure ts)
        {
            if (ts == EmptyList) return LinkList<Tree>.Cons(EmptyList, t);
            if (t.Rank < ts.Element.Rank) return LinkList<Tree>.Cons(ts, t);
            return InsTree(Link(t, ts.Element), ts.Next);
        }

        private static LinkList<Tree>.ListStructure Mrg(LinkList<Tree>.ListStructure ts1, LinkList<Tree>.ListStructure ts2)
        {
            if (ts2 == EmptyList) return ts1;
            if (ts1 == EmptyList) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return LinkList<Tree>.Cons(Mrg(ts1.Next, ts2), ts1.Element);
            if (ts2.Element.Rank < ts1.Element.Rank) return LinkList<Tree>.Cons(Mrg(ts1, ts2.Next), ts2.Element);
            return InsTree(Link(ts1.Element, ts2.Element), Mrg(ts1.Next, ts2.Next));
        }

        public static /* lazy */ Lazy<LinkList<Tree>.ListStructure> Insert(T x, /* $ */ Lazy<LinkList<Tree>.ListStructure> ts)
        {
            return /* $ */ new Lazy<LinkList<Tree>.ListStructure>(() => InsTree(new Tree(0, x, EmptyList), ts.Value));
        }

        public static /* lazy */ Lazy<LinkList<Tree>.ListStructure> Merge(/* $ */ Lazy<LinkList<Tree>.ListStructure> ts1, /* $ */ Lazy<LinkList<Tree>.ListStructure> ts2)
        {
            return /* $ */ new Lazy<LinkList<Tree>.ListStructure>(() => Mrg(ts1.Value, ts2.Value));
        }

        private class TreeParts
        {
            private readonly Tree tree;

            private readonly LinkList<Tree>.ListStructure list;

            public TreeParts(Tree tree, LinkList<Tree>.ListStructure list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return this.tree; }
            }

            public LinkList<Tree>.ListStructure List
            {
                get { return this.list; }
            }
        }

        private static TreeParts RemoveMinTree(LinkList<Tree>.ListStructure list)
        {
            if (list == EmptyList) throw new Exception("Empty");
            if (list.Next == EmptyList) return new TreeParts(list.Element, EmptyList);
            var prime = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(prime.Tree.Root) <= 0) return new TreeParts(list.Element, list.Next);
            return new TreeParts(prime.Tree, LinkList<Tree>.Cons(prime.List, list.Element));
        }

        public static T FindMin(/* $ */ Lazy<LinkList<Tree>.ListStructure> ts)
        {
            var t = RemoveMinTree(ts.Value);
            return t.Tree.Root;

        }

        public static /* lazy */ Lazy<LinkList<Tree>.ListStructure> DeleteMin(/* $ */ Lazy<LinkList<Tree>.ListStructure> ts)
        {
            var t = RemoveMinTree(ts.Value);
            var x = LinkList<Tree>.Reverse(t.Tree.List);
            return /* $ */ new Lazy<LinkList<Tree>.ListStructure>(() => Mrg(x, t.List));
        }
    }
}