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
    public class List2<T> : IEnumerable<T>
    {
        private static readonly List2<T> EmptyList = new List2<T>(default(T), null);

        public static List2<T> Empty
        {
            get { return EmptyList; }
        }

        private readonly T _head;

        private readonly List2<T> _tail;

        public List2(T element)
            : this(element, Empty)
        {
        }

        private List2(T element, List2<T> list)
        {
            _head = element;
            _tail = list;
        }

        public bool IsEmpty
        {
            get { return this == EmptyList; }
        }

        public List2<T> Cons(T element)
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

        public List2<T> Tail
        {
            get
            {
                if (IsEmpty) throw new Exception("Empty");
                return _tail;
            }
        }

        public List2<T> Cat(List2<T> list2)
        {
            if (IsEmpty) return list2;
            return list2.IsEmpty
                ? this
                : new List2<T>(_head, _tail.Cat(list2));
        }

        public List2<T> Reverse
        {
            get
            {
                if (IsEmpty) return Empty;
                return _tail.IsEmpty
                    ? this
                    : Rev(this, Empty);
            }
        }

        private static List2<T> Rev(List2<T> listIn, List2<T> listOut)
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
                : Tail.Lookup(i - 1);
        }

        public delegate T Fun(T value);

        public List2<T> Fupdate(Fun f, int i)
        {
            if (IsEmpty) throw new Exception("Empty");
            return i == 0
                ? Tail.Cons(f(Head))
                : Tail.Fupdate(f, i - 1).Cons(Head);
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
            private readonly List2<T> _start;
            private List2<T> _list;

            public ListEnum(List2<T> list)
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