// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ISet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    public interface ISet<T>
    {
        ISet<T> Empty { get; }

        ISet<T> Insert(T elem, ISet<T> set);

        bool Member(T elem, ISet<T> set);
    }
}