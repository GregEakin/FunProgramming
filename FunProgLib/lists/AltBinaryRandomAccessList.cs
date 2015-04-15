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
    // assumes polymorphic recursion!
    public static class AltBinaryRandomAccessList<T>
    {
        // datatype alpha RList = Nil | Zero of (alpha x alpha) RList | One of alpha x (alpha x alpha) RList

        public delegate T Fun(T value);
        // public delegate Tuple<T, T> Fun2(Fun g, Tuple<T, T> t);

        public abstract class Digit
        {
            private readonly RList<Tuple<T, T>>.Node rlist;

            protected Digit(RList<Tuple<T, T>>.Node rlist)
            {
                this.rlist = rlist;
            }

            protected RList<Tuple<T, T>>.Node RList
            {
                get { return rlist; }
            }

            public abstract Digit Cons(T x);

            public abstract Tuple<T, Digit> Uncons();

            public abstract T Lookup(int i);

            public abstract Digit Fupdate(Fun f, int i);
        }

        private sealed class Zero : Digit
        {
            public Zero(RList<Tuple<T, T>>.Node rlist)
                : base(rlist)
            {
            }

            public override Digit Cons(T x)
            {
                return new One(x, RList);
            }

            public override Tuple<T, Digit> Uncons()
            {
                var stuff = RList<Tuple<T, T>>.Head(RList);
                var list = RList<Tuple<T, T>>.Tail(RList);
                return new Tuple<T, Digit>(stuff.Item1, new One(stuff.Item2, list));
            }

            public override T Lookup(int i)
            {
                var stuff = RList<Tuple<T, T>>.Lookup(i / 2, RList);
                return i % 2 == 0
                    ? stuff.Item1
                    : stuff.Item2;
            }

            public override Digit Fupdate(Fun f, int i)
            {
                var fp = i % 2 == 0
                    ? new RList<Tuple<T, T>>.Fun(t => new Tuple<T, T>(f(t.Item1), t.Item2))
                    : new RList<Tuple<T, T>>.Fun(t => new Tuple<T, T>(t.Item1, f(t.Item2)));
                var list = RList<Tuple<T, T>>.Fupdate(fp, i / 2, RList);
                return new Zero(list);
            }
        }

        private sealed class One : Digit
        {
            private readonly T alpha;

            public One(T alpha, RList<Tuple<T, T>>.Node rlist)
                : base(rlist)
            {
                this.alpha = alpha;
            }

            public override Digit Cons(T x)
            {
                var list = RList<Tuple<T, T>>.Cons(new Tuple<T, T>(x, alpha), RList);
                return new Zero(list);
            }

            public override Tuple<T, Digit> Uncons()
            {
                return RList == null
                    ? new Tuple<T, Digit>(alpha, null)
                    : new Tuple<T, Digit>(alpha, new Zero(RList));
            }

            public override T Lookup(int i)
            {
                return i == 0
                    ? alpha
                    : new Zero(RList).Lookup(i - 1);
            }

            public override Digit Fupdate(Fun f, int i)
            {
                return i == 0
                    ? new One(f(alpha), RList)
                    : new Zero(RList).Fupdate(f, i - 1).Cons(alpha);
            }
        }

        public static Digit Empty
        {
            get { return null; }
        }

        public static bool IsEmpty(Digit list)
        {
            return list == null;
        }

        public static Digit Cons(T x, Digit ts)
        {
            return ts == null
                ? new One(x, null)
                : ts.Cons(x);
        }

        public static Tuple<T, Digit> Uncons(Digit ts)
        {
            if (ts == null) throw new Exception("Empty");
            return ts.Uncons();
        }

        public static T Head(Digit xs)
        {
            if (xs == null) throw new Exception("Empty");
            var x = xs.Uncons();
            return x.Item1;
        }

        public static Digit Tail(Digit xs)
        {
            if (xs == null) throw new Exception("Empty");
            var x = xs.Uncons();
            return x.Item2;
        }

        public static T Lookup(int i, Digit ts)
        {
            if (ts == null) throw new Exception("Empty");
            return ts.Lookup(i);
        }

        public static Digit Fupdate(Fun f, int i, Digit ts)
        {
            if (ts == null) throw new Exception("Subscript");
            return ts.Fupdate(f, i);
        }

        public static Digit Update(int i, T y, Digit ts)
        {
            return ts.Fupdate(x => y, i);
        }
    }
}
