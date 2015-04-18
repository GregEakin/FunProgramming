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
    public abstract class AltBinaryRandomAccessList<T>
    {
        public delegate T Fun(T value);

        private static readonly AltBinaryRandomAccessList<T> EmptyList = new AltBinaryRandomAccessEmpty<T>();

        public static AltBinaryRandomAccessList<T> Empty
        {
            get { return EmptyList; }
        }

        private readonly List2<Tuple<T, T>> _rlist;

        protected AltBinaryRandomAccessList(List2<Tuple<T, T>> rlist)
        {
            _rlist = rlist;
        }

        protected List2<Tuple<T, T>> List
        {
            get { return _rlist; }
        }

        public abstract bool IsEmpty { get; }

        public abstract AltBinaryRandomAccessList<T> Cons(T x);

        public abstract Tuple<T, AltBinaryRandomAccessList<T>> Uncons { get; }

        public abstract T Head { get; }

        public abstract AltBinaryRandomAccessList<T> Tail { get; }

        public abstract T Lookup(int i);

        public abstract AltBinaryRandomAccessList<T> Fupdate(Fun f, int i);

        public AltBinaryRandomAccessList<T> Update(int i, T y)
        {
            return Fupdate(x => y, i);
        }
    }

    internal class AltBinaryRandomAccessEmpty<T> : AltBinaryRandomAccessList<T>
    {
        internal AltBinaryRandomAccessEmpty()
            : base(List2<Tuple<T, T>>.Empty)
        {
        }

        public override bool IsEmpty
        {
            get { return true; }
        }

        public override AltBinaryRandomAccessList<T> Cons(T x)
        {
            return new AltBinaryRandomAccessOne<T>(x, List2<Tuple<T, T>>.Empty);
        }

        public override Tuple<T, AltBinaryRandomAccessList<T>> Uncons
        {
            get { throw new Exception("Empty"); }
        }

        public override T Head
        {
            get { throw new Exception("Empty"); }
        }

        public override AltBinaryRandomAccessList<T> Tail
        {
            get { throw new Exception("Empty"); }
        }

        public override T Lookup(int i)
        {
            throw new Exception("Empty");
        }

        public override AltBinaryRandomAccessList<T> Fupdate(Fun f, int i)
        {
            throw new Exception("Subscript");
        }
    }

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
            get { return Uncons.Item1; }
        }

        public override AltBinaryRandomAccessList<T> Tail
        {
            get { return Uncons.Item2; }
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
            get { return Uncons.Item1; }
        }

        public override AltBinaryRandomAccessList<T> Tail
        {
            get { return Uncons.Item2; }
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
