// Fun Programming Data Structures 1.0
// 
// Copyright © 2016 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.2.1 Lists With Efficient Catenation." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 153-8. Print.

namespace FunProgLib.lists
{
    using queue;
    using System;

    public static class CatenableList<T>
    {
        public sealed class C
        {
            public C(T x, BootstrappedQueue<Lazy<C>>.Queue xs)
            {
                X = x;
                Q = xs;
            }

            public T X { get; }
            public BootstrappedQueue<Lazy<C>>.Queue Q { get; }
        }

        public static C Empty { get; } = null;

        public static bool IsEmpty(C list) => list == Empty;

        private static C Link(C xs, Lazy<C> s) => new C(xs.X, BootstrappedQueue<Lazy<C>>.Snoc(xs.Q, s));

        private static C LinkAll(BootstrappedQueue<Lazy<C>>.Queue q)
        {
            var t = BootstrappedQueue<Lazy<C>>.Head(q).Value;
            var qp = BootstrappedQueue<Lazy<C>>.Tail(q);
            if (BootstrappedQueue<Lazy<C>>.IsEmpty(qp)) return t;
            return Link(t, new Lazy<C>(() => LinkAll(qp)));
        }

        public static C Cat(C xs, C ys)
        {
            if (IsEmpty(ys)) return xs;
            if (IsEmpty(xs)) return ys;
            return Link(xs, new Lazy<C>(() => ys));
        }

        public static C Cons(T x, C xs) => Cat(new C(x, BootstrappedQueue<Lazy<C>>.Empty), xs);

        public static C Snoc(C xs, T x) => Cat(xs, new C(x, BootstrappedQueue<Lazy<C>>.Empty));

        public static T Head(C c)
        {
            if (IsEmpty(c)) throw new ArgumentException("Empty", nameof(c));
            return c.X;
        }

        public static C Tail(C c)
        {
            if (c == null) throw new ArgumentException("Empty", nameof(c));
            if (BootstrappedQueue<Lazy<C>>.IsEmpty(c.Q)) return Empty;
            return LinkAll(c.Q);
        }
    }
}
