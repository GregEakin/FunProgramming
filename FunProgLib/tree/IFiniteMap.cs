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
    public interface IFiniteMap<TKey, T>
    {
        IFiniteMap<TKey, T> Empty { get; }

        IFiniteMap<TKey, T> Bind(TKey key, T value, IFiniteMap<TKey, T> map);

        T Lookup(TKey key, IFiniteMap<TKey, T> map);  // throw NotFound if key isn't found
    }
}