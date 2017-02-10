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

        public static List<int>.Node Inc(List<int>.Node ws) => Carry(1, ws);

        public static List<int>.Node Dec(List<int>.Node ws) => Borrow(1, ws);

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
