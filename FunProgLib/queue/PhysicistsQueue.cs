// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.2 Example: Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 72-74. Print.

namespace FunProgLib.queue
{
    using System;
    using lists;

    public static class PhysicistsQueue<T>
    {
        public sealed class Queue
        {
            public Queue(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
            {
                W = w;
                Lenf = lenf;
                F = f;
                Lenr = lenr;
                R = r;
            }

            public List<T>.Node W { get; }
            public int Lenf { get; }
            public Lazy<List<T>.Node> F { get; }
            public int Lenr { get; }
            public List<T>.Node R { get; }
        }

        public static Queue Empty { get; } = new Queue(List<T>.Empty, 0, new Lazy<List<T>.Node>(() => List<T>.Empty), 0, List<T>.Empty);

        public static bool IsEmpty(Queue queue) => queue.Lenf == 0;

        private static Queue CheckW(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
        {
            if (List<T>.IsEmpty(w)) return new Queue(f.Value, lenf, f, lenr, r);
            return new Queue(w, lenf, f, lenr, r);
        }

        private static Queue Check(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
        {
            if (lenr <= lenf) return CheckW(w, lenf, f, lenr, r);
            return CheckW(f.Value, lenf + lenr, new Lazy<List<T>.Node>(() => List<T>.Cat(f.Value, List<T>.Reverse(r))), 0, List<T>.Empty);
        }

        public static Queue Snoc(Queue queue, T element) => 
            Check(queue.W, queue.Lenf, queue.F, queue.Lenr + 1, List<T>.Cons(element, queue.R));

        public static T Head(Queue queue)
        {
            if (List<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
            return queue.W.Element;
        }

        public static Queue Tail(Queue queue)
        {
            if (List<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
            return Check(queue.W.Next, queue.Lenf - 1, new Lazy<List<T>.Node>(() => queue.F.Value.Next), queue.Lenr, queue.R);
        }
    }
}