// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BankersQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;

    using FunProgLib.streams;

    public static class BankersQueue<T>
    {
        public sealed class Queue
        {
            private readonly int lenf;
            private readonly Lazy<Stream<T>.StreamCell> f;
            private readonly int lenr;
            private readonly Lazy<Stream<T>.StreamCell> r;

            public Queue(int lenf, Lazy<Stream<T>.StreamCell> f, int lenr, Lazy<Stream<T>.StreamCell> r)
            {
                this.lenf = lenf;
                this.f = f;
                this.lenr = lenr;
                this.r = r;
            }

            public int LenF { get { return lenf; } }
            public Lazy<Stream<T>.StreamCell> F { get { return f; } }
            public int LenR { get { return lenr; } }
            public Lazy<Stream<T>.StreamCell> R { get { return r; } }
        }

        private readonly static Lazy<Stream<T>.StreamCell> EmptyCell = new Lazy<Stream<T>.StreamCell>(() => null);

        private static readonly Queue EmptyQueue = new Queue(0, EmptyCell, 0, EmptyCell);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue == null || IsEmpty(queue.F);
        }

        private static bool IsEmpty(Lazy<Stream<T>.StreamCell> queue)
        {
            return queue == null || queue.Value == null;
        }

        private static Queue Check(int lenf, Lazy<Stream<T>.StreamCell> f, int lenr, Lazy<Stream<T>.StreamCell> r)
        {
            if (lenr <= lenf) return new Queue(lenf, f, lenr, r);
            return new Queue(lenf + lenr, Stream<T>.Append(f, Stream<T>.Reverse(r)), 0, EmptyCell);
        }

        public static Queue Snoc(Queue queue, T element)
        {
            return Check(queue.LenF, queue.F, queue.LenR + 1, Stream<T>.Cons(element, queue.R));
        }

        public static T Head(Queue queue)
        {
            if (IsEmpty(queue.F)) throw new Exception("Empty");
            return queue.F.Value.Element;
        }

        public static Queue Tail(Queue queue)
        {
            if (IsEmpty(queue.F)) throw new Exception("Empty");
            return Check(queue.LenF - 1, queue.F.Value.Next, queue.LenR, queue.R);
        }
    }
}