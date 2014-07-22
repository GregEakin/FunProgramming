// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		Ordered.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    public interface IOrdered<T>
    {
        bool LessThan(IOrdered<T> that);

        bool Equal(IOrdered<T> that);

        bool LessThanEqual(IOrdered<T> that);
    }
}