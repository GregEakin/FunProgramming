// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		IStack.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.lists
{
    public interface IStack<T>
    {
        IStack<T> Empty { get; }

        bool IsEmpty(IStack<T> stack);

        IStack<T> Cons(IStack<T> stack, T element);

        T Head(IStack<T> stack);

        IStack<T> Tail(IStack<T> stack);
    }
}