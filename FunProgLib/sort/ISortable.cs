// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunProgramming
// FILE:		ISortable.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.sort
{
    using System;

    using FunProgLib.lists;

    public interface ISortable<T> where T : IComparable
    {
        ISortable<T> Empty { get; }

        ISortable<T> Add(T element, ISortable<T> sortable);

        List<T>.Node Sort(ISortable<T> sortable);
    }
}