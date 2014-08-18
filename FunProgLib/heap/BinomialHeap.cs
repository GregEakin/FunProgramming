// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinomialHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.heap
{
    using System;

    using FunProgLib.persistence;

    public static class BinomialHeap<T> where T : IComparable
    {
        public sealed class Tree
        {
            private readonly int rank;

            private readonly T root;

            private readonly LinkList<Tree>.List list;

            public Tree(int rank, T root, LinkList<Tree>.List list)
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

            public LinkList<Tree>.List List
            {
                get { return list; }
            }
        }

        private static readonly LinkList<Tree>.List EmptyList = null; // new LinkList<Tree>.List(EmptyList, null);

        public static LinkList<Tree>.List Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmapty(LinkList<Tree>.List list)
        {
            return list == EmptyList;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, LinkList<Tree>.Cons(t2, t1.List));
            return new Tree(t1.Rank + 1, t2.Root, LinkList<Tree>.Cons(t1, t2.List));
        }

        private static LinkList<Tree>.List InsertTree(Tree t, LinkList<Tree>.List ts)
        {
            if (ts == EmptyList) return new LinkList<Tree>.List(t, EmptyList);
            if (t.Rank < ts.Element.Rank) return LinkList<Tree>.Cons(t, ts);
            return InsertTree(Link(t, ts.Element), ts.Next);
        }

        public static LinkList<Tree>.List Insert(T x, LinkList<Tree>.List ts)
        {
            return InsertTree(new Tree(0, x, EmptyList), ts);
        }

        public static LinkList<Tree>.List Merge(LinkList<Tree>.List ts1, LinkList<Tree>.List ts2)
        {
            if (ts2 == EmptyList) return ts1;
            if (ts1 == EmptyList) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return LinkList<Tree>.Cons(ts1.Element, Merge(ts1.Next, ts2));
            if (ts2.Element.Rank < ts1.Element.Rank) return LinkList<Tree>.Cons(ts2.Element, Merge(ts1, ts2.Next));
            return InsertTree(Link(ts1.Element, ts2.Element), Merge(ts1.Next, ts2.Next));
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

        public static T FindMin(LinkList<Tree>.List ts)
        {
            var t = RemoveMinTree(ts);
            return t.Tree.Root;
        }

        public static LinkList<Tree>.List DeleteMin(LinkList<Tree>.List ts)
        {
            var t = RemoveMinTree(ts);
            var x = LinkList<Tree>.Reverse(t.Tree.List);
            return Merge(x, t.List);
        }
    }
}