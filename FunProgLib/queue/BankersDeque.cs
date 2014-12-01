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

        private static readonly Queue EmptyQueue = new Queue(0, null, 0, null);

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
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(x, q.F));
            return Check(q.LenF + 1, lazy, q.LenR, q.R);
        }

        public static T Head(Queue q)
        {
            var qf = q.F.Value as Stream<T>.Cons;
            var qr = q.R.Value as Stream<T>.Cons;
            if (qf == null && qr == null) throw new Exception("Empty");
            if (qf == null) return qr.X;
            return qf.X;
        }

        public static Queue Tail(Queue q)
        {
            var qf = q.F.Value as Stream<T>.Cons;
            var qr = q.R.Value as Stream<T>.Cons;
            if (qf == null && qr == null) throw new Exception("Empty");
            if (qf == null) return EmptyQueue;
            return Check(q.LenF - 1, qf.S, q.LenR, q.R);
        }

        public static Queue Snoc(Queue q, T x)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(x, q.R));
            return Check(q.LenF, q.F, q.LenR + 1, lazy);
        }

        public static T Last(Queue q)
        {
            var qr = q.R.Value as Stream<T>.Cons;
            var qf = q.F.Value as Stream<T>.Cons;
            if (qr == null && qf == null) throw new Exception("Empty");
            if (qr == null) return qf.X;
            return qr.X;
        }

        public static Queue Init(Queue q)
        {
            var qr = q.R.Value as Stream<T>.Cons;
            var qf = q.F.Value as Stream<T>.Cons;
            if (qr == null && qf == null) throw new Exception("Empty");
            if (qr == null) return EmptyQueue;
            var rp = qr.S;
            return Check(q.LenF, q.F, q.LenR - 1, rp);
        }
    }
}