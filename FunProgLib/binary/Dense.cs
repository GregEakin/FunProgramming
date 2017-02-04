// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.1 Positional Number System." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 116-17. Print.

namespace FunProgLib.binary
{
    using lists;
    using System;

    public static class Dense
    {
        public enum Digit { Zero, One };

        public static List<Digit>.Node Inc(List<Digit>.Node ds)
        {
            if (List<Digit>.IsEmpty(ds)) return List<Digit>.Cons(Digit.One, List<Digit>.Empty);
            if (ds.Element == Digit.Zero) return List<Digit>.Cons(Digit.One, ds.Next);
            return List<Digit>.Cons(Digit.Zero, Inc(ds.Next));
        }

        public static List<Digit>.Node Dec(List<Digit>.Node ds)
        {
            if (List<Digit>.IsEmpty(ds)) throw new ArgumentException("Can't go negative", nameof(ds));
            if (List<Digit>.IsEmpty(ds.Next)) return List<Digit>.Empty;
            if (ds.Element == Digit.One) return List<Digit>.Cons(Digit.Zero, ds.Next);
            return List<Digit>.Cons(Digit.One, Dec(ds.Next));
        }

        public static List<Digit>.Node Add(List<Digit>.Node ds1, List<Digit>.Node ds2)
        {
            if (List<Digit>.IsEmpty(ds2)) return ds1;
            if (List<Digit>.IsEmpty(ds1)) return ds2;
            if (ds2.Element == Digit.Zero) return List<Digit>.Cons(ds1.Element, Add(ds1.Next, ds2.Next));
            if (ds1.Element == Digit.Zero) return List<Digit>.Cons(ds2.Element, Add(ds1.Next, ds2.Next));
            return List<Digit>.Cons(Digit.Zero, Inc(Add(ds1.Next, ds2.Next)));
        }
    }
}
