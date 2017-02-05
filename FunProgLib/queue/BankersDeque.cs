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
    using streams;
    using System;

    public static class BankersDeque<T> // : IDeque<T>
    {
        private const int C = 2; // C > 1

        public class Queue
        {
            public Queue(int lenF, Lazy<Stream<T>.StreamCell> f, int lenR, Lazy<Stream<T>.StreamCell> r)
            {
                LenF = lenF;
                F = f;
                LenR = lenR;
                R = r;
            }

            public int LenF { get; }
            public Lazy<Stream<T>.StreamCell> F { get; }
            public int LenR { get; }
            public Lazy<Stream<T>.StreamCell> R { get; }
        }

        public static Queue Empty { get; } = new Queue(0, Stream<T>.DollarNil, 0, Stream<T>.DollarNil);

        public static bool IsEmpty(Queue q) => q.LenF + q.LenR == 0;

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
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(x, q.F));
            return Check(q.LenF + 1, lazy, q.LenR, q.R);
        }

        public static T Head(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            if (q.F == Stream<T>.DollarNil) return q.R.Value.Element;
            return q.F.Value.Element;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F == Stream<T>.DollarNil && q.R == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            if (q.F == Stream<T>.DollarNil) return Empty;
            return Check(q.LenF - 1, q.F.Value.Next, q.LenR, q.R);
        }

        public static Queue Snoc(Queue q, T x)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(x, q.R));
            return Check(q.LenF, q.F, q.LenR + 1, lazy);
        }

        public static T Last(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            if (q.R == Stream<T>.DollarNil) return q.F.Value.Element;
            return q.R.Value.Element;
        }

        public static Queue Init(Queue q)
        {
            if (q.R == Stream<T>.DollarNil && q.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(q));
            if (q.R == Stream<T>.DollarNil) return Empty;
            var rp = q.R.Value.Next;
            return Check(q.LenF, q.F, q.LenR - 1, rp);
        }
    }
}
