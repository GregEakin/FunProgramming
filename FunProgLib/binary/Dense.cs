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

public static class Dense
{
    public enum Digit { Zero, One };

    public static FunList<Digit>.Node Inc(FunList<Digit>.Node ds)
    {
        if (FunList<Digit>.IsEmpty(ds)) return FunList<Digit>.Cons(Digit.One, FunList<Digit>.Empty);
        if (ds.Element == Digit.Zero) return FunList<Digit>.Cons(Digit.One, ds.Next);
        return FunList<Digit>.Cons(Digit.Zero, Inc(ds.Next));
    }

    public static FunList<Digit>.Node Dec(FunList<Digit>.Node ds)
    {
        if (FunList<Digit>.IsEmpty(ds)) throw new ArgumentException("Can't go negative", nameof(ds));
        if (FunList<Digit>.IsEmpty(ds.Next)) return FunList<Digit>.Empty;
        if (ds.Element == Digit.One) return FunList<Digit>.Cons(Digit.Zero, ds.Next);
        return FunList<Digit>.Cons(Digit.One, Dec(ds.Next));
    }

    public static FunList<Digit>.Node Add(FunList<Digit>.Node ds1, FunList<Digit>.Node ds2)
    {
        if (FunList<Digit>.IsEmpty(ds2)) return ds1;
        if (FunList<Digit>.IsEmpty(ds1)) return ds2;
        if (ds2.Element == Digit.Zero) return FunList<Digit>.Cons(ds1.Element, Add(ds1.Next, ds2.Next));
        if (ds1.Element == Digit.Zero) return FunList<Digit>.Cons(ds2.Element, Add(ds1.Next, ds2.Next));
        return FunList<Digit>.Cons(Digit.Zero, Inc(Add(ds1.Next, ds2.Next)));
    }
}