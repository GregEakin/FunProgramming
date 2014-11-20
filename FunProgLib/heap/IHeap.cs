// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.heap
{
    using System;

    public interface IHeap<T> where T : IComparable
    {
        IHeap<T> Empty { get; }

        bool IsEmpty(IHeap<T> heap);

        IHeap<T> Insert(T value, IHeap<T> heap);

        IHeap<T> Merge(IHeap<T> a, IHeap<T> b);

        T FindMin(IHeap<T> heap);

        IHeap<T> DeleteMin(IHeap<T> heap);
    }
}