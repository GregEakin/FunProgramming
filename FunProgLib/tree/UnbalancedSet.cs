// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		UnbalancedSet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    public static class UnbalancedSet<T> where T : IComparable
    {
        public class Tree
        {
            private readonly Tree left;

            private readonly T element;

            private readonly Tree right;

            public Tree(Tree left, T element, Tree right)
            {
                this.left = left;
                this.element = element;
                this.right = right;
            }

            public Tree Left
            {
                get { return this.left; }
            }

            public T Element
            {
                get { return this.element; }
            }

            public Tree Right
            {
                get { return this.right; }
            }
        }

        private static readonly Tree EmptyTree = new Tree(EmptyTree, default(T), EmptyTree);

        public static Tree Empty
        {
            get
            {
                return EmptyTree;
            }
        }

        public static bool Member(Tree tree, T value)
        {
            if (tree == EmptyTree)
            {
                return false;
            }

            if (value.CompareTo(tree.Element) < 0) return Member(tree.Left, value);
            if (value.CompareTo(tree.Element) > 0) return Member(tree.Right, value);
            return true;
        }

        public static Tree Insert(Tree tree, T value)
        {
            if (tree == EmptyTree) return new Tree(EmptyTree, value, EmptyTree);
            if (value.CompareTo(tree.Element) < 0) return new Tree(Insert(tree.Left, value), tree.Element, tree.Right);
            if (value.CompareTo(tree.Element) > 0) return new Tree(tree.Left, tree.Element, Insert(tree.Right, value));
            return tree;
        }
    }
}