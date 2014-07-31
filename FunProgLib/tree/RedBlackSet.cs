// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		RedBlackSet.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    public static class RedBlackSet<T> where T : IComparable
    {
        public enum Color { R, B };

        public class Tree
        {
            private readonly Color color;

            private readonly Tree tree1;

            private readonly T elem;

            private readonly Tree tree2;

            public Tree(Color color, Tree tree1, T elem, Tree tree2)
            {
                this.color = color;
                this.tree1 = tree1;
                this.elem = elem;
                this.tree2 = tree2;
            }

            public Color Color
            {
                get { return this.color; }
            }

            public Tree Tree1
            {
                get { return this.tree1; }
            }

            public T Elem
            {
                get { return this.elem; }
            }

            public Tree Tree2
            {
                get { return this.tree2; }
            }
        }

        private static readonly Tree Empty = null;

        public static Tree EmptyTree
        {
            get { return Empty; }
        }

        public static bool Member(T x, Tree t)
        {
            if (t == Empty) return false;
            if (x.CompareTo(t.Elem) < 0) return Member(x, t.Tree1);
            if (t.Elem.CompareTo(x) < 0) return Member(x, t.Tree2);
            return true;
        }

        private static Tree Balance(Color color, Tree tree1, T x, Tree tree2)
        {
            if (color == Color.B)
            {
                if (tree1 != Empty && tree1.Color == Color.R)
                {
                    if (tree1.Tree1 != Empty && tree1.Tree1.Color == Color.R)
                        return new Tree(Color.R, new Tree(Color.B, tree1.Tree1.Tree1, tree1.Tree1.Elem, tree1.Tree1.Tree2), tree1.Elem, new Tree(Color.B, tree1.Tree2, x, tree2));

                    if (tree1.Tree2 != Empty && tree1.Tree2.Color == Color.R)
                        return new Tree(Color.R, new Tree(Color.B, tree1.Tree1, tree1.Elem, tree1.Tree2.Tree1), tree1.Tree2.Elem, new Tree(Color.B, tree1.Tree2.Tree2, x, tree2));
                }

                if (tree2 != Empty && tree2.Color == Color.R)
                {
                    if (tree2.Tree1 != Empty && tree2.Tree1.Color == Color.R)
                        return new Tree(Color.R, new Tree(Color.B, tree1, x, tree2.Tree1.Tree1), tree2.Tree1.Elem, new Tree(Color.B, tree2.Tree1.Tree2, tree2.Elem, tree2.Tree2));

                    if (tree2.Tree2 != Empty && tree2.Tree2.Color == Color.R)
                        return new Tree(Color.R, new Tree(Color.B, tree1, x, tree2.Tree1), tree2.Elem, new Tree(Color.B, tree2.Tree2.Tree2, tree2.Tree2.Elem, tree2.Tree2.Tree2));
                }
            }

            return new Tree(color, tree1, x, tree2);
        }

        public static Tree Insert(T x, Tree s)
        {
            var val = Ins(x, s);
            return new Tree(Color.B, val.Tree1, val.Elem, val.Tree2);
        }

        private static Tree Ins(T x, Tree s)
        {
            if (s == Empty) return new Tree(Color.R, Empty, x, Empty);
            if (x.CompareTo(s.Elem) < 0) return Balance(s.Color, Ins(x, s.Tree1), s.Elem, s.Tree2);
            if (s.Elem.CompareTo(x) < 0) return Balance(s.Color, s.Tree1, s.Elem, Ins(x, s.Tree2));
            return s;
        }
    }
}