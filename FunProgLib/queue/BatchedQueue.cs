// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BatchedQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;

    using FunProgLib.persistence;

    public static class BatchedQueue<T>
    {
        public sealed class Queue
        {
            private readonly LinkList<T>.List f;
            private readonly LinkList<T>.List r;

            public Queue(LinkList<T>.List f, LinkList<T>.List r)
            {
                this.f = f;
                this.r = r;
            }

            public LinkList<T>.List F { get { return f; } }
            public LinkList<T>.List R { get { return r; } }
        }

        private static readonly LinkList<T>.List EmptyList = null; // new LinkList<T>.List(new T[0]);

        private static readonly Queue EmptyQueue = new Queue(EmptyList, EmptyList);

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            return q.F == EmptyList;
        }

        private static Queue CheckF(LinkList<T>.List f, LinkList<T>.List r)
        {
            if (f == EmptyList) return new Queue(LinkList<T>.Reverse(r), EmptyList);
            return new Queue(f, r);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return CheckF(q.F, LinkList<T>.Cons(x, q.R));
        }

        public static T Head(Queue q)
        {
            if (q.F == EmptyList) throw new Exception("Empty");
            return q.F.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == EmptyList) throw new Exception("Empty");
            return CheckF(q.F.Next, q.R);
        }
    }
}