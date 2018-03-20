// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.3.2 Skew Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 134-8. Print.

namespace FunProgLib.heap
{
    using lists;
    using System;

    public static class SkewBinomialHeap<T>
        where T : IComparable<T> // : IHeap<T>
    {
        public sealed class Tree
        {
            public Tree(int rank, T root, List<T>.Node list, List<Tree>.Node treeList)
            {
                Rank = rank;
                Root = root;
                List = list;
                TreeList = treeList;
            }

            public int Rank { get; }

            public T Root { get; }

            public List<T>.Node List { get; }

            public List<Tree>.Node TreeList { get; }
        }

        public static List<Tree>.Node Empty => List<Tree>.Empty;

        public static bool IsEmpty(List<Tree>.Node heap) => List<Tree>.IsEmpty(heap);

        public static int Rank(Tree t) => t.Rank;

        public static T Root(Tree t) => t.Root;

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, t1.List, List<Tree>.Cons(t2, t1.TreeList));
            return new Tree(t1.Rank + 1, t2.Root, t2.List, List<Tree>.Cons(t1, t2.TreeList));
        }

        private static Tree SkewLink(T element, Tree t1, Tree t2)
        {
            var node = Link(t1, t2);
            if (element.CompareTo(node.Root) <= 0) return new Tree(node.Rank, element, List<T>.Cons(node.Root, node.List), node.TreeList);
            return new Tree(node.Rank, node.Root, List<T>.Cons(element, node.List), node.TreeList);
        }

        private static List<Tree>.Node InsTree(Tree t1, List<Tree>.Node treeList)
        {
            if (List<Tree>.IsEmpty(treeList)) return List<Tree>.Cons(t1, List<Tree>.Empty);

            var t2 = List<Tree>.Head(treeList);
            if (t1.Rank < t2.Rank) return List<Tree>.Cons(t1, treeList);
            var ts = List<Tree>.Tail(treeList);
            return InsTree(Link(t1, t2), ts);
        }

        private static List<Tree>.Node MergeTrees(List<Tree>.Node ts1, List<Tree>.Node ts2)
        {
            if (List<Tree>.IsEmpty(ts2)) return ts1;
            if (List<Tree>.IsEmpty(ts1)) return ts2;

            var t1 = List<Tree>.Head(ts1);
            var tsp1 = List<Tree>.Tail(ts1);
            var t2 = List<Tree>.Head(ts2);
            if (t1.Rank < t2.Rank) return List<Tree>.Cons(t1, MergeTrees(tsp1, ts2));
            var tsp2 = List<Tree>.Tail(ts2);
            if (t2.Rank < t1.Rank) return List<Tree>.Cons(t2, MergeTrees(ts1, tsp2));
            return InsTree(Link(List<Tree>.Head(ts1), List<Tree>.Head(ts2)), MergeTrees(tsp1, tsp2));
        }

        private static List<Tree>.Node Normalize(List<Tree>.Node tsp)
        {
            if (List<Tree>.IsEmpty(tsp)) return List<Tree>.Empty;
            var t = List<Tree>.Head(tsp);
            var ts = List<Tree>.Tail(tsp);
            return InsTree(t, ts);
        }

        public static List<Tree>.Node Insert(T x, List<Tree>.Node ts)
        {
            if (List<Tree>.IsEmpty(ts)) return List<Tree>.Cons(new Tree(0, x, List<T>.Empty, List<Tree>.Empty), List<Tree>.Empty);
            var tail = List<Tree>.Tail(ts);
            if (List<Tree>.IsEmpty(tail)) return List<Tree>.Cons(new Tree(0, x, List<T>.Empty, List<Tree>.Empty), ts);
            var t1 = List<Tree>.Head(ts);
            var t2 = List<Tree>.Head(tail);
            var rest = List<Tree>.Tail(tail);
            if (t1.Rank == t2.Rank) return List<Tree>.Cons(SkewLink(x, t1, t2), rest);
            return List<Tree>.Cons(new Tree(0, x, List<T>.Empty, List<Tree>.Empty), ts);
        }

        public static List<Tree>.Node Merge(List<Tree>.Node ts1, List<Tree>.Node ts2) => MergeTrees(Normalize(ts1), Normalize(ts2));

        private static (Tree, List<Tree>.Node) RemoveMinTree(List<Tree>.Node ds)
        {
            if (List<Tree>.IsEmpty(ds)) throw new ArgumentNullException(nameof(ds));
            var t = List<Tree>.Head(ds);
            var ts = List<Tree>.Tail(ds);
            if (List<Tree>.IsEmpty(ts)) return (t, ts);
            var (tp, tsp) = RemoveMinTree(ts);
            if (t.Root.CompareTo(tp.Root) <= 0) return (t, ts);
            return (tp, List<Tree>.Cons(t, tsp));
        }

        public static T FindMin(List<Tree>.Node ts) => RemoveMinTree(ts).Item1.Root;

        private static List<Tree>.Node InsertAll(List<T>.Node xsp, List<Tree>.Node ts)
        {
            if (List<T>.IsEmpty(xsp)) return ts;
            var x = List<T>.Head(xsp);
            var xs = List<T>.Tail(xsp);
            return InsertAll(xs, Insert(x, ts));
        }

        public static List<Tree>.Node DeleteMin(List<Tree>.Node ts)
        {
            var (tree, ts2) = RemoveMinTree(ts);
            var _ = tree.Rank;
            var x = tree.Root;
            var xs = tree.List;
            var ts1 = tree.TreeList;
            return InsertAll(xs, Merge(List<Tree>.Reverse(ts1), ts2));
        }
    }
}
