// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "4.2 Streams." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 34-37. Print.

namespace FunProgLib.streams
{
    using System;

    public static class Stream<T>
    {
        public sealed class StreamCell
        {
            private readonly T x;
            private readonly Lazy<StreamCell> s;

            public StreamCell(T x, Lazy<StreamCell> s)
            {
                this.x = x;
                this.s = s;
            }

            public T X { get { return this.x; } }
            public Lazy<StreamCell> S { get { return this.s; } }
        }

        private static readonly Lazy<StreamCell> NilStreamCell = new Lazy<StreamCell>(() => null);

        public static Lazy<StreamCell> DollarNil { get { return NilStreamCell; } }

        public static Lazy<StreamCell> DollarCons(T element, Lazy<StreamCell> s)
        {
            return new Lazy<StreamCell>(() => new StreamCell(element, s));
        }

        public static Lazy<StreamCell> Append(Lazy<StreamCell> s1, Lazy<StreamCell> t)
        {
            if (s1 == null || s1 == DollarNil) return t;
            return DollarCons(s1.Value.X, Append(s1.Value.S, t));
        }

        public static Lazy<StreamCell> Take(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return DollarNil;
            if (s == DollarNil) return DollarNil;
            return DollarCons(s.Value.X, Take(n - 1, s.Value.S));
        }

        private static Lazy<StreamCell> DropPrime(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return s;
            if (s == DollarNil) return DollarNil;
            return DropPrime(n - 1, s.Value.S);
        }

        public static Lazy<StreamCell> Drop(int n, Lazy<StreamCell> s)
        {
            return DropPrime(n, s);
        }

        private static Lazy<StreamCell> ReversePrime(Lazy<StreamCell> s, Lazy<StreamCell> r)
        {
            if (s == null || s == DollarNil) return r;
            return ReversePrime(s.Value.S, DollarCons(s.Value.X, r));
        }

        public static Lazy<StreamCell> Reverse(Lazy<StreamCell> s)
        {
            return ReversePrime(s, DollarNil);
        }
    }
}
