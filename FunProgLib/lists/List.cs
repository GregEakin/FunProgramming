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
    public static class List<T> // : IStack<T>
    {
        public sealed class Node : IEnumerable<T>
        {
            private readonly T element;

            private readonly Node next;

            public Node(T element, Node next)
            {
                this.element = element;
                this.next = next;
            }

            public T Element
            {
                get { return element; }
            }

            public Node Next
            {
                get { return next; }
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
                private readonly Node start;
                private Node list;

                public ListEnum(Node list)
                {
                    this.start = new Node(default(T), list);
                    this.list = this.start;
                }

                public bool MoveNext()
                {
                    if (IsEmpty(this.list)) return false;
                    this.list = this.list.Next;
                    return !IsEmpty(this.list);
                }

                public void Reset()
                {
                    this.list = this.start;
                }

                object IEnumerator.Current
                {
                    get { return list.element; }
                }

                public T Current
                {
                    get { return list.element; }
                }

                public void Dispose()
                {
                }
            }
        }

        private static readonly Node EmptyList = null;

        public static Node Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmpty(Node list)
        {
            return list == EmptyList;
        }

        public static Node Cons(T element, Node list)
        {
            return new Node(element, list);
        }

        public static T Head(Node list)
        {
            if (IsEmpty(list)) throw new Exception("Empty");
            return list.Element;
        }

        public static Node Tail(Node list)
        {
            if (IsEmpty(list)) throw new Exception("Empty");
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
    }
}