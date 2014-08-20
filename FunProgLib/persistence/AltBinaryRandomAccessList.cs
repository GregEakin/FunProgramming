// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		AltBinaryRandomAccessList.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.persistence
{
    using System;

    public static class AltBinaryRandomAccessList<T>
    {
        public abstract class Digit
        {
            private readonly LinkList<Tuple<T, T>>.List list;

            protected Digit(LinkList<Tuple<T, T>>.List list)
            {
                this.list = list;
            }

            public LinkList<Tuple<T, T>>.List List
            {
                get { return this.list; }
            }
        }

        private sealed class Zero : Digit
        {
            public Zero(LinkList<Tuple<T, T>>.List list)
                : base(list)
            {
            }
        }

        private sealed class One : Digit
        {
            private readonly T alpha;

            public One(T alpha, LinkList<Tuple<T, T>>.List list)
                : base(list)
            {
                this.alpha = alpha;
            }

            public T Alpha
            {
                get { return this.alpha; }
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
            if (ts == null) return new One(x, null);
            var zero = ts as Zero;
            if (zero != null) return new One(x, zero.List);
            var one = ts as One;
            if (one != null) return new Zero(LinkList<Tuple<T, T>>.Cons(new Tuple<T, T>(x, one.Alpha), one.List));
            throw new Exception();
        }

        private static Tuple<T, Digit> Uncons(Digit digit)
        {
            if (digit == null) throw new Exception("Empty");

            var one = digit as One;
            if (one != null)
            {
                if (one.List == null) return new Tuple<T, Digit>(one.Alpha, null);
                return new Tuple<T, Digit>(one.Alpha, new Zero(one.List));
            }

            var zero = digit as Zero;
            if (zero != null)
            {
                // var (stuff, list) = Uncons(zero.List);
                var stuff = LinkList<Tuple<T, T>>.Head(zero.List);
                var list = LinkList<Tuple<T, T>>.Tail(zero.List);
                return new Tuple<T, Digit>(stuff.Item1, new One(stuff.Item2, list));
            }

            throw new Exception();
        }

        public static T Head(Digit ts)
        {
            var x = Uncons(ts);
            return x.Item1;
        }

        public static Digit Tail(Digit ts)
        {
            var x = Uncons(ts);
            return x.Item2;
        }

        public static T Lookup(int i, Digit ts)
        {
            if (ts == null) throw new Exception("Subscript");

            var one = ts as One;
            if (one != null)
            {
                if (i == 0) return one.Alpha;
                return Lookup(i - 1, new Zero(one.List));
            }

            var zero = ts as Zero;
            if (zero != null)
            {
                //var stuff = RList.Lookup(i / 2, Zero.List);
                //if (i % 2 == 0) return stuff.Alpha1;
                //return stuff.Alpha2;
                throw new NotImplementedException();
            }

            throw new Exception();
        }

        private delegate T Del(T value);

        private static Digit Fupdate(Del f, int i, Digit ts)
        {
            if (ts == null) throw new Exception("Subscript");
            var one = ts as One;
            if (one != null)
            {
                if (i == 0) return new One(f(one.Alpha), one.List);
                return Cons(one.Alpha, Fupdate(f, i - 1, new Zero(one.List)));
            }

            var zero = ts as Zero;
            if (zero != null)
            {
                // var fp(x,y) = i % 2 == 0 ? (f(x) y) : (x, f(y));
                var fp = i % 2 == 0
                    ? new Func<Tuple<T, T>, Del, Tuple<T, T>>((Tuple<T, T> stuff, Del g) => new Tuple<T, T>(g(stuff.Item1), stuff.Item2))
                    : new Func<Tuple<T, T>, Del, Tuple<T, T>>((Tuple<T, T> stuff, Del g) => new Tuple<T, T>(stuff.Item1, g(stuff.Item2)));
                //return new Zero(Fupdate(fp, i / 2, zero.List));
                throw new NotImplementedException();
            }

            throw new Exception();
        }

        public static Digit Update(int i, T x, Digit ts)
        {
            return Fupdate(y => x, i, ts);
        }
    }
}