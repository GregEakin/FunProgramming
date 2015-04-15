// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.1.3 Bootstrapped Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 146-50. Print.

using System;
using FunProgLib.lists;

namespace FunProgLib.queue
{
    // assumes polymorphic recursion!
    public static class BootstrappedQueue<T>  // : Queue<T>
    {
        public sealed class Queue
        {
            private readonly int lenfm;

            private readonly List<T>.Node f;

            private readonly BootstrappedQueue<Lazy<List<T>.Node>>.Queue m;

            private readonly int lenr;

            private readonly List<T>.Node r;

            public Queue(int lenfm, List<T>.Node f, BootstrappedQueue<Lazy<List<T>.Node>>.Queue m, int lenr, List<T>.Node r)
            {
                this.lenfm = lenfm;
                this.f = f;
                this.m = m;
                this.lenr = lenr;
                this.r = r;
            }

            public int LenFm
            {
                get { return lenfm; }
            }

            public List<T>.Node F
            {
                get { return f; }
            }

            public BootstrappedQueue<Lazy<List<T>.Node>>.Queue M
            {
                get { return m; }
            }

            public int LenR
            {
                get { return lenr; }
            }

            public List<T>.Node R
            {
                get { return r; }
            }
        }

        private static readonly List<T>.Node EmptyList = null;
        private static readonly Queue EmptyQueue = null;

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue == null;
        }

        private static Queue CheckQ(Queue queue)
        {
            if (IsEmpty(queue)) throw new Exception("Empty");
            if (queue.LenR <= queue.LenFm) return CheckF(queue);
            var m = BootstrappedQueue<Lazy<List<T>.Node>>.Snoc(queue.M, new Lazy<List<T>.Node>(() => List<T>.Reverse(queue.R)));
            var q = new Queue(queue.LenFm + queue.LenR, queue.F, m, 0, EmptyList);
            return CheckF(q);
        }

        private static Queue CheckF(Queue queue)
        {
            if (IsEmpty(queue)) throw new Exception("Empty");
            if (!List<T>.IsEmpty(queue.F)) return queue;
            if (BootstrappedQueue<Lazy<List<T>.Node>>.IsEmpty(queue.M)) return Empty;
            var headM = BootstrappedQueue<Lazy<List<T>.Node>>.Head(queue.M).Value;
            var tailM = BootstrappedQueue<Lazy<List<T>.Node>>.Tail(queue.M);
            return new Queue(queue.LenFm, headM, tailM, queue.LenR, queue.R);
        }

        public static Queue Snoc(Queue queue, T x)
        {
            return IsEmpty(queue)
                ? new Queue(1, List<T>.Cons(x, List<T>.Empty), BootstrappedQueue<Lazy<List<T>.Node>>.Empty, 0, List<T>.Empty)
                : CheckQ(new Queue(queue.LenFm, queue.F, queue.M, queue.LenR + 1, List<T>.Cons(x, queue.R)));
        }

        public static T Head(Queue queue)
        {
            if (IsEmpty(queue)) throw new Exception("Empty");
            return List<T>.Head(queue.F);
        }

        public static Queue Tail(Queue queue)
        {
            if (IsEmpty(queue)) throw new Exception("Empty");
            return CheckQ(new Queue(queue.LenFm - 1, List<T>.Tail(queue.F), queue.M, queue.LenR, queue.R));
        }
    }
}