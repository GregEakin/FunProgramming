// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.2.1 Example: Hood-Melville Real-Time Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 102-105. Print.

namespace FunProgLib.queue
{
    public static class HoodMelvilleQueue<T>
    {
        public sealed class Queue
        {
        }

        private static readonly Queue EmptyQueue = null;

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue == null;
        }

        public static Queue Snoc(Queue queue, string item)
        {
            throw new System.NotImplementedException();
        }

        public static T Head(Queue queue)
        {
            throw new System.NotImplementedException();
        }

        public static Queue Tail(Queue queue)
        {
            throw new System.NotImplementedException();
        }
    }
}