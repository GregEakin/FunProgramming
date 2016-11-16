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
            public Queue(int lenfm, List<T>.Node f, BootstrappedQueue<Lazy<List<T>.Node>>.Queue m, int lenr, List<T>.Node r)
            {
                LenFM = lenfm;
                F = f;
                M = m;
                LenR = lenr;
                R = r;
            }

            public int LenFM { get; }

            public List<T>.Node F { get; }

            public BootstrappedQueue<Lazy<List<T>.Node>>.Queue M { get; }

            public int LenR { get; }

            public List<T>.Node R { get; }
        }

        public static Queue Empty { get; } = null;

        public static bool IsEmpty(Queue queue)
        {
            return queue == Empty;
        }

        private static Queue CheckQ(int lenfm, List<T>.Node f, BootstrappedQueue<Lazy<List<T>.Node>>.Queue m, int lenr, List<T>.Node r)
        {
            if (lenr <= lenfm) return CheckF(lenfm, f, m, lenr, r);
            return CheckF(lenfm + lenr, f, BootstrappedQueue<Lazy<List<T>.Node>>.Snoc(m, new Lazy<List<T>.Node>(() => List<T>.Reverse(r))), 0, List<T>.Empty);
        }

        private static Queue CheckF(int lenfm, List<T>.Node f, BootstrappedQueue<Lazy<List<T>.Node>>.Queue m, int lenr, List<T>.Node r)
        {
            if (List<T>.IsEmpty(f) && m == null) return Empty;
            if (List<T>.IsEmpty(f)) return new Queue(lenfm, BootstrappedQueue<Lazy<List<T>.Node>>.Head(m).Value, BootstrappedQueue<Lazy<List<T>.Node>>.Tail(m), lenr, r);
            return new Queue(lenfm, f, m, lenr, r);
        }

        public static Queue Snoc(Queue queue, T item)
        {
            if (queue == Empty) return new Queue(1, new List<T>.Node(item, List<T>.Empty), null, 0, List<T>.Empty);
            return CheckQ(queue.LenFM, queue.F, queue.M, queue.LenR + 1, List<T>.Cons(item, queue.R));
        }

        public static T Head(Queue queue)
        {
            if (queue == Empty) throw new ArgumentNullException(nameof(queue));
            return List<T>.Head(queue.F);
        }

        public static Queue Tail(Queue queue)
        {
            if (queue == Empty) throw new ArgumentNullException(nameof(queue));
            return CheckQ(queue.LenFM - 1, List<T>.Tail(queue.F), queue.M, queue.LenR, queue.R);
        }
    }
}