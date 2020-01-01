// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.3.2 Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 166-9. Print.

using System;

namespace FunProgLib.tree
{
    public static class TrieOfTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        // assumes polymorphic recursion!
        public sealed class Tree
        {
            // alpha
            // albha tree
            // alpha tree
        }

        public sealed class Map
        {
            // alpha option
            // alpha map map M.map
        }

        public static Map Empty { get; } = new Map();

        public static TValue Lookup()
        {
            throw new NotImplementedException();
        }

        public static Map Bind()
        {
            throw new NotImplementedException();
        }
    }
}