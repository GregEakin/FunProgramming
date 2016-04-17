// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "5.2 Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 42-45. Print.

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

        private static readonly Queue EmptyQueue = new Queue(List<T>.Empty, List<T>.Empty);

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            return List<T>.IsEmpty(q.F);
        }

        private static Queue CheckF(List<T>.Node f, List<T>.Node r)
        {
            if (List<T>.IsEmpty(f)) return new Queue(List<T>.Reverse(r), List<T>.Empty);
            return new Queue(f, r);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return CheckF(q.F, List<T>.Cons(x, q.R));
        }

        public static T Head(Queue q)
        {
            if (List<T>.IsEmpty(q.F)) throw new ArgumentException("Empty", nameof(q));
            return q.F.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (List<T>.IsEmpty(q.F)) throw new ArgumentException("Empty", nameof(q));
            return CheckF(q.F.Next, q.R);
        }
    }
}