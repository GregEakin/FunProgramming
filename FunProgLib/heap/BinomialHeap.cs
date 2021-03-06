﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.2 Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 20-24. Print.

namespace FunProgLib.heap
{
    using System;
    using lists;

    public static class BinomialHeap<T> where T : IComparable<T>
    {
        public sealed class Tree
        {
            public Tree(int rank, T root, List<Tree>.Node list)
            {
                Rank = rank;
                Root = root;
                List = list;
            }

            public int Rank { get; }

            public T Root { get; }

            public List<Tree>.Node List { get; }
        }

        public static List<Tree>.Node Empty => List<Tree>.Empty;

        public static bool IsEmpty(List<Tree>.Node list) => List<Tree>.IsEmpty(list);

        public static int Rank(Tree t1) => t1.Rank;

        public static T Root(Tree t1) => t1.Root;

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, List<Tree>.Cons(t2, t1.List));
            return new Tree(t1.Rank + 1, t2.Root, List<Tree>.Cons(t1, t2.List));
        }

        private static List<Tree>.Node InsertTree(Tree t, List<Tree>.Node ts)
        {
            if (List<Tree>.IsEmpty(ts)) return new List<Tree>.Node(t, List<Tree>.Empty);
            if (t.Rank < ts.Element.Rank) return List<Tree>.Cons(t, ts);
            return InsertTree(Link(t, ts.Element), ts.Next);
        }

        public static List<Tree>.Node Insert(T x, List<Tree>.Node ts) => InsertTree(new Tree(0, x, List<Tree>.Empty), ts);

        public static List<Tree>.Node Merge(List<Tree>.Node ts1, List<Tree>.Node ts2)
        {
            if (List<Tree>.IsEmpty(ts2)) return ts1;
            if (List<Tree>.IsEmpty(ts1)) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return List<Tree>.Cons(ts1.Element, Merge(ts1.Next, ts2));
            if (ts2.Element.Rank < ts1.Element.Rank) return List<Tree>.Cons(ts2.Element, Merge(ts1, ts2.Next));
            return InsertTree(Link(ts1.Element, ts2.Element), Merge(ts1.Next, ts2.Next));
        }

        private static (Tree, List<Tree>.Node) RemoveMinTree(List<Tree>.Node list)
        {
            if (List<Tree>.IsEmpty(list)) throw new ArgumentNullException(nameof(list));
            if (List<Tree>.IsEmpty(list.Next)) return (list.Element, List<Tree>.Empty);
            var (tp, tsp) = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(tp.Root) <= 0) return (list.Element, list.Next);
            return (tp, List<Tree>.Cons(list.Element, tsp));
        }

        public static T FindMin(List<Tree>.Node ts) => RemoveMinTree(ts).Item1.Root;

        public static List<Tree>.Node DeleteMin(List<Tree>.Node ts)
        {
            var (tree, list) = RemoveMinTree(ts);
            var rev = List<Tree>.Reverse(tree.List);
            return Merge(rev, list);
        }
    }
}
