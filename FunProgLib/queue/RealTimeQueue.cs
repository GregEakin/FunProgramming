// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "7.2 Real-Time Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 86-89. Print.

using System;
using FunProgLib.lists;
using FunProgLib.streams;

namespace FunProgLib.queue
{
    public static class RealTimeQueue<T>
    {
        public sealed class Queue
        {
            public Queue(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> s)
            {
                F = f;
                R = r;
                S = s;
            }

            public Lazy<Stream<T>.StreamCell> F { get; }
            public List<T>.Node R { get; }
            public Lazy<Stream<T>.StreamCell> S { get; }
        }

        public static Queue Empty { get; } = new Queue(Stream<T>.DollarNil, List<T>.Empty, Stream<T>.DollarNil);

        public static bool IsEmpty(Queue queue) => queue.F == Stream<T>.DollarNil;

        private static Lazy<Stream<T>.StreamCell> Rotate(Lazy<Stream<T>.StreamCell> xp, List<T>.Node yp, Lazy<Stream<T>.StreamCell> a)
        {
            if (xp == Stream<T>.DollarNil) return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(yp.Element, a));
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(xp.Value.Element, Rotate(xp.Value.Next, yp.Next, new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(yp.Element, a)))));
        }

        private static Queue Exec(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> sp)
        {
            if (sp.Value != null) return new Queue(f, r, sp.Value.Next);
            var fp = Rotate(f, r, Stream<T>.DollarNil);
            return new Queue(fp, List<T>.Empty, fp);
        }

        public static Queue Snoc(Queue q, T x) => Exec(q.F, List<T>.Cons(x, q.R), q.S);

        public static T Head(Queue q)
        {
            if (q.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            return Exec(q.F.Value.Next, q.R, q.S);
        }
    }
}