// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

namespace FunProgLib.lists
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class RList<T> // : IStack<T>
    {
        public sealed class Node : IEnumerable<T>
        {
            public Node(T element, Node next)
            {
                Element = element;
                Next = next;
            }

            public T Element { get; }

            public Node Next { get; }

            public IEnumerator<T> GetEnumerator() => new ListEnum(this);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private sealed class ListEnum : IEnumerator<T>
            {
                private readonly Node _start;
                private Node _list;

                public ListEnum(Node list)
                {
                    _start = new Node(default(T), list);
                    _list = _start;
                }

                public bool MoveNext()
                {
                    if (IsEmpty(_list)) return false;
                    _list = _list.Next;
                    return !IsEmpty(_list);
                }

                public void Reset() => _list = _start;

                object IEnumerator.Current => _list.Element;

                public T Current => _list.Element;

                public void Dispose()
                { }
            }
        }

        public static Node Empty { get; } = null;

        public static bool IsEmpty(Node list) => list == Empty;

        public static Node Cons(T element, Node list) => new Node(element, list);

        public static T Head(Node list)
        {
            if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
            return list.Element;
        }

        public static Node Tail(Node list)
        {
            if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
            return list.Next;
        }

        public static Node Cat(Node list1, Node list2)
        {
            if (IsEmpty(list1)) return list2;
            if (IsEmpty(list2)) return list1;
            return new Node(list1.Element, Cat(list1.Next, list2));
        }

        public static Node Reverse(Node list)
        {
            if (IsEmpty(list)) return Empty;
            if (IsEmpty(list.Next)) return list;
            return Rev(list, Empty);
        }

        private static Node Rev(Node listIn, Node listOut)
        {
            if (IsEmpty(listIn)) return listOut;
            var next = new Node(Head(listIn), listOut);
            return Rev(Tail(listIn), next);
        }

        public static T Lookup(int i, Node list)
        {
            if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
            if (i < 0) throw new ArgumentException("neg", nameof(i));
            return i == 0
                ? Head(list)
                : Lookup(i - 1, Tail(list));
        }

        //public static Node Update(int i, T item, Node list)
        //{
        //    if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        //    if (i < 0) throw new ArgumentException("neg", nameof(i));
        //    return i == 0
        //        ? Cons(item, Tail(list))
        //        : Cons(Head(list), Update(i - 1, item, Tail(list)));
        //}

        public delegate T Del(T value);

        public static Node Fupdate(Del f, int i, Node ts)
        {
            if (IsEmpty(ts)) throw new ArgumentNullException(nameof(ts));
            if (i < 0) throw new ArgumentException("Negative", nameof(i));
            var head = Head(ts);
            var tail = Tail(ts);
            return i == 0
                ? Cons(f(head), tail)
                : Cons(head, Fupdate(f, i - 1, tail));
        }
    }
}
