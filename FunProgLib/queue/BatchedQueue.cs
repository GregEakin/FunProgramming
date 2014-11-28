// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgLib.queue
{
    using System;

    using FunProgLib.lists;

    public static class BatchedQueue<T>
    {
        public sealed class Queue
        {
            private readonly List<T>.Node f;
            private readonly List<T>.Node r;

            public Queue(List<T>.Node f, List<T>.Node r)
            {
                this.f = f;
                this.r = r;
            }

            public List<T>.Node F { get { return f; } }
            public List<T>.Node R { get { return r; } }
        }

        private static readonly List<T>.Node EmptyList = null; // new List<T>.Node(new T[0]);

        private static readonly Queue EmptyQueue = new Queue(EmptyList, EmptyList);

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            return q.F == EmptyList;
        }

        private static Queue CheckF(List<T>.Node f, List<T>.Node r)
        {
            if (f == EmptyList) return new Queue(List<T>.Reverse(r), EmptyList);
            return new Queue(f, r);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return CheckF(q.F, List<T>.Cons(x, q.R));
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