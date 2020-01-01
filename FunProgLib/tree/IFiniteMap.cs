// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.3 Binaryt Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

namespace FunProgLib.tree
{
    public interface IFiniteMap<TKey, T>
    {
        IFiniteMap<TKey, T> Empty { get; }

        IFiniteMap<TKey, T> Bind(TKey key, T value, IFiniteMap<TKey, T> map);

        T Lookup(TKey key, IFiniteMap<TKey, T> map);  // throw NotFound if key isn't found
    }
}