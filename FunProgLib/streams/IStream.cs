// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunProgramming
// FILE:		IStream.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.streams
{
    public interface IStream<T>
    {
        IStream<T> Append(IStream<T> left, IStream<T> right);  // ⧺

        IStream<T> Take(int n, IStream<T> stream);

        IStream<T> Drop(int n, IStream<T> stream);

        IStream<T> Reverse(IStream<T> stream);
    }
}