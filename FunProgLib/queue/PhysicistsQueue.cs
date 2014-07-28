// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		PhysicistsQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;

    using FunProgLib.persistence;

    public static class PhysicistsQueue<T>
    {
        public class Queue
        {
            private readonly LinkList<T>.ListStructure w;
            private readonly int lenf;
            private readonly Lazy<LinkList<T>.ListStructure> f;
            private readonly int lenr;
            private readonly LinkList<T>.ListStructure r;

            public Queue(LinkList<T>.ListStructure w, int lenf, Lazy<LinkList<T>.ListStructure> f, int lenr, LinkList<T>.ListStructure r)
            {
                this.w = w;
                this.lenf = lenf;
                this.f = f;
                this.lenr = lenr;
                this.r = r;
            }

            public LinkList<T>.ListStructure W { get { return this.w; } }
            public int Lenf { get { return this.lenf; } }
            public Lazy<LinkList<T>.ListStructure> F { get { return this.f; } }
            public int Lenr { get { return this.lenr; } }
            public LinkList<T>.ListStructure R { get { return this.r; } }
        }

        private static readonly LinkList<T>.ListStructure EmptyList = null;

        private static readonly Queue EmptyQueue = new Queue(EmptyList, 0, new Lazy<LinkList<T>.ListStructure>(() => EmptyList), 0, EmptyList);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue.Lenf == 0;
        }

        private static Queue CheckW(LinkList<T>.ListStructure w, int lenf, Lazy<LinkList<T>.ListStructure> f, int lenr, LinkList<T>.ListStructure r)
        {
            if (w == EmptyList) return new Queue(f.Value, lenf, f, lenr, r);
            return new Queue(w, lenf, f, lenr, r);
        }

        private static Queue Check(LinkList<T>.ListStructure w, int lenf, Lazy<LinkList<T>.ListStructure> f, int lenr, LinkList<T>.ListStructure r)
        {
            if (lenr <= lenf) return CheckW(w, lenf, f, lenr, r);
            return CheckW(f.Value, lenf + lenr, new Lazy<LinkList<T>.ListStructure>(() => LinkList<T>.Cat(f.Value, LinkList<T>.Reverse(r))), 0, EmptyList);
        }

        public static Queue Snoc(Queue queue, T element)
        {
            return Check(queue.W, queue.Lenf, queue.F, queue.Lenr + 1, LinkList<T>.Cons(queue.R, element));
        }

        public static T Head(Queue queue)
        {
            if (queue.W == EmptyList) throw new Exception("Empty");
            return queue.W.Element;
        }

        public static Queue Tail(Queue queue)
        {
            if (queue.W == EmptyList) throw new Exception("Empty");
            return Check(queue.W.Next, queue.Lenf - 1, new Lazy<LinkList<T>.ListStructure>(() => queue.F.Value.Next), queue.Lenr, queue.R);
        }
    }
}