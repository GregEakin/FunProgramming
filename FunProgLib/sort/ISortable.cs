// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.3 Example: Bottom-Up Mergesort with Sharing." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 74-78. Print.

using System;
using FunProgLib.lists;

namespace FunProgLib.sort
{
    public interface ISortable<T> where T : IComparable<T>
    {
        ISortable<T> Empty { get; }

        ISortable<T> Add(T element, ISortable<T> sortable);

        List<T>.Node Sort(ISortable<T> sortable);
    }
}