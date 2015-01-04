﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.2 Binaryt Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

using System;

namespace FunProgLib.tree
{
    public static class UnbalancedSet<T> where T : IComparable<T> // ISet
    {
        public sealed class Tree // : IOrdered<T>
        {
            private readonly Tree a;
            private readonly T y;
            private readonly Tree b;

            public Tree(Tree a, T y, Tree b)
            {
                this.a = a;
                this.y = y;
                this.b = b;
            }

            public Tree A
            {
                get { return a; }
            }

            public T Y
            {
                get { return y; }
            }

            public Tree B
            {
                get { return b; }
            }
        }

        // type Set = Tree

        private static readonly Tree EmptyTree = null;

        public static Tree Empty
        {
            get { return EmptyTree; }
        }

        public static bool Member(T x, Tree s)
        {
            if (s == EmptyTree) return false;
            if (x.CompareTo(s.Y) < 0) return Member(x, s.A);
            if (s.Y.CompareTo(x) < 0) return Member(x, s.B);
            return true;
        }

        public static Tree Insert(T x, Tree s)
        {
            if (s == EmptyTree) return new Tree(EmptyTree, x, EmptyTree);
            if (x.CompareTo(s.Y) < 0) return new Tree(Insert(x, s.A), s.Y, s.B);
            if (s.Y.CompareTo(x) < 0) return new Tree(s.A, s.Y, Insert(x, s.B));
            return s;
        }
    }
}