// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		Dense.cs
// AUTHOR:		Greg Eakin

namespace FunProgLib.binary
{
    using FunProgLib.lists;

    public static class Dense
    {
        public enum Digit { Zero, One };

        public static List<Digit>.Node Inc(List<Digit>.Node ds)
        {
            if (ds == null) return List<Digit>.Cons(Digit.One, null);
            if (ds.Element == Digit.Zero) return List<Digit>.Cons(Digit.One, ds.Next);
            return List<Digit>.Cons(Digit.Zero, Inc(ds.Next));
        }

        public static List<Digit>.Node Dec(List<Digit>.Node ds)
        {
            if (ds.Next == null) return null;
            if (ds.Element == Digit.One) return List<Digit>.Cons(Digit.Zero, ds.Next);
            return List<Digit>.Cons(Digit.One, Dec(ds.Next));
        }

        public static List<Digit>.Node Add(List<Digit>.Node ds1, List<Digit>.Node ds2)
        {
            if (ds2 == null) return ds1;
            if (ds1 == null) return ds2;
            if (ds2.Element == Digit.Zero) return List<Digit>.Cons(ds1.Element, Add(ds1.Next, ds2.Next));
            if (ds1.Element == Digit.Zero) return List<Digit>.Cons(ds2.Element, Add(ds1.Next, ds2.Next));
            return List<Digit>.Cons(Digit.Zero, Inc(Add(ds1.Next, ds2.Next)));
        }
    }
}