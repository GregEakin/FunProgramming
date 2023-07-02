// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "5.5 Pairing Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 52-54. Print.

using FunProgLib.lists;

namespace FunProgLib.heap;

public static class ParingHeap<T> where T : IComparable<T>
{
    public sealed class Heap
    {
        public Heap(T root, FunList<Heap>.Node list)
        {
            Root = root;
            FunList = list;
        }

        public T Root { get; }

        public FunList<Heap>.Node FunList { get; }
    }

    public static Heap Empty { get; } = null;

    public static bool IsEmpty(Heap list) => list == Empty;

    public static Heap Merge(Heap h1, Heap h2)
    {
        if (h2 == Empty) return h1;
        if (h1 == Empty) return h2;

        if (h1.Root.CompareTo(h2.Root) <= 0) return new Heap(h1.Root, FunList<Heap>.Cons(h2, h1.FunList));
        return new Heap(h2.Root, FunList<Heap>.Cons(h1, h2.FunList));
    }

    public static Heap Insert(T x, Heap h) => Merge(new Heap(x, FunList<Heap>.Empty), h);

    private static Heap MergePairs(FunList<Heap>.Node hs)
    {
        if (FunList<Heap>.IsEmpty(hs)) return Empty;
        if (FunList<Heap>.IsEmpty(hs.Next)) return hs.Element;
        return Merge(Merge(hs.Element, hs.Next.Element), MergePairs(hs.Next.Next));
    }

    public static T FindMin(Heap h)
    {
        if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
        return h.Root;
    }

    public static Heap DeleteMin(Heap h)
    {
        if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
        return MergePairs(h.FunList);
    }
}