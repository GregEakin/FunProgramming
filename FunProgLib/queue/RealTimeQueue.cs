// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		RealTimeQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;

    using FunProgLib.persistence;
    using FunProgLib.streams;

    public static class RealTimeQueue<T>
    {
        public class Queue
        {
            private readonly Lazy<Stream<T>.StreamCell> f;
            private readonly LinkList<T>.List r;
            private readonly Lazy<Stream<T>.StreamCell> s;

            public Queue(Lazy<Stream<T>.StreamCell> f, LinkList<T>.List r, Lazy<Stream<T>.StreamCell> s)
            {
                this.f = f;
                this.r = r;
                this.s = s;
            }

            public Lazy<Stream<T>.StreamCell> F { get { return f; } }
            public LinkList<T>.List R { get { return r; } }
            public Lazy<Stream<T>.StreamCell> S { get { return s; } }
        }

        private static readonly Lazy<Stream<T>.StreamCell> EmptyCell = new Lazy<Stream<T>.StreamCell>(() => null);
        private static readonly LinkList<T>.List EmptyList = null;
        private static readonly Queue EmptyQueue = new Queue(EmptyCell, EmptyList, EmptyCell);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue == null || queue.F == null || queue.F.Value == null;
        }

        private static Lazy<Stream<T>.StreamCell> Rotate(Lazy<Stream<T>.StreamCell> f, LinkList<T>.List r, Lazy<Stream<T>.StreamCell> s)
        {
            if (f == null || f.Value == null) return Stream<T>.Cons(r.Element, s);
            return Stream<T>.Cons(f.Value.Element, Rotate(f.Value.Next, r.Next, Stream<T>.Cons(r.Element, s)));
        }

        private static Queue Exec(Lazy<Stream<T>.StreamCell> f, LinkList<T>.List r, Lazy<Stream<T>.StreamCell> s)
        {
            if (s != null && s.Value != null) return new Queue(f, r, s.Value.Next);
            var fp = Rotate(f, r, EmptyCell);
            return new Queue(fp, EmptyList, fp);
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Exec(q.F, LinkList<T>.Cons(x, q.R), q.S);
        }

        public static T Head(Queue q)
        {
            if (IsEmpty(q)) throw new Exception("Empty");
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (IsEmpty(q)) throw new Exception("Empty");
            return Exec(q.F.Value.Next, q.R, q.S);
        }
    }
}