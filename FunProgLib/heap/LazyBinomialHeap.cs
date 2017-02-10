// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.1 Example: Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 70-72. Print.

namespace FunProgLib.heap
{
    using lists;
    using System;
    // using Heap = System.Lazy<lists.List<LazyBinomialHeap<int>.Tree>.Node>;

    public static class LazyBinomialHeap<T> where T : IComparable<T>
    {
        public sealed class Tree
        {
            public Tree(int rank, T x, List<Tree>.Node list)
            {
                Rank = rank;
                Root = x;
                List = list;
            }

            public int Rank { get; }

            public T Root { get; }

            public List<Tree>.Node List { get; }
        }

        public static Lazy<List<Tree>.Node> Empty { get; } = new Lazy<List<Tree>.Node>(() => List<Tree>.Empty);

        public static bool IsEmpty(Lazy<List<Tree>.Node> heap) => List<Tree>.IsEmpty(heap.Value);

        public static int Rank(Tree t) => t.Rank;

        public static T Root(Tree t) => t.Root;

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, List<Tree>.Cons(t2, t1.List));
            return new Tree(t1.Rank + 1, t2.Root, List<Tree>.Cons(t1, t2.List));
        }

        private static List<Tree>.Node InsTree(Tree t, List<Tree>.Node ts)
        {
            if (List<Tree>.IsEmpty(ts)) return List<Tree>.Cons(t, List<Tree>.Empty);
            if (t.Rank < ts.Element.Rank) return List<Tree>.Cons(t, ts);
            return InsTree(Link(t, ts.Element), ts.Next);
        }

        private static List<Tree>.Node Mrg(List<Tree>.Node ts1, List<Tree>.Node ts2)
        {
            if (List<Tree>.IsEmpty(ts2)) return ts1;
            if (List<Tree>.IsEmpty(ts1)) return ts2;

            if (ts1.Element.Rank < ts2.Element.Rank) return List<Tree>.Cons(ts1.Element, Mrg(ts1.Next, ts2));
            if (ts2.Element.Rank < ts1.Element.Rank) return List<Tree>.Cons(ts2.Element, Mrg(ts1, ts2.Next));
            return InsTree(Link(ts1.Element, ts2.Element), Mrg(ts1.Next, ts2.Next));
        }

        public static Lazy<List<Tree>.Node> Insert(T x, Lazy<List<Tree>.Node> ts) => 
            new Lazy<List<Tree>.Node>(() => InsTree(new Tree(0, x, List<Tree>.Empty), ts.Value));

        public static Lazy<List<Tree>.Node> Merge(Lazy<List<Tree>.Node> ts1, Lazy<List<Tree>.Node> ts2) => 
            new Lazy<List<Tree>.Node>(() => Mrg(ts1.Value, ts2.Value));

        private class TreeParts
        {
            public TreeParts(Tree tree, List<Tree>.Node list)
            {
                Tree = tree;
                List = list;
            }

            public Tree Tree { get; }

            public List<Tree>.Node List { get; }
        }

        private static TreeParts RemoveMinTree(List<Tree>.Node list)
        {
            if (List<Tree>.IsEmpty(list)) throw new ArgumentNullException(nameof(list));
            if (List<Tree>.IsEmpty(list.Next)) return new TreeParts(list.Element, List<Tree>.Empty);
            var prime = RemoveMinTree(list.Next);
            if (list.Element.Root.CompareTo(prime.Tree.Root) <= 0) return new TreeParts(list.Element, list.Next);
            return new TreeParts(prime.Tree, List<Tree>.Cons(list.Element, prime.List));
        }

        public static T FindMin(Lazy<List<Tree>.Node> ts) => RemoveMinTree(ts.Value).Tree.Root;

        public static Lazy<List<Tree>.Node> DeleteMin(Lazy<List<Tree>.Node> ts)
        {
            return new Lazy<List<Tree>.Node>(() =>
                {
                    var t = RemoveMinTree(ts.Value);
                    var x = List<Tree>.Reverse(t.Tree.List);
                    return Mrg(x, t.List);
                });
        }
    }
}
