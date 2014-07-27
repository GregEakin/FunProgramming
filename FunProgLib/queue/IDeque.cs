// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		Deque.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.queue
{
    public interface IDeque<T>
    {
        IDeque<T> Empty { get; }

        bool IsEmpty(IDeque<T> queue);

        IDeque<T> Cons(IDeque<T> queue, T element);

        T Head(IDeque<T> queue);

        IDeque<T> Tail(IQueue<T> queue);

        IDeque<T> Snoc(IDeque<T> queue, T element);

        T Last(IDeque<T> queue);

        IDeque<T> Init(IQueue<T> queue);
    }
}