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

        private static readonly Lazy<Stream<T>.StreamCell> EmptyCell = new Lazy<Stream<T>.StreamCell>(() => null);
        private static readonly List<T>.Node EmptyList = null;
        private static readonly Queue EmptyQueue = new Queue(EmptyCell, EmptyList, EmptyCell);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue.F.Value == null;
        }

        private static Lazy<Stream<T>.StreamCell> Rotate(Lazy<Stream<T>.StreamCell> fp, List<T>.Node r, Lazy<Stream<T>.StreamCell> s)
        {
            if (fp.Value == null) return Stream<T>.DollarCons(r.Element, s);
            return Stream<T>.DollarCons(fp.Value.X, Rotate(fp.Value.S, r.Next, Stream<T>.DollarCons(r.Element, s)));
        }

        private static Queue Exec(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> sp)
        {
            if (sp.Value != null) return new Queue(f, r, sp.Value.S);
            var fp = Rotate(f, r, EmptyCell);
            return new Queue(fp, EmptyList, fp);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Exec(q.F, List<T>.Cons(x, q.R), q.S);
        }

        public static T Head(Queue q)
        {
            if (q.F.Value == null) throw new Exception("Empty");
            return q.F.Value.X;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F.Value == null) throw new Exception("Empty");
            return Exec(q.F.Value.S, q.R, q.S);
        }
    }
}