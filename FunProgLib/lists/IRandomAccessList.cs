// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists
{
    public interface IRandomAccessList<T>
    {
        // datatype alpha Tree = Leaf of alpha | Node of int x alpha Tree x alpha Tree
        // datatype alpha Digit = Zero | One of alpha Tree
        // type alpha RList = alpha Digit list

        IRandomAccessList<T> Empty { get; }

        bool IsEmpty(IRandomAccessList<T> rlist);

        IRandomAccessList<T> Cons(T element, IRandomAccessList<T> rlist);

        T Head(IRandomAccessList<T> rlist);

        IRandomAccessList<T> Tail(IRandomAccessList<T> rlist);

        T Lookup(int index, IRandomAccessList<T> rlist);

        IRandomAccessList<T> Update(int index, T element, IRandomAccessList<T> rlist);
    }
}