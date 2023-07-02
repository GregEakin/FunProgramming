// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.3 Example: Bottom-Up  Mergesort with Sharing." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 144-7. Print.

using FunProgLib.lists;

namespace FunProgLib.sort;

public static class BottomUpMergeSort<T> where T : IComparable<T>
{
    public sealed class Sortable
    {
        public Sortable(int size, Lazy<FunList<FunList<T>.Node>.Node> segs)
        {
            Size = size;
            Segs = segs;
        }

        public int Size { get; }

        public Lazy<FunList<FunList<T>.Node>.Node> Segs { get; }
    }

    private static FunList<T>.Node Mrg(FunList<T>.Node xs, FunList<T>.Node ys)
    {
        if (xs == null) return ys;
        if (ys == null) return xs;
        if (xs.Element.CompareTo(ys.Element) <= 0) return FunList<T>.Cons(xs.Element, Mrg(xs.Next, ys));
        return FunList<T>.Cons(ys.Element, Mrg(xs, ys.Next));
    }

    public static Sortable Empty { get; } = new Sortable(0, new Lazy<FunList<FunList<T>.Node>.Node>(() => FunList<FunList<T>.Node>.Empty));

    public static Sortable Add(T x, Sortable segs)
    {
        var xs = FunList<T>.Cons(x, FunList<T>.Empty);
        return new Sortable(segs.Size + 1, new Lazy<FunList<FunList<T>.Node>.Node>(AddSeg(xs, segs.Segs.Value, segs.Size)));
    }

    private static Func<FunList<FunList<T>.Node>.Node> AddSeg(FunList<T>.Node seg, FunList<FunList<T>.Node>.Node segs, int size)
    {
        if (size % 2 == 0) return () => FunList<FunList<T>.Node>.Cons(seg, segs);
        return AddSeg(Mrg(seg, segs.Element), segs.Next, size / 2);
    }

    public static FunList<T>.Node Sort(Sortable segs) => MrgAll(FunList<T>.Empty, segs.Segs.Value);

    private static FunList<T>.Node MrgAll(FunList<T>.Node xs, FunList<FunList<T>.Node>.Node ys)
    {
        if (ys == null) return xs;
        return MrgAll(Mrg(xs, ys.Element), ys.Next);
    }
}