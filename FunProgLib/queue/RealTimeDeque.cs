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

        private static readonly Queue EmptyQueue = new Queue(0, null, null, 0, null, null);

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
            if (s != null) return s.Value.Next;
            return null;
        }

        private static Lazy<Stream<T>.StreamCell> Exec2(Lazy<Stream<T>.StreamCell> s)
        {
            return Exec1(Exec1(s));
        }

        private static Lazy<Stream<T>.StreamCell> RotateRev(Lazy<Stream<T>.StreamCell> fp, Lazy<Stream<T>.StreamCell> r, Lazy<Stream<T>.StreamCell> a)
        {
            if (fp == null) return Stream<T>.Append(Stream<T>.Reverse(r), a);
            var x = fp.Value.Element;
            var f = fp.Value.Next;
            return Stream<T>.Cons(x, RotateRev(f, Stream<T>.Drop(C, r), Stream<T>.Append(Stream<T>.Reverse(Stream<T>.Take(C, r)), a)));
        }

        private static Lazy<Stream<T>.StreamCell> RotateDrop(Lazy<Stream<T>.StreamCell> f, int j, Lazy<Stream<T>.StreamCell> r)
        {
            if (j < C) return RotateRev(f, Stream<T>.Drop(j, r), null);
            var x = f.Value.Element;
            var fp = f.Value.Next;
            return Stream<T>.Cons(x, RotateDrop(fp, j - C, Stream<T>.Drop(C, r)));
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
            return Check(q.LenF + 1, Stream<T>.Cons(x, q.F), Exec1(q.Sf), q.LenR, q.R, Exec1(q.Sr));
        }

        public static T Head(Queue q)
        {
            if (q.F == null && q.R == null) throw new Exception("Empty");
            if (q.F == null) return q.R.Value.Element;
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == null && q.R == null) throw new Exception("Empty");
            if (q.F == null) return EmptyQueue;
            var fp = q.F.Value.Next;
            return Check(q.LenF - 1, fp, Exec2(q.Sf), q.LenR, q.R, Exec2(q.Sr));
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Check(q.LenF, q.F, Exec1(q.Sf), q.LenR + 1, Stream<T>.Cons(x, q.R), Exec1(q.Sr));
        }

        public static T Last(Queue q)
        {
            if (q.R == null && q.F == null) throw new Exception("Empty");
            if (q.R == null) return q.F.Value.Element;
            return q.R.Value.Element;
        }

        public static Queue Init(Queue q)
        {
            if (q.R == null && q.F == null) throw new Exception("Empty");
            if (q.R == null) return EmptyQueue;
            var rp = q.R.Value.Next;
            return Check(q.LenF, q.F, Exec2(q.Sf), q.LenR - 1, rp, Exec2(q.Sr));
        }
    }
}