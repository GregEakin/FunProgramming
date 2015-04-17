// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

namespace FunProgLib.lists
{
    public interface IStack<T>
    {
        bool IsEmpty { get; }

        IStack<T> Cons(T element);

        T Head { get; }

        IStack<T> Tail { get; }
    }
}