// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists
{
    using System;

    public class BinaryRandomAccessList<T>
    {
        private static readonly BinaryRandomAccessList<T> EmptyList = new BinaryRandomAccessList<T>();

        public static BinaryRandomAccessList<T> Empty
        {
            get { return EmptyList; }
        }

        private readonly List2<BinaryRandomAccessDigit<T>> _list;

        private BinaryRandomAccessList()
        {
            _list = null;
            throw new NotImplementedException();
        }

        //public virtual bool IsEmpty()
        //{
        //    return true;
        //}

        //private readonly Tree<T> _one;

        //internal Tree<T> One
        //{
        //    get { return _one; }
        //}


        //private Tree<T> Link(Tree<T> t1)
        //{
        //    return new Node<T>(t1.Size, t1, null);
        //}

        internal virtual BinaryRandomAccessList<T> ConsTree(Tree<T> t)
        {
            return new BinaryRandomAccessList<T>(_list.ConsTree(t));
        }

        internal virtual Tuple<Tree<T>, BinaryRandomAccessList<T>> UnconsTree
        {
            get { return _list.UnconsTree; }
        }

        public virtual BinaryRandomAccessList<T> Cons(T x)
        {
            return new BinaryRandomAccessList<T>(_list.Cons(x));
        }

        public virtual new T Head
        {
            get { return _list.Head; }
        }

        public virtual new BinaryRandomAccessList<T> Tail
        {
            get { return _list.Tail; }
        }

        public virtual new T Lookup(int i)
        {
            return _list.Lookup(i);
        }

        public virtual BinaryRandomAccessList<T> Update(int i, T x)
        {
            return _list.Update(i, x);
        }
    }
}