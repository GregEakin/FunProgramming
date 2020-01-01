// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

namespace FunProgLib.lists
{
    public interface IStack<T>
    {
        IStack<T> Empty { get; }

        bool IsEmpty(IStack<T> stack);

        IStack<T> Cons(T element, IStack<T> stack);

        T Head(IStack<T> stack);

        IStack<T> Tail(IStack<T> stack);
    }
}