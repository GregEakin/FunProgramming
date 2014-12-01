// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.4.3 Real-Time Deques." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 111-12. Print.

namespace FunProgLib.queue
{
    using System;

    using FunProgLib.streams;

    public static class RealTimeDeque<T> // : IDeque<T>
    {
        private const int C = 2; // C == 2 || C == 3

        public class Queue
        {
            private readonly int lenF;
            private readonly Lazy<Stream<T>.StreamCell> f;
            private readonly Lazy<Stream<T>.StreamCell> sf;
            private readonly int lenR;
            private readonly Lazy<Stream<T>.StreamCell> r;
            private readonly Lazy<Stream<T>.StreamCell> sr;

            public Queue(
                int lenF,
                Lazy<Stream<T>.StreamCell> f,
                Lazy<Stream<T>.StreamCell> sf,
                int lenR,
                Lazy<Stream<T>.StreamCell> r,
                Lazy<Stream<T>.StreamCell> sr)
            {
                this.lenF = lenF;
                this.f = f;
                this.sf = sf;
                this.lenR = lenR;
                this.sr = sr;
                this.r = r;
            }

            public int LenF { get { return this.lenF; } }
            public Lazy<Stream<T>.StreamCell> F { get { return this.f; } }
            public Lazy<Stream<T>.StreamCell> Sf { get { return this.sf; } }
            public int LenR { get { return this.lenR; } }
            public Lazy<Stream<T>.StreamCell> R { get { return this.r; } }
            public Lazy<Stream<T>.StreamCell> Sr { get { return this.sr; } }
        }

        private static readonly Queue EmptyQueue = new Queue(0, Stream<T>.Empty, Stream<T>.Empty, 0, Stream<T>.Empty, Stream<T>.Empty);

        public static Queue Empty
        {
            get
            {
                return EmptyQueue;
            }
        }

        public static bool IsEmpty(Queue q)
        {
            return q.LenF + q.LenR == 0;
        }

        private static Lazy<Stream<T>.StreamCell> Exec1(Lazy<Stream<T>.StreamCell> s)
        {
            var cons = s.Value as Stream<T>.Cons;
            if (cons != null) return cons.S;
            return s;
        }

        private static Lazy<Stream<T>.StreamCell> Exec2(Lazy<Stream<T>.StreamCell> s)
        {
            return Exec1(Exec1(s));
        }

        private static Lazy<Stream<T>.StreamCell> RotateRev(Lazy<Stream<T>.StreamCell> fc, Lazy<Stream<T>.StreamCell> r, Lazy<Stream<T>.StreamCell> a)
        {
            var fp = fc.Value as Stream<T>.Cons;
            if (fp == null) return Stream<T>.Append(Stream<T>.Reverse(r), a);
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(fp.X, RotateRev(fp.S, Stream<T>.Drop(C, r), Stream<T>.Append(Stream<T>.Reverse(Stream<T>.Take(C, r)), a))));
        }

        private static Lazy<Stream<T>.StreamCell> RotateDrop(Lazy<Stream<T>.StreamCell> fc, int j, Lazy<Stream<T>.StreamCell> r)
        {
            if (j < C) return RotateRev(fc, Stream<T>.Drop(j, r), Stream<T>.Empty);
            var fp = fc.Value as Stream<T>.Cons;
            if (fp == null) throw new Exception("Not supposed to happen.");
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(fp.X, RotateDrop(fp.S, j - C, Stream<T>.Drop(C, r))));
        }

        private static Queue Check(int lenF, Lazy<Stream<T>.StreamCell> f, Lazy<Stream<T>.StreamCell> sf, int lenR, Lazy<Stream<T>.StreamCell> r, Lazy<Stream<T>.StreamCell> sr)
        {
            if (lenF > C * lenR + 1)
            {
                var i = (lenF + lenR) / 2;
                var j = lenF + lenR - i;
                var fp = Stream<T>.Take(i, f);
                var rp = RotateDrop(r, i, f);
                return new Queue(i, fp, fp, j, rp, rp);
            }

            if (lenR > C * lenF + 1)
            {
                var j = (lenF + lenR) / 2;
                var i = lenF + lenR - j;
                var rp = Stream<T>.Take(j, r);
                var fp = RotateDrop(f, j, r);
                return new Queue(i, fp, fp, j, rp, rp);
            }

            return new Queue(lenF, f, sf, lenR, r, sr);
        }

        public static Queue Cons(T x, Queue q)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(x, q.F));
            return Check(q.LenF + 1, lazy, Exec1(q.Sf), q.LenR, q.R, Exec1(q.Sr));
        }

        public static T Head(Queue q)
        {
            var consF = q.F.Value as Stream<T>.Cons;
            var consR = q.R.Value as Stream<T>.Cons;
            if (consF == null && consR == null) throw new Exception("Empty");
            if (consF == null) return consR.X;
            return consF.X;
        }

        public static Queue Tail(Queue q)
        {
            var consF = q.F.Value as Stream<T>.Cons;
            var consR = q.R.Value as Stream<T>.Cons;
            if (consF == null && consR == null) throw new Exception("Empty");
            if (consF == null) return EmptyQueue;
            var fp = consF.S;
            return Check(q.LenF - 1, fp, Exec2(q.Sf), q.LenR, q.R, Exec2(q.Sr));
        }

        public static Queue Snoc(Queue q, T x)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.Cons(x, q.R));
            return Check(q.LenF, q.F, Exec1(q.Sf), q.LenR + 1, lazy, Exec1(q.Sr));
        }

        public static T Last(Queue q)
        {
            var consR = q.R.Value as Stream<T>.Cons;
            var consF = q.F.Value as Stream<T>.Cons;
            if (consR == null && consF == null) throw new Exception("Empty");
            if (consR == null) return consF.X;
            return consR.X;
        }

        public static Queue Init(Queue q)
        {
            var consR = q.R.Value as Stream<T>.Cons;
            var consF = q.F.Value as Stream<T>.Cons;
            if (consR == null && consF == null) throw new Exception("Empty");
            if (consR == null) return EmptyQueue;
            var rp = consR.S;
            return Check(q.LenF, q.F, Exec2(q.Sf), q.LenR - 1, rp, Exec2(q.Sr));
        }
    }
}