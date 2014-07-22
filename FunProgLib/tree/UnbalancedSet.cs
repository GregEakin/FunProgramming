// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		UnbalancedSet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    public class UnbalancedSet<K, T> where K : IComparable
    {
        private static readonly UnbalancedSet<K, T> Empty = new UnbalancedSet<K, T> { left = Empty, right = Empty };

        public static UnbalancedSet<K, T> E
        {
            get
            {
                return Empty;
            }
        }

        private K elem;

        private UnbalancedSet<K, T> left;

        private UnbalancedSet<K, T> right;

        public bool Member(K that)
        {
            if (this == Empty)
            {
                return false;
            }

            if (that.CompareTo(elem) < 0) return this.left.Member(that);
            else if (that.CompareTo(this.elem) > 0) return this.right.Member(that);
            else return true;
        }

        public UnbalancedSet<K, T> Insert(K that)
        {
            if (ReferenceEquals(this, Empty)) return new UnbalancedSet<K, T> { left = Empty, elem = that, right = Empty };

            if (that.CompareTo(elem) < 0) return new UnbalancedSet<K, T> { left = this.left.Insert(that), elem = this.elem, right = this.right };
            else if (that.CompareTo(this.elem) > 0) return new UnbalancedSet<K, T> { left = this.left, elem = this.elem, right = this.right.Insert(that) };
            else return this;
        }
    }
}