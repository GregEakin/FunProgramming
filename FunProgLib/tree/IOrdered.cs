// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgLib.tree
{
    public interface IOrdered<in T>
    {
        bool Equal(T left, T right);

        bool LessThan(T left, T right);

        bool LessThanEqual(T left, T right);
    }
}