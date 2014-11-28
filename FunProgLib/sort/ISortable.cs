// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

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