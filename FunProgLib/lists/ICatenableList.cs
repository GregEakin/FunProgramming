// Fun Programming Data Structures 1.0
// 
// Copyright © 2016 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "!0.2.1 Lists With Efficient Catenation." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 153-8. Print.

namespace FunProgLib.lists;

public interface ICatenableList<T>
{
    ICatenableList<T> Empty { get; }
    bool IsEmpty(ICatenableList<T> list);

    ICatenableList<T> Cons(T element, ICatenableList<T> list);
    ICatenableList<T> Snoc(ICatenableList<T> list, T element);
    ICatenableList<T> Cat(ICatenableList<T> list1, ICatenableList<T> list2);

    T Head(ICatenableList<T> list);
    ICatenableList<T> Tail(ICatenableList<T> list);
}