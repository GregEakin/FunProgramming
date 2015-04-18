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
    public class AltBinaryRandomAccessList<T>
    {
        public delegate T Fun(T value);

        private static readonly AltBinaryRandomAccessList<T> EmptyList = new AltBinaryRandomAccessList<T>();

        public static AltBinaryRandomAccessList<T> Empty
        {
            get { return EmptyList; }
        }

        private readonly List2<Tuple<T, T>> _rlist;

        protected AltBinaryRandomAccessList()
            : this(List2<Tuple<T, T>>.Empty)
        {
        }

        protected AltBinaryRandomAccessList(List2<Tuple<T, T>> rlist)
        {
            _rlist = rlist;
        }

        protected List2<Tuple<T, T>> List
        {
            get { return _rlist; }
        }

        public virtual bool IsEmpty
        {
            get { return true; }
        }

        public virtual AltBinaryRandomAccessList<T> Cons(T x)
        {
            return new AltBinaryRandomAccessOne<T>(x, List2<Tuple<T, T>>.Empty);
        }

        public virtual Tuple<T, AltBinaryRandomAccessList<T>> Uncons
        {
            get { throw new Exception("Empty"); }
        }

        public virtual T Head
        {
            get { throw new Exception("Empty"); }
        }

        public virtual AltBinaryRandomAccessList<T> Tail
        {
            get { throw new Exception("Empty"); }
        }

        public virtual T Lookup(int i)
        {
            throw new Exception("Empty");
        }

        public virtual AltBinaryRandomAccessList<T> Fupdate(Fun f, int i)
        {
            throw new Exception("Subscript");
        }

        public AltBinaryRandomAccessList<T> Update(int i, T y)
        {
            return Fupdate(x => y, i);
        }
    }
}
