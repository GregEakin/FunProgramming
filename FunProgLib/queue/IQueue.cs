// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

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