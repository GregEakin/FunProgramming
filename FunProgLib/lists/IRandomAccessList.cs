// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists
{
    public interface IRandomAccessList<T>
    {
        IRandomAccessList<T> Empty { get; }

        bool IsEmpty(IRandomAccessList<T> list);

        IRandomAccessList<T> Cons(T element, IRandomAccessList<T> list);

        T Head(IRandomAccessList<T> list);

        IRandomAccessList<T> Tail(IRandomAccessList<T> list);

        T Lookup(int index, IRandomAccessList<T> list);

        IRandomAccessList<T> Update(int index, T element, IRandomAccessList<T> list);
    }
}