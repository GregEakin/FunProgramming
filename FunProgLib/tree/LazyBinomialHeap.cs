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

            private readonly List<Tree>.ListStructure list;

            public Tree(int rank, T x, List<Tree>.ListStructure list)
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

            public List<Tree>.ListStructure List
            {
                get { return list; }
            }
        }

        // type Heap = Lazy<List<Tree>.ListStructure /* susp */

        private static readonly List<Tree>.ListStructure EmptyList = List<Tree>.Empty;

        private static readonly Lazy<List<Tree>.ListStructure> EmptyHeap = /* $ */ new Lazy<List<Tree>.ListStructure>(() => EmptyList);

        public static Lazy<List<Tree>.ListStructure> Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmapty(/* $ */ Lazy<List<Tree>.ListStructure> heap)
        {
            return heap.Value == EmptyList;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, List<Tree>.Cons(t1.List, t2));
            return new Tree(t1.Rank + 1, t2.Root, List<Tree>.Cons(t2.List, t1));
        }

        private static List<Tree>.ListStructure InsTree(Tree t, List<Tree>.ListStructure ts)
        {
            if (ts == EmptyList) return List<Tree>.Cons(EmptyList, t);
            if (t.Rank < ts.Element.Rank) return List<Tree>.Cons(ts, t);
            return InsTree(Link(t, ts.Element), ts.Next);
        }

        private static List<Tree>.ListStructure Mrg(List<Tree>.ListStructure ts1, List<Tree>.ListStructure ts2)
        {
            if (ts2 == EmptyList) return ts1;
            if (ts1 == EmptyList) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return List<Tree>.Cons(Mrg(ts1.Next, ts2), ts1.Element);
            if (ts2.Element.Rank < ts1.Element.Rank) return List<Tree>.Cons(Mrg(ts1, ts2.Next), ts2.Element);
            return InsTree(Link(ts1.Element, ts2.Element), Mrg(ts1.Next, ts2.Next));
        }

        public static /* lazy */ Lazy<List<Tree>.ListStructure> Insert(T x, /* $ */ Lazy<List<Tree>.ListStructure> ts)
        {
            return /* $ */ new Lazy<List<Tree>.ListStructure>(() => InsTree(new Tree(0, x, EmptyList), ts.Value));
        }

        public static /* lazy */ Lazy<List<Tree>.ListStructure> Merge(/* $ */ Lazy<List<Tree>.ListStructure> ts1, /* $ */ Lazy<List<Tree>.ListStructure> ts2)
        {
            return /* $ */ new Lazy<List<Tree>.ListStructure>(() => Mrg(ts1.Value, ts2.Value));
        }

        private class TreeParts
        {
            private readonly Tree tree;

            private readonly List<Tree>.ListStructure list;

            public TreeParts(Tree tree, List<Tree>.ListStructure list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return this.tree; }
            }

            public List<Tree>.ListStructure List
            {
                get { return this.list; }
            }
        }

        private static TreeParts RemoveMinTree(List<Tree>.ListStructure list)
        {
            if (list == EmptyList) throw new Exception("Empty");
            if (list.Next == EmptyList) return new TreeParts(list.Element, EmptyList);
            var prime = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(prime.Tree.Root) <= 0) return new TreeParts(list.Element, list.Next);
            return new TreeParts(prime.Tree, List<Tree>.Cons(prime.List, list.Element));
        }

        public static T FindMin(/* $ */ Lazy<List<Tree>.ListStructure> ts)
        {
            var t = RemoveMinTree(ts.Value);
            return t.Tree.Root;

        }

        public static /* lazy */ Lazy<List<Tree>.ListStructure> DeleteMin(/* $ */ Lazy<List<Tree>.ListStructure> ts)
        {
            var t = RemoveMinTree(ts.Value);
            var x = List<Tree>.Reverse(t.Tree.List);
            return /* $ */ new Lazy<List<Tree>.ListStructure>(() => Mrg(x, t.List));
        }
    }
}