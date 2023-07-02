// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.2 Binaryt Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

namespace FunProgLib.tree;

public interface IOrdered<in T>
{
    bool Equal(T left, T right);

    bool LessThan(T left, T right);

    bool LessThanEqual(T left, T right);
}