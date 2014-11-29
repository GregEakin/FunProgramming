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
    using System;

    using FunProgLib.lists;

    public class SkewBinomialHeap<T>
        where T : IComparable<T> // : IHeap<T>
    {
        public sealed class Tree
        {
            private readonly int rank;

            private readonly T node;

            private readonly List<T>.Node list;

            private readonly List<Tree>.Node treeList;

            public Tree(int rank, T node, List<T>.Node list, List<Tree>.Node treeList)
            {
                this.rank = rank;
                this.node = node;
                this.list = list;
                this.treeList = treeList;
            }

            public int Rank
            {
                get { return this.rank; }
            }

            public T Node
            {
                get { return this.node; }
            }

            public List<T>.Node List
            {
                get { return list; }
            }

            public List<Tree>.Node TreeList
            {
                get { return this.treeList; }
            }
        }

        private static readonly Tree EmptyHeap = null;

        public static Tree Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmpty(Tree heap)
        {
            return heap == EmptyHeap;
        }

        private static int Rank(Tree heap)
        {
            return heap.Rank;
        }

        private static T Root(Tree heap)
        {
            return heap.Node;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            if (t1.Node.CompareTo(t2.Node) <= 0) return new Tree(t1.Rank + 1, t1.Node, t1.List, List<Tree>.Cons(t2, t1.TreeList));
            return new Tree(t1.Rank + 1, t2.Node, t2.List, List<Tree>.Cons(t1, t2.TreeList));
        }

        private static Tree SkewLink(T element, Tree t1, Tree t2)
        {
            var node = Link(t1, t2);
            if (element.CompareTo(node.Node) <= 0) return new Tree(node.Rank, element, List<T>.Cons(node.Node, node.List), node.TreeList);
            return new Tree(node.Rank + 1, node.Node, List<T>.Cons(element, node.List), node.TreeList);
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
            if (!List<Tree>.IsEmpty(ts) && List<Tree>.IsEmpty(List<Tree>.Tail(ts)))
            {
                var t1 = List<Tree>.Head(ts);
                var tail = List<Tree>.Tail(ts);
                var t2 = List<Tree>.Head(tail);
                var rest = List<Tree>.Tail(tail);
                if (t1.Rank == t2.Rank) return List<Tree>.Cons(SkewLink(x, t1, t2), rest);
                return List<Tree>.Cons(new Tree(0, x, List<T>.Empty, List<Tree>.Empty), ts);
            }
            return List<Tree>.Cons(new Tree(0, x, List<T>.Empty, List<Tree>.Empty), List<Tree>.Tail(ts));
        }

        public static List<Tree>.Node Merge(List<Tree>.Node ts1, List<Tree>.Node ts2)
        {
            return MergeTrees(Normalize(ts1), Normalize(ts2));
        }

        private static List<Tree>.Node RemoveMinTree(List<Tree>.Node ds)
        {
            if (List<Tree>.IsEmpty(ds)) throw new Exception("Empty");
            throw new NotImplementedException();
        }

        public static T FindMin(List<Tree>.Node ts)
        {
            var val = RemoveMinTree(ts);
            return val.Element.Node;
        }

        public static List<Tree>.Node DeleteMin(List<Tree>.Node ts)
        {
            throw new NotImplementedException();
        }
    }
}
