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
        public abstract class StreamCell
        { }

        private sealed class Nil : StreamCell
        { }

        public sealed class Cons : StreamCell
        {
            private readonly T x;
            private readonly Lazy<StreamCell> s;

            public Cons(T x, Lazy<StreamCell> s)
            {
                this.x = x;
                this.s = s;
            }

            public T X { get { return this.x; } }
            public Lazy<StreamCell> S { get { return this.s; } }
        }

        private static readonly Lazy<StreamCell> NilStreamCell = new Lazy<StreamCell>(() => new Nil());

        public static Lazy<StreamCell> Empty { get { return NilStreamCell; } }

        public static Lazy<StreamCell> Append(Lazy<StreamCell> s1, Lazy<StreamCell> t)
        {
            var cons = s1.Value as Cons;
            if (cons == null) return t;
            return new Lazy<StreamCell>(() => new Cons(cons.X, Append(cons.S, t)));
        }

        public static Lazy<StreamCell> Take(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return Empty;
            var cons = s.Value as Cons;
            if (cons == null) return Empty;
            return new Lazy<StreamCell>(() => new Cons(cons.X, Take(n - 1, cons.S)));
        }

        private static Lazy<StreamCell> DropPrime(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return s;
            var cons = s.Value as Cons;
            if (cons == null) return Empty;
            return DropPrime(n - 1, cons.S);
        }

        public static Lazy<StreamCell> Drop(int n, Lazy<StreamCell> s)
        {
            return DropPrime(n, s);
        }

        private static Lazy<StreamCell> ReversePrime(Lazy<StreamCell> s, Lazy<StreamCell> r)
        {
            var cons = s.Value as Cons;
            if (cons == null) return Empty;
            var lazy = new Lazy<StreamCell>(() => new Cons(cons.X, r));
            return ReversePrime(cons.S, lazy);
        }

        public static Lazy<StreamCell> Reverse(Lazy<StreamCell> s)
        {
            return ReversePrime(s, Empty);
        }
    }
}
