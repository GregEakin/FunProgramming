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

using FunProgLib.lists;
using System;

namespace FunProgLib.queue
{
    // assumes polymorphic recursion!
    public static class BootstrappedQueue<T>
    {
        public sealed class Queue
        {
            private readonly int lenfm;

            private readonly List<T>.Node f;

            // alpha list susp Queue m
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

            public int LenFM
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

        // private static readonly List<T>.Node EmptyList = null;
        private static readonly Queue EmptyQueue = null;

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue == null;
        }

        private static Queue CheckQ(Queue q)
        {
            if (q.LenR <= q.LenFM) return CheckF(q);
            // return CheckF(new Queue(q.LenFM + q.LenR, q.F, Snoc(q.M, List<T>.Reverse(q.R)), 0, EmptyList));
            throw new System.NotImplementedException();
        }

        private static Queue CheckF(Queue q)
        {
            throw new System.NotImplementedException();
        }

        public static Queue Snoc(Queue queue, T item)
        {
            throw new System.NotImplementedException();
        }

        public static T Head(Queue queue)
        {
            throw new System.NotImplementedException();
        }

        public static Queue Tail(Queue queue)
        {
            throw new System.NotImplementedException();
        }
    }
}