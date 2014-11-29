// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.1 Leftist Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 17-20. Print.

namespace FunProgLib.heap
{
    using System;

    public interface IHeap<T> where T : IComparable<T>
    {
        IHeap<T> Empty { get; }

        bool IsEmpty(IHeap<T> heap);

        IHeap<T> Insert(T value, IHeap<T> heap);

        IHeap<T> Merge(IHeap<T> a, IHeap<T> b);

        T FindMin(IHeap<T> heap);

        IHeap<T> DeleteMin(IHeap<T> heap);
    }
}