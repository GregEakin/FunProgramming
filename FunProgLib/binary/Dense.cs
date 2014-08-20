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

        public static LinkList<string>.List Inc(LinkList<string>.List ds)
        {
            if (ds == null) return LinkList<string>.Cons(DigitShapes[1], null);
            if (ds.Element == DigitShapes[0]) return LinkList<string>.Cons(DigitShapes[1], ds.Next);
            return LinkList<string>.Cons(DigitShapes[0], Inc(ds.Next));
        }

        public static LinkList<string>.List Dec(LinkList<string>.List ds)
        {
            if (ds.Next == null) return null;
            if (ds.Element == DigitShapes[1]) return LinkList<string>.Cons(DigitShapes[0], ds.Next);
            return LinkList<string>.Cons(DigitShapes[1], Dec(ds.Next));
        }

        public static LinkList<string>.List Add(LinkList<string>.List ds1, LinkList<string>.List ds2)
        {
            if (ds2 == null) return ds1;
            if (ds1 == null) return ds2;
            if (ds2.Element == DigitShapes[0]) return LinkList<string>.Cons(ds1.Element, Add(ds1.Next, ds2.Next));
            if (ds1.Element == DigitShapes[0]) return LinkList<string>.Cons(ds2.Element, Add(ds1.Next, ds2.Next));
            return LinkList<string>.Cons(DigitShapes[0], Inc(Add(ds1.Next, ds2.Next)));
        }
    }
}