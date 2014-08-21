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
        // enum Digit { Zero, One };
        private readonly static string[] DigitShapes = { "Zero", "One" };

        public static List<string>.Node Inc(List<string>.Node ds)
        {
            if (ds == null) return List<string>.Cons(DigitShapes[1], null);
            if (ds.Element == DigitShapes[0]) return List<string>.Cons(DigitShapes[1], ds.Next);
            return List<string>.Cons(DigitShapes[0], Inc(ds.Next));
        }

        public static List<string>.Node Dec(List<string>.Node ds)
        {
            if (ds.Next == null) return null;
            if (ds.Element == DigitShapes[1]) return List<string>.Cons(DigitShapes[0], ds.Next);
            return List<string>.Cons(DigitShapes[1], Dec(ds.Next));
        }

        public static List<string>.Node Add(List<string>.Node ds1, List<string>.Node ds2)
        {
            if (ds2 == null) return ds1;
            if (ds1 == null) return ds2;
            if (ds2.Element == DigitShapes[0]) return List<string>.Cons(ds1.Element, Add(ds1.Next, ds2.Next));
            if (ds1.Element == DigitShapes[0]) return List<string>.Cons(ds2.Element, Add(ds1.Next, ds2.Next));
            return List<string>.Cons(DigitShapes[0], Inc(Add(ds1.Next, ds2.Next)));
        }
    }
}