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
    internal sealed class AltBinaryRandomAccessOne<T> : AltBinaryRandomAccessList<T>
    {
        private readonly T _alpha;

        internal AltBinaryRandomAccessOne(T alpha, List2<Tuple<T, T>> rlist)
            : base(rlist)
        {
            _alpha = alpha;
        }

        public override bool IsEmpty
        {
            get { return false; }
        }

        public override AltBinaryRandomAccessList<T> Cons(T x)
        {
            var list = List.Cons(new Tuple<T, T>(x, _alpha));
            return new AltBinaryRandomAccessZero<T>(list);
        }

        public override Tuple<T, AltBinaryRandomAccessList<T>> Uncons
        {
            get
            {
                return List.IsEmpty
                    ? new Tuple<T, AltBinaryRandomAccessList<T>>(_alpha, Empty)
                    : new Tuple<T, AltBinaryRandomAccessList<T>>(_alpha, new AltBinaryRandomAccessZero<T>(List));
            }
        }

        public override T Head
        {
            get { return _alpha; }
        }

        public override AltBinaryRandomAccessList<T> Tail
        {
            get
            {
                return List.IsEmpty
                    ? Empty
                    : new AltBinaryRandomAccessZero<T>(List);
            }
        }

        public override T Lookup(int i)
        {
            return i == 0
                ? _alpha
                : new AltBinaryRandomAccessZero<T>(List).Lookup(i - 1);
        }

        public override AltBinaryRandomAccessList<T> Fupdate(Fun f, int i)
        {
            return i == 0
                ? new AltBinaryRandomAccessOne<T>(f(_alpha), List)
                : new AltBinaryRandomAccessZero<T>(List).Fupdate(f, i - 1).Cons(_alpha);
        }
    }
}
