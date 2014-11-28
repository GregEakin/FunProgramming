// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgLib.streams
{
    public interface IStream<T>
    {
        IStream<T> Append(IStream<T> left, IStream<T> right);  // ⧺

        IStream<T> Take(int n, IStream<T> stream);

        IStream<T> Drop(int n, IStream<T> stream);

        IStream<T> Reverse(IStream<T> stream);
    }
}