// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.3.2 Example: Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 64-67. Print.

namespace FunProgLib.queue
{
    using streams;
    using System;

    public static class BankersQueue<T>
    {
        public sealed class Queue
        {
            public Queue(int lenf, Lazy<Stream<T>.StreamCell> f, int lenr, Lazy<Stream<T>.StreamCell> r)
            {
                LenF = lenf;
                F = f;
                LenR = lenr;
                R = r;
            }

            public int LenF { get; }
            public Lazy<Stream<T>.StreamCell> F { get; }
            public int LenR { get; }
            public Lazy<Stream<T>.StreamCell> R { get; }
        }

        public static Queue Empty { get; } = new Queue(0, Stream<T>.DollarNil, 0, Stream<T>.DollarNil);

        public static bool IsEmpty(Queue queue) => queue.LenF == 0;

        private static Queue Check(int lenf, Lazy<Stream<T>.StreamCell> f, int lenr, Lazy<Stream<T>.StreamCell> r)
        {
            if (lenr <= lenf) return new Queue(lenf, f, lenr, r);
            return new Queue(lenf + lenr, Stream<T>.Append(f, Stream<T>.Reverse(r)), 0, Stream<T>.DollarNil);
        }

        public static Queue Snoc(Queue queue, T element)
        {
            var lazy = new Lazy<Stream<T>.StreamCell>(() => new Stream<T>.StreamCell(element, queue.R));
            return Check(queue.LenF, queue.F, queue.LenR + 1, lazy);
        }

        public static T Head(Queue queue)
        {
            if (queue.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(queue));
            return queue.F.Value.Element;
        }

        public static Queue Tail(Queue queue)
        {
            if (queue.F == Stream<T>.DollarNil) throw new ArgumentNullException(nameof(queue));
            return Check(queue.LenF - 1, queue.F.Value.Next, queue.LenR, queue.R);
        }
    }
}
