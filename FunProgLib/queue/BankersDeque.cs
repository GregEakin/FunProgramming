// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.4.2 Banker's Deques." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 108-10. Print.

namespace FunProgLib.queue
{
    using System;

    using FunProgLib.streams;

    public static class BankersDeque<T> // : IDeque<T>
    {
        private const int C = 2; // C > 1

        public class Queue
        {
            private readonly int lenF;
            private readonly Lazy<Stream<T>.StreamCell> f;
            private readonly int lenR;
            private readonly Lazy<Stream<T>.StreamCell> r;

            public Queue(int lenF, Lazy<Stream<T>.StreamCell> f, int lenR, Lazy<Stream<T>.StreamCell> r)
            {
                this.lenF = lenF;
                this.f = f;
                this.lenR = lenR;
                this.r = r;
            }

            public int LenF { get { return this.lenF; } }
            public Lazy<Stream<T>.StreamCell> F { get { return this.f; } }
            public int LenR { get { return this.lenR; } }
            public Lazy<Stream<T>.StreamCell> R { get { return this.r; } }
        }

        private static readonly Queue EmptyQueue = new Queue(0, Stream<T>.DollarNil, 0, Stream<T>.DollarNil);

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            return q.LenF + q.LenR == 0;
        }

        private static Queue Check(int lenF, Lazy<Stream<T>.StreamCell> f, int lenR, Lazy<Stream<T>.StreamCell> r)
        {
            if (lenF > C * lenR + 1)
            {
                var i = (lenF + lenR) / 2;
                var j = lenF + lenR - i;
                var fp = Stream<T>.Take(i, f);
                var rp = Stream<T>.Append(r, Stream<T>.Reverse(Stream<T>.Drop(i, f)));
                return new Queue(i, fp, j, rp);
            }

            if (lenR > C * lenF + 1)
            {
                var j = (lenF + lenR) / 2;
                var i = lenF + lenR - j;
                var rp = Stream<T>.Take(j, r);
                var fp = Stream<T>.Append(f, Stream<T>.Reverse(Stream<T>.Drop(j, r)));
                return new Queue(i, fp, j, rp);
            }

            return new Queue(lenF, f, lenR, r);
        }

        public static Queue Cons(T x, Queue q)
        {
            var lazy = Stream<T>.DollarCons(x, q.F);
            return Check(q.LenF + 1, lazy, q.LenR, q.R);
        }

        public static T Head(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new Exception("Empty");
            if (q.F == Stream<T>.DollarNil) return q.R.Value.X;
            return q.F.Value.X;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new Exception("Empty");
            if (q.F == Stream<T>.DollarNil) return EmptyQueue;
            return Check(q.LenF - 1, q.F.Value.S, q.LenR, q.R);
        }

        public static Queue Snoc(Queue q, T x)
        {
            var lazy = Stream<T>.DollarCons(x, q.R);
            return Check(q.LenF, q.F, q.LenR + 1, lazy);
        }

        public static T Last(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new Exception("Empty");
            if (q.R == Stream<T>.DollarNil) return q.F.Value.X;
            return q.R.Value.X;
        }

        public static Queue Init(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new Exception("Empty");
            if (q.R == Stream<T>.DollarNil) return EmptyQueue;
            var rp = q.R.Value.S;
            return Check(q.LenF, q.F, q.LenR - 1, rp);
        }
    }
}