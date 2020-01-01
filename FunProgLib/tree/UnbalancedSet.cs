// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.2 Binary Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

namespace FunProgLib.tree
{
    using System;

    public static class UnbalancedSet<T> where T : IComparable<T> // ISet
    {
        public sealed class Tree // : IOrdered<T>
        {
            public Tree(Tree a, T y, Tree b)
            {
                A = a;
                Y = y;
                B = b;
            }

            public Tree A { get; }

            public T Y { get; }

            public Tree B { get; }
        }

        // type Set = Tree

        public static Tree Empty { get; } = null;

        public static bool Member(T x, Tree s)
        {
            if (s == Empty) return false;
            if (x.CompareTo(s.Y) < 0) return Member(x, s.A);
            if (s.Y.CompareTo(x) < 0) return Member(x, s.B);
            return true;
        }

        public static Tree Insert(T x, Tree s)
        {
            if (s == Empty) return new Tree(Empty, x, Empty);
            if (x.CompareTo(s.Y) < 0) return new Tree(Insert(x, s.A), s.Y, s.B);
            if (s.Y.CompareTo(x) < 0) return new Tree(s.A, s.Y, Insert(x, s.B));
            return s;
        }
    }
}
