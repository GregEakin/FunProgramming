// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IOrdered.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    public interface IOrdered<in T>
    {
        bool Equal(T left, T right);

        bool LessThan(T left, T right);

        bool LessThanEqual(T left, T right);
    }
}