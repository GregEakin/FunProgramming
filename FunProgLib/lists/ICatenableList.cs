// Fun Programming Data Structures 1.0
// 
// Copyright © 2014-5 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.1.2 Binary Random-Access Lists Revisited." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 153. Print.

namespace FunProgLib.lists
{
    public interface ICatenableList<T>
    {
        // type alpha Cat

        ICatenableList<T> Empty { get; }
        bool IsEmpty(ICatenableList<T> rlist);

        ICatenableList<T> Cons(T element, ICatenableList<T> rlist);
        ICatenableList<T> Snoc(ICatenableList<T> rlist, T element);
        ICatenableList<T> Cat(ICatenableList<T> left, ICatenableList<T> right);

        T Head(ICatenableList<T> rlist);    // raises Empty
        ICatenableList<T> Tail(ICatenableList<T> rlist);  // raises Empty
    }
}