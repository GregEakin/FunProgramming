// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

using System;
using System.Collections;
using System.Collections.Generic;

namespace FunProgLib.lists
{
    public class List2<T> : IStack<T>, IEnumerable<T>
    {
        private static readonly IStack<T> EmptyList = new List2<T>(default(T), null);

        public static IStack<T> Empty
        {
            get { return EmptyList; }
        }

        private readonly T _head;

        private readonly IStack<T> _tail;

        public List2(T element)
            : this(element, Empty)
        {
        }

        private List2(T element, IStack<T> list)
        {
            _head = element;
            _tail = list;
        }

        public bool IsEmpty
        {
            get { return this == EmptyList; }
        }

        public IStack<T> Cons(T element)
        {
            return new List2<T>(element, this);
        }

        public T Head
        {
            get
            {
                if (IsEmpty) throw new Exception("Empty");
                return _head;
            }
        }

        public IStack<T> Tail
        {
            get
            {
                if (IsEmpty) throw new Exception("Empty");
                return _tail;
            }
        }

        public IStack<T> Cat(IStack<T> list2)
        {
            if (IsEmpty) return list2;
            if (list2.IsEmpty) return this;
            return new List2<T>(_head, ((List2<T>)_tail).Cat(list2));
        }

        public IStack<T> Reverse
        {
            get
            {
                if (IsEmpty) return Empty;
                if (_tail.IsEmpty) return this;
                return Rev(this, Empty);
            }
        }

        private static IStack<T> Rev(IStack<T> listIn, IStack<T> listOut)
        {
            while (!listIn.IsEmpty)
            {
                var next = new List2<T>(listIn.Head, listOut);
                listIn = listIn.Tail;
                listOut = next;
            }

            return listOut;
        }

        public T Lookup(int i)
        {
            if (IsEmpty) throw new Exception("Empty");
            return i == 0
                ? Head
                : ((List2<T>)Tail).Lookup(i - 1);
        }

        public delegate T Fun(T value);

        public IStack<T> Fupdate(Fun f, int i)
        {
            if (IsEmpty) throw new Exception("Empty");
            return i == 0
                ? Tail.Cons(f(Head))
                : ((List2<T>)Tail).Fupdate(f, i - 1).Cons(Head);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnum(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private sealed class ListEnum : IEnumerator<T>
        {
            private readonly IStack<T> _start;
            private IStack<T> _list;

            public ListEnum(IStack<T> list)
            {
                _start = list.Cons(default(T));
                _list = _start;
            }

            public bool MoveNext()
            {
                if (_list.IsEmpty) return false;
                _list = _list.Tail;
                return !_list.IsEmpty;
            }

            public void Reset()
            {
                _list = _start;
            }

            object IEnumerator.Current
            {
                get { return _list.Head; }
            }

            public T Current
            {
                get { return _list.Head; }
            }

            public void Dispose()
            {
            }
        }
    }
}