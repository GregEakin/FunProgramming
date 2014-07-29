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
        // type Heap

        // Heap Empty { get; }
        // bool IsEmpty(Heap heap)

        // Heap Insert(Heap heap, T value)
        // Heap Merge(Heap a, Heap b)

        // T FindMin(Heap heap)
        // Heap DeleteMin(Heap heap)
    }
}