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
            var f = fp.Value as Stream<T>.Cons;
            if (f == null) return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(r.Element, s));
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(f.X, Rotate(f.S, r.Next, new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(r.Element, s)))));
        }

        private static Queue Exec(Lazy<Stream<T>.StreamCell> f, List<T>.Node r, Lazy<Stream<T>.StreamCell> sp)
        {
            var s = sp.Value as Stream<T>.Cons;
            if (s != null) return new Queue(f, r, s.S);
            var fp = Rotate(f, r, EmptyCell);
            return new Queue(fp, EmptyList, fp);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Exec(q.F, List<T>.Cons(x, q.R), q.S);
        }

        public static T Head(Queue q)
        {
            var qf = q.F.Value as Stream<T>.Cons;
            if (qf == null) throw new Exception("Empty");
            return qf.X;
        }

        public static Queue Tail(Queue q)
        {
            var qf = q.F.Value as Stream<T>.Cons;
            if (qf == null) throw new Exception("Empty");
            return Exec(qf.S, q.R, q.S);
        }
    }
}