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

    using streams;

    public static class RealTimeDeque<T> // : IDeque<T>
    {
        private const int C = 2; // C == 2 || C == 3

        public class Queue
        {
            public Queue(
                int lenF,
                Lazy<Stream<T>.StreamCell> f,
                Lazy<Stream<T>.StreamCell> sf,
                int lenR,
                Lazy<Stream<T>.StreamCell> r,
                Lazy<Stream<T>.StreamCell> sr)
            {
                LenF = lenF;
                F = f;
                Sf = sf;
                LenR = lenR;
                Sr = sr;
                R = r;
            }

            public int LenF { get; }
            public Lazy<Stream<T>.StreamCell> F { get; }
            public Lazy<Stream<T>.StreamCell> Sf { get; }
            public int LenR { get; }
            public Lazy<Stream<T>.StreamCell> R { get; }
            public Lazy<Stream<T>.StreamCell> Sr { get; }
        }

        public static Queue Empty { get; } = new Queue(0, Stream<T>.DollarNil, Stream<T>.DollarNil, 0, Stream<T>.DollarNil, Stream<T>.DollarNil);

        public static bool IsEmpty(Queue q)
        {
            return q.LenF + q.LenR == 0;
        }

        private static Lazy<Stream<T>.StreamCell> Exec1(Lazy<Stream<T>.StreamCell> s)
        {
            if (s != Stream<T>.DollarNil) return s.Value.Next;
            return s;
        }

        private static Lazy<Stream<T>.StreamCell> Exec2(Lazy<Stream<T>.StreamCell> s)
        {
            return Exec1(Exec1(s));
        }

        private static Lazy<Stream<T>.StreamCell> RotateRev(Lazy<Stream<T>.StreamCell> fc, Lazy<Stream<T>.StreamCell> r, Lazy<Stream<T>.StreamCell> a)
        {
            if (fc == Stream<T>.DollarNil) return Stream<T>.Append(Stream<T>.Reverse(r), a);
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(fc.Value.Element, RotateRev(fc.Value.Next, Stream<T>.Drop(C, r), Stream<T>.Append(Stream<T>.Reverse(Stream<T>.Take(C, r)), a))));
        }

        private static Lazy<Stream<T>.StreamCell> RotateDrop(Lazy<Stream<T>.StreamCell> fc, int j, Lazy<Stream<T>.StreamCell> r)
        {
            if (j < C) return RotateRev(fc, Stream<T>.Drop(j, r), Stream<T>.DollarNil);
            if (fc == Stream<T>.DollarNil) throw new Exception("Not supposed to happen.");
            return new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(fc.Value.Element, RotateDrop(fc.Value.Next, j - C, Stream<T>.Drop(C, r))));
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
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(x, q.F));
            return Check(q.LenF + 1, lazy, Exec1(q.Sf), q.LenR, q.R, Exec1(q.Sr));
        }

        public static T Head(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new ArgumentException("Empty", nameof(q));
            if (q.F == Stream<T>.DollarNil) return q.R.Value.Element;
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new ArgumentException("Empty", nameof(q));
            if (q.F == Stream<T>.DollarNil) return Empty;
            var fp = q.F.Value.Next;
            return Check(q.LenF - 1, fp, Exec2(q.Sf), q.LenR, q.R, Exec2(q.Sr));
        }

        public static Queue Snoc(Queue q, T x)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(x, q.R));
            return Check(q.LenF, q.F, Exec1(q.Sf), q.LenR + 1, lazy, Exec1(q.Sr));
        }

        public static T Last(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new ArgumentException("Empty", nameof(q));
            if (q.R == Stream<T>.DollarNil) return q.F.Value.Element;
            return q.R.Value.Element;
        }

        public static Queue Init(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new ArgumentException("Empty", nameof(q));
            if (q.R == Stream<T>.DollarNil) return Empty;
            var rp = q.R.Value.Next;
            return Check(q.LenF, q.F, Exec2(q.Sf), q.LenR - 1, rp, Exec2(q.Sr));
        }
    }
}