// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		UnbalancedSet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;
    using System.Text;

    public class UnbalancedSet<T> where T : IComparable
    {
        private static readonly UnbalancedSet<T> EmptyTree = new UnbalancedSet<T>(EmptyTree, default(T), EmptyTree);

        public static UnbalancedSet<T> Empty
        {
            get
            {
                return EmptyTree;
            }
        }

        private readonly T element;

        private readonly UnbalancedSet<T> left;

        private readonly UnbalancedSet<T> right;

        private UnbalancedSet(UnbalancedSet<T> left, T element, UnbalancedSet<T> right)
        {
            this.left = left;
            this.element = element;
            this.right = right;
        }

        public bool Member(T value)
        {
            if (this == EmptyTree)
            {
                return false;
            }

            if (value.CompareTo(this.element) < 0) return this.left.Member(value);
            else if (value.CompareTo(this.element) > 0) return this.right.Member(value);
            else return true;
        }

        public UnbalancedSet<T> Insert(T value)
        {
            if (this == EmptyTree) return new UnbalancedSet<T>(EmptyTree, value, EmptyTree);

            if (value.CompareTo(this.element) < 0) return new UnbalancedSet<T>(this.left.Insert(value), this.element, this.right);
            else if (value.CompareTo(this.element) > 0) return new UnbalancedSet<T>(this.left, this.element, this.right.Insert(value));
            else return this;
        }

        public override string ToString()
        {
            return DumpTree(this);
        }

        private static string DumpTree(UnbalancedSet<T> tree)
        {
            if (tree == EmptyTree) return "\u2205";

            var results = new StringBuilder();

            if (tree.left != EmptyTree)
            {
                results.Append(tree.left);
            }

            results.Append(tree.element);
            results.Append(", ");

            if (tree.right != EmptyTree)
            {
                results.Append(tree.right);
            }

            return results.ToString();
        }
    }
}