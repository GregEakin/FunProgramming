// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.3.2 Skew Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 134-8. Print.

using FunProgLib.lists;

namespace FunProgLib.heap;

public static class SkewBinomialHeap<T>
    where T : IComparable<T> // : IHeap<T>
{
    public sealed class Tree
    {
        public Tree(int rank, T root, FunList<T>.Node list, FunList<Tree>.Node treeList)
        {
            Rank = rank;
            Root = root;
            FunList = list;
            TreeList = treeList;
        }

        public int Rank { get; }

        public T Root { get; }

        public FunList<T>.Node FunList { get; }

        public FunList<Tree>.Node TreeList { get; }
    }

    public static FunList<Tree>.Node Empty => FunList<Tree>.Empty;

    public static bool IsEmpty(FunList<Tree>.Node heap) => FunList<Tree>.IsEmpty(heap);

    public static int Rank(Tree t) => t.Rank;

    public static T Root(Tree t) => t.Root;

    private static Tree Link(Tree t1, Tree t2)
    {
        if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, t1.FunList, FunList<Tree>.Cons(t2, t1.TreeList));
        return new Tree(t1.Rank + 1, t2.Root, t2.FunList, FunList<Tree>.Cons(t1, t2.TreeList));
    }

    private static Tree SkewLink(T element, Tree t1, Tree t2)
    {
        var node = Link(t1, t2);
        if (element.CompareTo(node.Root) <= 0) return new Tree(node.Rank, element, FunList<T>.Cons(node.Root, node.FunList), node.TreeList);
        return new Tree(node.Rank, node.Root, FunList<T>.Cons(element, node.FunList), node.TreeList);
    }

    private static FunList<Tree>.Node InsTree(Tree t1, FunList<Tree>.Node treeList)
    {
        if (FunList<Tree>.IsEmpty(treeList)) return FunList<Tree>.Cons(t1, FunList<Tree>.Empty);

        var t2 = FunList<Tree>.Head(treeList);
        if (t1.Rank < t2.Rank) return FunList<Tree>.Cons(t1, treeList);
        var ts = FunList<Tree>.Tail(treeList);
        return InsTree(Link(t1, t2), ts);
    }

    private static FunList<Tree>.Node MergeTrees(FunList<Tree>.Node ts1, FunList<Tree>.Node ts2)
    {
        if (FunList<Tree>.IsEmpty(ts2)) return ts1;
        if (FunList<Tree>.IsEmpty(ts1)) return ts2;

        var t1 = FunList<Tree>.Head(ts1);
        var tsp1 = FunList<Tree>.Tail(ts1);
        var t2 = FunList<Tree>.Head(ts2);
        if (t1.Rank < t2.Rank) return FunList<Tree>.Cons(t1, MergeTrees(tsp1, ts2));
        var tsp2 = FunList<Tree>.Tail(ts2);
        if (t2.Rank < t1.Rank) return FunList<Tree>.Cons(t2, MergeTrees(ts1, tsp2));
        return InsTree(Link(FunList<Tree>.Head(ts1), FunList<Tree>.Head(ts2)), MergeTrees(tsp1, tsp2));
    }

    private static FunList<Tree>.Node Normalize(FunList<Tree>.Node tsp)
    {
        if (FunList<Tree>.IsEmpty(tsp)) return FunList<Tree>.Empty;
        var t = FunList<Tree>.Head(tsp);
        var ts = FunList<Tree>.Tail(tsp);
        return InsTree(t, ts);
    }

    public static FunList<Tree>.Node Insert(T x, FunList<Tree>.Node ts)
    {
        if (FunList<Tree>.IsEmpty(ts)) return FunList<Tree>.Cons(new Tree(0, x, FunList<T>.Empty, FunList<Tree>.Empty), FunList<Tree>.Empty);
        var tail = FunList<Tree>.Tail(ts);
        if (FunList<Tree>.IsEmpty(tail)) return FunList<Tree>.Cons(new Tree(0, x, FunList<T>.Empty, FunList<Tree>.Empty), ts);
        var t1 = FunList<Tree>.Head(ts);
        var t2 = FunList<Tree>.Head(tail);
        var rest = FunList<Tree>.Tail(tail);
        if (t1.Rank == t2.Rank) return FunList<Tree>.Cons(SkewLink(x, t1, t2), rest);
        return FunList<Tree>.Cons(new Tree(0, x, FunList<T>.Empty, FunList<Tree>.Empty), ts);
    }

    public static FunList<Tree>.Node Merge(FunList<Tree>.Node ts1, FunList<Tree>.Node ts2) => MergeTrees(Normalize(ts1), Normalize(ts2));

    private static (Tree, FunList<Tree>.Node) RemoveMinTree(FunList<Tree>.Node ds)
    {
        if (FunList<Tree>.IsEmpty(ds)) throw new ArgumentNullException(nameof(ds));
        var t = FunList<Tree>.Head(ds);
        var ts = FunList<Tree>.Tail(ds);
        if (FunList<Tree>.IsEmpty(ts)) return (t, ts);
        var (tp, tsp) = RemoveMinTree(ts);
        if (t.Root.CompareTo(tp.Root) <= 0) return (t, ts);
        return (tp, FunList<Tree>.Cons(t, tsp));
    }

    public static T FindMin(FunList<Tree>.Node ts) => RemoveMinTree(ts).Item1.Root;

    private static FunList<Tree>.Node InsertAll(FunList<T>.Node xsp, FunList<Tree>.Node ts)
    {
        if (FunList<T>.IsEmpty(xsp)) return ts;
        var x = FunList<T>.Head(xsp);
        var xs = FunList<T>.Tail(xsp);
        return InsertAll(xs, Insert(x, ts));
    }

    public static FunList<Tree>.Node DeleteMin(FunList<Tree>.Node ts)
    {
        var (tree, ts2) = RemoveMinTree(ts);
        //var _ = tree.Rank;
        //var x = tree.Root;
        var xs = tree.FunList;
        var ts1 = tree.TreeList;
        return InsertAll(xs, Merge(FunList<Tree>.Reverse(ts1), ts2));
    }
}