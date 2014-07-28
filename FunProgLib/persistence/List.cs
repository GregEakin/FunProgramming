// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IList.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.persistence
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class List<T> // : Stack<T>
    {
        public sealed class ListStructure : IEnumerable<T>
        {
            private readonly T element;

            private readonly ListStructure next;

            public ListStructure(ListStructure next, T element)
            {
                this.element = element;
                this.next = next;
            }

            public T Element
            {
                get { return element; }
            }

            public ListStructure Next
            {
                get { return next; }
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
                private readonly ListStructure start;
                private ListStructure list;

                public ListEnum(ListStructure list)
                {
                    this.start = new ListStructure(list, default(T));
                    this.list = start;
                }

                public bool MoveNext()
                {
                    if (list == EmptyList) return false;
                    list = list.Next;
                    return list != EmptyList;
                }

                public void Reset()
                {
                    list = start;
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

        private static readonly ListStructure EmptyList = null;

        public static ListStructure Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmpty(ListStructure list)
        {
            return list == EmptyList;
        }

        public static ListStructure Cons(ListStructure list, T element)
        {
            return new ListStructure(list, element);
        }

        public static T Head(ListStructure list)
        {
            if (list == EmptyList)
                throw new Exception("Empty");

            return list.Element;
        }

        public static ListStructure Tail(ListStructure list)
        {
            if (list == EmptyList)
                throw new Exception("Empty");

            return list.Next;
        }
    }
}