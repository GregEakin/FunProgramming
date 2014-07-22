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
        private static readonly UnbalancedSet<T> Empty = new UnbalancedSet<T> { left = Empty, right = Empty };

        public static UnbalancedSet<T> E
        {
            get
            {
                return Empty;
            }
        }

        private T elem;

        private UnbalancedSet<T> left;

        private UnbalancedSet<T> right;

        public bool Member(T that)
        {
            if (this == Empty)
            {
                return false;
            }

            if (that.CompareTo(elem) < 0) return this.left.Member(that);
            else if (that.CompareTo(this.elem) > 0) return this.right.Member(that);
            else return true;
        }

        public UnbalancedSet<T> Insert(T that)
        {
            if (ReferenceEquals(this, Empty)) return new UnbalancedSet<T> { left = Empty, elem = that, right = Empty };

            if (that.CompareTo(elem) < 0) return new UnbalancedSet<T> { left = this.left.Insert(that), elem = this.elem, right = this.right };
            else if (that.CompareTo(this.elem) > 0) return new UnbalancedSet<T> { left = this.left, elem = this.elem, right = this.right.Insert(that) };
            else return this;
        }

        public override string ToString()
        {
            return DumpTree(this);
        }

        private static string DumpTree(UnbalancedSet<T> tree)
        {
            if (ReferenceEquals(tree, Empty)) return "\u2205";

            var results = new StringBuilder();

            if (!ReferenceEquals(tree.left, Empty))
            {
                results.Append(tree.left);
            }

            results.Append(tree.elem);
            results.Append(", ");

            if (!ReferenceEquals(tree.right, Empty))
            {
                results.Append(tree.right);
            }

            return results.ToString();
        }
    }
}