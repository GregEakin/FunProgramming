// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		RealTimeQueue.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    using System;

    public static class RealTimeQueue<T>
    {
        public class Queue
        {
        }

        private static readonly Queue EmptyQueue = null;

        public static Queue Empty { get { return EmptyQueue; } }

        public static bool IsEmpty(Queue q)
        {
            throw new NotImplementedException();
        }

        public static Queue Snoc(Queue q, T x)
        {
            throw new NotImplementedException();
        }

        public static T Head(Queue q)
        {
            throw new NotImplementedException();
        }

        public static Queue Tail(Queue q)
        {
            throw new NotImplementedException();
        }
    }
}