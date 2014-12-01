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

namespace FunProgLib.queue
{
    using System;

    using FunProgLib.lists;
    using FunProgLib.streams;

    public static class RealTimeQueue<T>
    {
        public sealed class Queue
        {
            private readonly Lazy<Stream<T>.StreamCell> f;
            private readonly List<T>.Node r;
            private readonly Lazy<Stream<T>.StreamCell> s;

            public Queue(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> s)
            {
                this.f = f;
                this.r = r;
                this.s = s;
            }

            public Lazy<Stream<T>.StreamCell> F { get { return f; } }
            public List<T>.Node R { get { return r; } }
            public Lazy<Stream<T>.StreamCell> S { get { return s; } }
        }

        private static readonly List<T>.Node EmptyList = null;
        private static readonly Queue EmptyQueue = new Queue(Stream<T>.DollarNil, EmptyList, Stream<T>.DollarNil);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue.F == Stream<T>.DollarNil;
        }

        private static Lazy<Stream<T>.StreamCell> Rotate(Lazy<Stream<T>.StreamCell> fp, List<T>.Node r, Lazy<Stream<T>.StreamCell> s)
        {
            if (fp == Stream<T>.DollarNil) return Stream<T>.DollarCons(r.Element, s);
            return Stream<T>.DollarCons(fp.Value.Element, Rotate(fp.Value.Next, r.Next, Stream<T>.DollarCons(r.Element, s)));
        }

        private static Queue Exec(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> sp)
        {
            if (sp.Value != null) return new Queue(f, r, sp.Value.Next);
            var fp = Rotate(f, r, Stream<T>.DollarNil);
            return new Queue(fp, EmptyList, fp);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Exec(q.F, List<T>.Cons(x, q.R), q.S);
        }

        public static T Head(Queue q)
        {
            if (q.F == Stream<T>.DollarNil) throw new Exception("Empty");
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == Stream<T>.DollarNil) throw new Exception("Empty");
            return Exec(q.F.Value.Next, q.R, q.S);
        }
    }
}