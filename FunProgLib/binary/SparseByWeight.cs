// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		SparseByWeight.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.binary
{

    using FunProgLib.persistence;

    public static class SparseByWeight
    {
        private static LinkList<int>.List Carry(int w, LinkList<int>.List list)
        {
            if (list == null) return LinkList<int>.Cons(w, null);
            if (w < list.Element) return LinkList<int>.Cons(w, list);
            return Carry(2 * w, list.Next);
        }

        private static LinkList<int>.List Borrow(int w, LinkList<int>.List list)
        {
            if (w == list.Element) return list.Next;
            return LinkList<int>.Cons(w, Borrow(2 * w, list));
        }

        public static LinkList<int>.List Inc(LinkList<int>.List ws)
        {
            return Carry(1, ws);
        }

        public static LinkList<int>.List Dec(LinkList<int>.List ws)
        {
            return Borrow(1, ws);
        }

        public static LinkList<int>.List Add(LinkList<int>.List ds1, LinkList<int>.List ds2)
        {
            if (ds2 == null) return ds1;
            if (ds1 == null) return ds2;
            if (ds1.Element < ds2.Element) return LinkList<int>.Cons(ds1.Element, Add(ds1.Next, ds2));
            if (ds2.Element < ds1.Element) return LinkList<int>.Cons(ds2.Element, Add(ds1, ds2.Next));
            return Carry(2 * ds1.Element, Add(ds1.Next, ds2.Next));
        }
    }
}