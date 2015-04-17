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

        private readonly T head;

        private readonly IStack<T> tail;

        public List2(T head)
            : this(head, Empty)
        {
        }

        private List2(T head, IStack<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }

        public bool IsEmpty
        {
            get { return this == EmptyList; }
        }

        public IStack<T> Cons(T item)
        {
            return new List2<T>(item, this);
        }

        public T Head
        {
            get { return head; }
        }

        public IStack<T> Tail
        {
            get { return tail; }
        }

        public IStack<T> Cat(List2<T> list2)
        {
            if (IsEmpty) return list2;
            if (list2.IsEmpty) return this;
            return new List2<T>(head, list2.Cat((List2<T>)tail));
        }

        public IStack<T> Reverse()
        {
            if (IsEmpty) return Empty;
            if (tail.IsEmpty) return this;
            return Rev(this, Empty);
        }

        private static IStack<T> Rev(IStack<T> listIn, IStack<T> listOut)
        {
            if (listIn.IsEmpty) return listOut;
            var next = new List2<T>(listIn.Head, listOut);
            return Rev(listIn.Tail, next);
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
            private readonly IStack<T> start;
            private IStack<T> list;

            public ListEnum(IStack<T> list)
            {
                this.start = list.Cons(default(T));
                this.list = start;
            }

            public bool MoveNext()
            {
                if (list.IsEmpty) return false;
                list = list.Tail;
                return !list.IsEmpty;
            }

            public void Reset()
            {
                list = start;
            }

            object IEnumerator.Current
            {
                get { return list.Head; }
            }

            public T Current
            {
                get { return list.Head; }
            }

            public void Dispose()
            {
            }
        }
    }
}