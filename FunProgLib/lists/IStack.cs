﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

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