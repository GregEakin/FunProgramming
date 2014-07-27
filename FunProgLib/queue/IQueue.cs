// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		Queue.cs
// AUTHOR:		Greg Eakin
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