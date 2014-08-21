﻿// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		SparseByWeight.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.binary
{
    using FunProgLib.lists;

    public static class SparseByWeight
    {
        private static List<int>.Node Carry(int w, List<int>.Node list)
        {
            if (list == null) return List<int>.Cons(w, null);
            if (w < list.Element) return List<int>.Cons(w, list);
            return Carry(2 * w, list.Next);
        }

        private static List<int>.Node Borrow(int w, List<int>.Node list)
        {
            if (w == list.Element) return list.Next;
            return List<int>.Cons(w, Borrow(2 * w, list));
        }

        public static List<int>.Node Inc(List<int>.Node ws)
        {
            return Carry(1, ws);
        }

        public static List<int>.Node Dec(List<int>.Node ws)
        {
            return Borrow(1, ws);
        }

        public static List<int>.Node Add(List<int>.Node ds1, List<int>.Node ds2)
        {
            if (ds2 == null) return ds1;
            if (ds1 == null) return ds2;
            if (ds1.Element < ds2.Element) return List<int>.Cons(ds1.Element, Add(ds1.Next, ds2));
            if (ds2.Element < ds1.Element) return List<int>.Cons(ds2.Element, Add(ds1, ds2.Next));
            return Carry(2 * ds1.Element, Add(ds1.Next, ds2.Next));
        }
    }
}