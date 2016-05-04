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

namespace FunProgLib.lists
{
    using System;

    // assumes polymorphic recursion!
    public static class AltBinaryRandomAccessList<T>
    {
        public abstract class DataType
        {
            protected DataType(RList<Tuple<T, T>>.Node list)
            {
                RList = list;
            }

            public RList<Tuple<T, T>>.Node RList { get; }
        }

        public sealed class Zero : DataType
        {
            public Zero(RList<Tuple<T, T>>.Node list)
                : base(list)
            {
            }
        }

        public sealed class One : DataType
        {
            public One(T alpha, RList<Tuple<T, T>>.Node list)
                : base(list)
            {
                Alpha = alpha;
            }

            public T Alpha { get; }
        }

        public static DataType Empty => null;

        public static bool IsEmpty(DataType list)
        {
            return list == null;
        }

        public static DataType Cons(T x, DataType ts)
        {
            if (ts == null) return new One(x, null);

            var zero = ts as Zero;
            if (zero != null) return new One(x, zero.RList);

            var one = ts as One;
            if (one != null) return new Zero(RList<Tuple<T, T>>.Cons(new Tuple<T, T>(x, one.Alpha), one.RList));

            throw new ArgumentException("must be null, Zero or One", nameof(ts));
        }

        private static Tuple<T, DataType> Uncons(DataType dataType)
        {
            var one = dataType as One;
            if (one != null)
            {
                if (one.RList == null) return new Tuple<T, DataType>(one.Alpha, null);
                return new Tuple<T, DataType>(one.Alpha, new Zero(one.RList));
            }

            var zero = dataType as Zero;
            if (zero != null)
            {
                var xy = RList<Tuple<T, T>>.Head(zero.RList);
                var psp = RList<Tuple<T, T>>.Tail(zero.RList);
                return new Tuple<T, DataType>(xy.Item1, new One(xy.Item2, psp));
            }

            throw new ArgumentException("must be Zero or One", nameof(dataType));
        }

        public static T Head(DataType ts)
        {
            var x = Uncons(ts);
            return x.Item1;
        }

        public static DataType Tail(DataType ts)
        {
            var x = Uncons(ts);
            return x.Item2;
        }

        public static T Lookup(int i, DataType ts)
        {
            var one = ts as One;
            if (one != null)
            {
                if (i == 0) return one.Alpha;
                return Lookup(i - 1, new Zero(one.RList));
            }

            var zero = ts as Zero;
            if (zero != null)
            {
                var node = RList<Tuple<T, T>>.Lookup(i / 2, zero.RList);
                if (i % 2 == 0) return node.Item1;
                return node.Item2;
            }

            throw new ArgumentException("must be Zero or One", nameof(ts));
        }

        public delegate T Del(T value);

        public static DataType Fupdate(Del f, int i, DataType ts)
        {
            var one = ts as One;
            if (one != null)
            {
                if (i == 0) return new One(f(one.Alpha), one.RList);
                return Cons(one.Alpha, Fupdate(f, i - 1, new Zero(one.RList)));
            }

            var zero = ts as Zero;
            if (zero != null)
            {
                RList<Tuple<T, T>>.Del fp0 = value => new Tuple<T, T>(f(value.Item1), value.Item2);
                RList<Tuple<T, T>>.Del fp1 = value => new Tuple<T, T>(value.Item1, f(value.Item2));
                var fp = i % 2 == 0 ? fp0 : fp1;
                return new Zero(RList<Tuple<T, T>>.Fupdate(fp, i / 2, zero.RList));
            }

            throw new ArgumentException("must be Zero or One", nameof(ts));
        }

        public static DataType Update(int i, T y, DataType xs)
        {
            return Fupdate(x => y, i, xs);
        }
    }
}
