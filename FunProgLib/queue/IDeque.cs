// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.4 Double-Ended Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 106-107. Print.

namespace FunProgLib.queue
{
    public interface IDeque<T>
    {
        IDeque<T> Empty { get; }

        bool IsEmpty(IDeque<T> queue);

        IDeque<T> Cons(T element, IDeque<T> queue);

        T Head(IDeque<T> queue);

        IDeque<T> Tail(IQueue<T> queue);

        IDeque<T> Snoc(IDeque<T> queue, T element);

        T Last(IDeque<T> queue);

        IDeque<T> Init(IQueue<T> queue);
    }
}