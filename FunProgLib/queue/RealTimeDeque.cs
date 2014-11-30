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

    public static class RealTimeDeque<T> // : IDeque<T>
    {
        public class Deque
        {
        }

        public static Deque Empty { get { return null; } }

        public static bool IsEmpty(Deque q)
        {
            throw new NotImplementedException();
        }

        public static Deque Cons(T x, Deque q)
        {
            throw new NotImplementedException();
        }

        public static T Head(Deque q)
        {
            throw new NotImplementedException();
        }

        public static Deque Tail(Deque q)
        {
            throw new NotImplementedException();
        }

        public static Deque Snoc(Deque q, T x)
        {
            throw new NotImplementedException();
        }

        public static T Last(Deque q)
        {
            throw new NotImplementedException();
        }

        public static Deque Init(Deque q)
        {
            throw new NotImplementedException();
        }
    }
}