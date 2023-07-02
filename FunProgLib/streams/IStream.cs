// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "4.2 Streams." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 34-37. Print.

namespace FunProgLib.streams;

public interface IStream<T>
{
    IStream<T> Append(IStream<T> left, IStream<T> right);  // ⧺

    IStream<T> Take(int n, IStream<T> stream);

    IStream<T> Drop(int n, IStream<T> stream);

    IStream<T> Reverse(IStream<T> stream);
}