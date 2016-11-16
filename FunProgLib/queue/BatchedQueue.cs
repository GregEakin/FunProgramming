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

    using lists;

    public static class BatchedQueue<T>
    {
        public sealed class Queue
        {
            public Queue(List<T>.Node f, List<T>.Node r)
            {
                F = f;
                R = r;
            }

            public List<T>.Node F { get; }
            public List<T>.Node R { get; }
        }

        public static Queue Empty { get; } = new Queue(List<T>.Empty, List<T>.Empty);

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