// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IFiniteMap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    public interface IFiniteMap<TKey, T>
    {
        IFiniteMap<TKey, T> Empty { get; }

        IFiniteMap<TKey, T> Bind(TKey key, T value, IFiniteMap<TKey, T> map);

        T Lookup(TKey key, IFiniteMap<TKey, T> map);  // throw NotFound if key isn't found
    }
}