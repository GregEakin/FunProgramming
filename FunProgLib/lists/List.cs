﻿// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IList.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.lists
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class List<T> // : Stack<T>
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
                get { return this.element; }
            }

            public Node Next
            {
                get { return this.next; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new ListEnum(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
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
                    get { return this.list.element; }
                }

                public T Current
                {
                    get { return this.list.element; }
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