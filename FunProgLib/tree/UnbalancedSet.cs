// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		UnbalancedSet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    public static class UnbalancedSet<T> where T : IComparable // ISet
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
                get { return this.a; }
            }

            public T Y
            {
                get { return this.y; }
            }

            public Tree B
            {
                get { return this.b; }
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