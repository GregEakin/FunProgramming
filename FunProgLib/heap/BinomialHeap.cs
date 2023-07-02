// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.2 Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 20-24. Print.

using FunProgLib.lists;

namespace FunProgLib.heap;

public static class BinomialHeap<T> where T : IComparable<T>
{
    public sealed class Tree
    {
        public Tree(int rank, T root, FunList<Tree>.Node list)
        {
            Rank = rank;
            Root = root;
            FunList = list;
        }

        public int Rank { get; }

        public T Root { get; }

        public FunList<Tree>.Node FunList { get; }
    }

    public static FunList<Tree>.Node Empty => FunList<Tree>.Empty;

    public static bool IsEmpty(FunList<Tree>.Node list) => FunList<Tree>.IsEmpty(list);

    public static int Rank(Tree t1) => t1.Rank;

    public static T Root(Tree t1) => t1.Root;

    private static Tree Link(Tree t1, Tree t2)
    {
        if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, FunList<Tree>.Cons(t2, t1.FunList));
        return new Tree(t1.Rank + 1, t2.Root, FunList<Tree>.Cons(t1, t2.FunList));
    }

    private static FunList<Tree>.Node InsertTree(Tree t, FunList<Tree>.Node ts)
    {
        if (FunList<Tree>.IsEmpty(ts)) return new FunList<Tree>.Node(t, FunList<Tree>.Empty);
        if (t.Rank < ts.Element.Rank) return FunList<Tree>.Cons(t, ts);
        return InsertTree(Link(t, ts.Element), ts.Next);
    }

    public static FunList<Tree>.Node Insert(T x, FunList<Tree>.Node ts) => InsertTree(new Tree(0, x, FunList<Tree>.Empty), ts);

    public static FunList<Tree>.Node Merge(FunList<Tree>.Node ts1, FunList<Tree>.Node ts2)
    {
        if (FunList<Tree>.IsEmpty(ts2)) return ts1;
        if (FunList<Tree>.IsEmpty(ts1)) return ts2;

        if (ts1.Element.Rank < ts2.Element.Rank) return FunList<Tree>.Cons(ts1.Element, Merge(ts1.Next, ts2));
        if (ts2.Element.Rank < ts1.Element.Rank) return FunList<Tree>.Cons(ts2.Element, Merge(ts1, ts2.Next));
        return InsertTree(Link(ts1.Element, ts2.Element), Merge(ts1.Next, ts2.Next));
    }

    private static (Tree, FunList<Tree>.Node) RemoveMinTree(FunList<Tree>.Node list)
    {
        if (FunList<Tree>.IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        if (FunList<Tree>.IsEmpty(list.Next)) return (list.Element, FunList<Tree>.Empty);
        var (tp, tsp) = RemoveMinTree(list.Next);
        if (list.Element.Root.CompareTo(tp.Root) <= 0) return (list.Element, list.Next);
        return (tp, FunList<Tree>.Cons(list.Element, tsp));
    }

    public static T FindMin(FunList<Tree>.Node ts) => RemoveMinTree(ts).Item1.Root;

    public static FunList<Tree>.Node DeleteMin(FunList<Tree>.Node ts)
    {
        var (tree, list) = RemoveMinTree(ts);
        var rev = FunList<Tree>.Reverse(tree.FunList);
        return Merge(rev, list);
    }
}