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

    using FunProgLib.lists;

    public static class PhysicistsQueue<T>
    {
        public sealed class Queue
        {
            private readonly List<T>.Node w;
            private readonly int lenf;
            private readonly Lazy<List<T>.Node> f;
            private readonly int lenr;
            private readonly List<T>.Node r;

            public Queue(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
            {
                this.w = w;
                this.lenf = lenf;
                this.f = f;
                this.lenr = lenr;
                this.r = r;
            }

            public List<T>.Node W { get { return w; } }
            public int Lenf { get { return lenf; } }
            public Lazy<List<T>.Node> F { get { return f; } }
            public int Lenr { get { return lenr; } }
            public List<T>.Node R { get { return r; } }
        }

        private static readonly List<T>.Node EmptyList = null;

        private static readonly Queue EmptyQueue = new Queue(EmptyList, 0, new Lazy<List<T>.Node>(() => EmptyList), 0, EmptyList);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue.Lenf == 0;
        }

        private static Queue CheckW(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
        {
            if (w == EmptyList) return new Queue(f.Value, lenf, f, lenr, r);
            return new Queue(w, lenf, f, lenr, r);
        }

        private static Queue Check(List<T>.Node w, int lenf, Lazy<List<T>.Node> f, int lenr, List<T>.Node r)
        {
            if (lenr <= lenf) return CheckW(w, lenf, f, lenr, r);
            return CheckW(f.Value, lenf + lenr, new Lazy<List<T>.Node>(() => List<T>.Cat(f.Value, List<T>.Reverse(r))), 0, EmptyList);
        }

        public static Queue Snoc(Queue queue, T element)
        {
            return Check(queue.W, queue.Lenf, queue.F, queue.Lenr + 1, List<T>.Cons(element, queue.R));
        }

        public static T Head(Queue queue)
        {
            if (queue.W == EmptyList) throw new Exception("Empty");
            return queue.W.Element;
        }

        public static Queue Tail(Queue queue)
        {
            if (queue.W == EmptyList) throw new Exception("Empty");
            return Check(queue.W.Next, queue.Lenf - 1, new Lazy<List<T>.Node>(() => queue.F.Value.Next), queue.Lenr, queue.R);
        }
    }
}