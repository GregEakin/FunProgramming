// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.1 Positional Number System." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 116-17. Print.

using FunProgLib.lists;

namespace FunProgLib.binary;

public static class SparseByWeight
{
    private static FunList<int>.Node Carry(int w, FunList<int>.Node list)
    {
        if (FunList<int>.IsEmpty(list)) return FunList<int>.Cons(w, null);
        if (w < list.Element) return FunList<int>.Cons(w, list);
        return Carry(2 * w, list.Next);
    }

    private static FunList<int>.Node Borrow(int w, FunList<int>.Node list)
    {
        if (w == list.Element) return list.Next;
        return FunList<int>.Cons(w, Borrow(2 * w, list));
    }

    public static FunList<int>.Node Inc(FunList<int>.Node ws) => Carry(1, ws);

    public static FunList<int>.Node Dec(FunList<int>.Node ws) => Borrow(1, ws);

    public static FunList<int>.Node Add(FunList<int>.Node ds1, FunList<int>.Node ds2)
    {
        if (FunList<int>.IsEmpty(ds2)) return ds1;
        if (FunList<int>.IsEmpty(ds1)) return ds2;
        if (ds1.Element < ds2.Element) return FunList<int>.Cons(ds1.Element, Add(ds1.Next, ds2));
        if (ds2.Element < ds1.Element) return FunList<int>.Cons(ds2.Element, Add(ds1, ds2.Next));
        return Carry(2 * ds1.Element, Add(ds1.Next, ds2.Next));
    }
}