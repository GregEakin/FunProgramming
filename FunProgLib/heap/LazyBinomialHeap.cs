// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.1 Example: Binomial Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 70-72. Print.

using FunProgLib.lists;
// using Heap = System.Lazy<lists.FunList<LazyBinomialHeap<int>.Tree>.Node>;

namespace FunProgLib.heap;

public static class LazyBinomialHeap<T> where T : IComparable<T>
{
    public sealed class Tree
    {
        public Tree(int rank, T x, FunList<Tree>.Node list)
        {
            Rank = rank;
            Root = x;
            FunList = list;
        }

        public int Rank { get; }

        public T Root { get; }

        public FunList<Tree>.Node FunList { get; }
    }

    public static Lazy<FunList<Tree>.Node> Empty { get; } = new Lazy<FunList<Tree>.Node>(() => FunList<Tree>.Empty);

    public static bool IsEmpty(Lazy<FunList<Tree>.Node> heap) => 
        heap == null || ReferenceEquals(Empty, heap) || FunList<Tree>.IsEmpty(heap.Value);

    public static int Rank(Tree t) => t.Rank;

    public static T Root(Tree t) => t.Root;

    private static Tree Link(Tree t1, Tree t2)
    {
        if (t1.Root.CompareTo(t2.Root) <= 0) return new Tree(t1.Rank + 1, t1.Root, FunList<Tree>.Cons(t2, t1.FunList));
        return new Tree(t1.Rank + 1, t2.Root, FunList<Tree>.Cons(t1, t2.FunList));
    }

    private static FunList<Tree>.Node InsTree(Tree t, FunList<Tree>.Node ts)
    {
        if (FunList<Tree>.IsEmpty(ts)) return FunList<Tree>.Cons(t, FunList<Tree>.Empty);
        if (t.Rank < ts.Element.Rank) return FunList<Tree>.Cons(t, ts);
        return InsTree(Link(t, ts.Element), ts.Next);
    }

    private static FunList<Tree>.Node Mrg(FunList<Tree>.Node ts1, FunList<Tree>.Node ts2)
    {
        if (FunList<Tree>.IsEmpty(ts2)) return ts1;
        if (FunList<Tree>.IsEmpty(ts1)) return ts2;

        if (ts1.Element.Rank < ts2.Element.Rank) return FunList<Tree>.Cons(ts1.Element, Mrg(ts1.Next, ts2));
        if (ts2.Element.Rank < ts1.Element.Rank) return FunList<Tree>.Cons(ts2.Element, Mrg(ts1, ts2.Next));
        return InsTree(Link(ts1.Element, ts2.Element), Mrg(ts1.Next, ts2.Next));
    }

    public static Lazy<FunList<Tree>.Node> Insert(T x, Lazy<FunList<Tree>.Node> ts) => 
        new Lazy<FunList<Tree>.Node>(() => InsTree(new Tree(0, x, FunList<Tree>.Empty), ts.Value));

    public static Lazy<FunList<Tree>.Node> Merge(Lazy<FunList<Tree>.Node> ts1, Lazy<FunList<Tree>.Node> ts2) => 
        new Lazy<FunList<Tree>.Node>(() => Mrg(ts1.Value, ts2.Value));

    private static (Tree, FunList<Tree>.Node) RemoveMinTree(FunList<Tree>.Node list)
    {
        if (FunList<Tree>.IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        if (FunList<Tree>.IsEmpty(list.Next)) return (list.Element, FunList<Tree>.Empty);
        var (tp, tsp) = RemoveMinTree(list.Next);
        if (list.Element.Root.CompareTo(tp.Root) <= 0) return (list.Element, list.Next);
        return (tp, FunList<Tree>.Cons(list.Element, tsp));
    }

    public static T FindMin(Lazy<FunList<Tree>.Node> ts) => RemoveMinTree(ts.Value).Item1.Root;

    public static Lazy<FunList<Tree>.Node> DeleteMin(Lazy<FunList<Tree>.Node> ts)
    {
        var (t, ts2) = RemoveMinTree(ts.Value);
        return new Lazy<FunList<Tree>.Node>(() => Mrg(FunList<Tree>.Reverse(t.FunList), ts2));
    }
}