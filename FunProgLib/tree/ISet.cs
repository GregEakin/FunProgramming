// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgLib.tree
{
    public interface ISet<T>
    {
        ISet<T> Empty { get; }

        ISet<T> Insert(T elem, ISet<T> set);

        bool Member(T elem, ISet<T> set);
    }
}