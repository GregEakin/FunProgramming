// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.1.2 Binary Random-Access Lists Revisited." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 144-7. Print.

using System;

namespace FunProgLib.lists
{
    internal sealed class AltBinaryRandomAccessZero<T> : AltBinaryRandomAccessList<T>
    {
        internal AltBinaryRandomAccessZero(List2<Tuple<T, T>> rlist)
            : base(rlist)
        {
        }

        public override bool IsEmpty
        {
            get { return false; }
        }

        public override AltBinaryRandomAccessList<T> Cons(T x)
        {
            return new AltBinaryRandomAccessOne<T>(x, List);
        }

        public override Tuple<T, AltBinaryRandomAccessList<T>> Uncons
        {
            get
            {
                var head = List.Head;
                var list = List.Tail;
                return new Tuple<T, AltBinaryRandomAccessList<T>>(head.Item1, new AltBinaryRandomAccessOne<T>(head.Item2, list));
            }
        }

        public override T Head
        {
            get { return List.Head.Item1; }
        }

        public override AltBinaryRandomAccessList<T> Tail
        {
            get { return new AltBinaryRandomAccessOne<T>(List.Head.Item2, List.Tail); }
        }

        public override T Lookup(int i)
        {
            var element = List.Lookup(i / 2);
            return i % 2 == 0
                ? element.Item1
                : element.Item2;
        }

        public override AltBinaryRandomAccessList<T> Fupdate(Fun f, int i)
        {
            var fp = i % 2 == 0
                ? new List2<Tuple<T, T>>.Fun(t => new Tuple<T, T>(f(t.Item1), t.Item2))
                : new List2<Tuple<T, T>>.Fun(t => new Tuple<T, T>(t.Item1, f(t.Item2)));
            var list = List.Fupdate(fp, i / 2);
            return new AltBinaryRandomAccessZero<T>(list);
        }
    }
}
