// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BatchedQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class BatchedQueue<T>
    {
        public class Queue
        {
            private readonly ReadOnlyCollection<T> f;
            private readonly ReadOnlyCollection<T> r;

            public Queue(ReadOnlyCollection<T> f, ReadOnlyCollection<T> r)
            {
                this.f = f;
                this.r = r;
            }

            public ReadOnlyCollection<T> F { get { return f; } }
            public ReadOnlyCollection<T> R { get { return r; } }
        }

        private static readonly ReadOnlyCollection<T> EmptyList = new ReadOnlyCollection<T>(new T[0]);

        private static readonly Queue EmptyQueue = new Queue(EmptyList, EmptyList);

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            return q.F.Count == 0;
        }

        private static Queue CheckF(ReadOnlyCollection<T> f, ReadOnlyCollection<T> r)
        {
            if (f.Count == 0) return new Queue(r.Reverse().ToList().AsReadOnly(), EmptyList);
            return new Queue(f, r);
        }

        private static ReadOnlyCollection<T> Concatenate(T element, IEnumerable<T> list)
        {
            var x = list.ToList();
            x.Insert(0, element);
            return x.AsReadOnly();
        }

        public static Queue Snoc(Queue q, T x)
        {
            return CheckF(q.F, Concatenate(x, q.R));
        }

        public static T Head(Queue q)
        {
            if (q.F.Count == 0) throw new Exception("Empty");
            return q.F[0];
        }

        public static Queue Tail(Queue q)
        {
            if (q.F.Count == 0) throw new Exception("Empty");
            return CheckF(q.F.Skip(1).ToList().AsReadOnly(), q.R);
        }
    }
}