// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "5.2 Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 42-45. Print.

namespace FunProgLib.queue
{
    public interface IQueue<T>
    {
        IQueue<T> Empty { get; }

        bool IsEmpty(IQueue<T> queue);

        IQueue<T> Snoc(IQueue<T> queue, T element);

        T Head(IQueue<T> queue);

        IQueue<T> Tail(IQueue<T> queue);
    }
}