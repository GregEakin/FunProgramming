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

    public static class LinkList<T> // : Stack<T>
    {
        public sealed class List : IEnumerable<T>
        {
            private readonly T element;

            private readonly List next;

            public List(T element, List next)
            {
                this.element = element;
                this.next = next;
            }

            public T Element
            {
                get { return this.element; }
            }

            public List Next
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
                private readonly List start;
                private List list;

                public ListEnum(List list)
                {
                    this.start = new List(default(T), list);
                    this.list = this.start;
                }

                public bool MoveNext()
                {
                    if (this.list == EmptyList) return false;
                    this.list = this.list.Next;
                    return this.list != EmptyList;
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

        private static readonly List EmptyList = null;

        public static List Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmpty(List list)
        {
            return list == EmptyList;
        }

        public static List Cons(T element, List list)
        {
            return new List(element, list);
        }

        public static T Head(List list)
        {
            if (IsEmpty(list))
                throw new Exception("Empty");

            return list.Element;
        }

        public static List Tail(List list)
        {
            if (IsEmpty(list))
                throw new Exception("Empty");

            return list.Next;
        }

        public static List Cat(List list1, List list2)
        {
            if (IsEmpty(list1)) return list2;
            if (IsEmpty(list2)) return list1;
            return new List(list1.Element, Cat(list1.Next, list2));
        }

        public static List Reverse1(List list)
        {
            if (IsEmpty(list)) return Empty;
            if (IsEmpty(list.Next)) return list;

            // return list.Aggregate(Empty, (current, element) => Cons(element, current));
            var result = Empty;
            foreach (var element in list)
            {
                result = Cons(element, result);
            }

            return result;
        }


        public static List Reverse2(List list)
        {
            if (IsEmpty(list)) return Empty;
            if (IsEmpty(list.Next)) return list;

            var result = Empty;
            while (!IsEmpty(list))
            {
                result = Cons(Head(list), result);
                list = Tail(list);
            }

            return result;
        }

        public static List Reverse(List list)
        {
            if (IsEmpty(list)) return Empty;
            if (IsEmpty(list.Next)) return list;
            return Rev(list, Empty);
        }

        private static List Rev(List listIn, List listOut)
        {
            if (IsEmpty(listIn)) return listOut;
            var next = new List(Head(listIn), listOut);
            return Rev(Tail(listIn), next);
        }
    }
}