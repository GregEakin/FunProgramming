// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		BinaryRandomAccessList.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.lists
{
    using System;

    public static class BinaryRandomAccessList<T>
    {
        public abstract class Tree
        {
        }

        private sealed class Leaf : Tree
        {
            private readonly T alpha;

            public Leaf(T alpha)
            {
                this.alpha = alpha;
            }

            public T Alpha
            {
                get { return this.alpha; }
            }
        }

        private sealed class Node : Tree
        {
            private readonly int index;

            private readonly Tree tree1;

            private readonly Tree tree2;

            public Node(int index, Tree tree1, Tree tree2)
            {
                this.index = index;
                this.tree1 = tree1;
                this.tree2 = tree2;
            }

            public int Index
            {
                get { return this.index; }
            }

            public Tree Tree1
            {
                get { return this.tree1; }
            }

            public Tree Tree2
            {
                get { return this.tree2; }
            }
        }

        public sealed class Digit
        {
            private readonly Tree one;

            public Digit(Tree tree)
            {
                this.one = tree;
            }

            public Tree One
            {
                get { return this.one; }
            }
        }

        private readonly static Digit Zero = new Digit(null);

        private sealed class Stuff
        {
            private readonly Tree tree;

            private readonly LinkList<Digit>.List list;

            public Stuff(Tree tree, LinkList<Digit>.List list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return this.tree; }
            }

            public LinkList<Digit>.List List
            {
                get { return this.list; }
            }
        }

        private static readonly LinkList<Digit>.List EmptyList = null;

        public static LinkList<Digit>.List Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmpty(LinkList<Digit>.List list)
        {
            return list == EmptyList;
        }

        private static int Size(Tree tree)
        {
            var node = tree as Node;
            if (node != null) return node.Index;
            return 1;
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            return new Node(Size(t1) + Size(t2), t1, t2);
        }

        private static LinkList<Digit>.List ConsTree(Tree t, LinkList<Digit>.List ts)
        {
            if (IsEmpty(ts)) return LinkList<Digit>.Cons(new Digit(t), EmptyList);
            if (ts.Element == Zero) return LinkList<Digit>.Cons(new Digit(t), ts.Next);
            return LinkList<Digit>.Cons(Zero, ConsTree(Link(t, ts.Element.One), ts.Next));
        }

        private static Stuff UnconsTree(LinkList<Digit>.List list)
        {
            if (IsEmpty(list)) throw new Exception("Empty");
            if (list.Element == Zero)
            {
                var stuff = UnconsTree(list.Next);
                var node = stuff.Tree as Node;
                if (node != null) return new Stuff(node.Tree1, LinkList<Digit>.Cons(new Digit(node.Tree2), stuff.List));
                throw new Exception();
            }
            if (IsEmpty(list.Next)) return new Stuff(list.Element.One, EmptyList);
            return new Stuff(list.Element.One, LinkList<Digit>.Cons(Zero, list.Next));
        }

        public static LinkList<Digit>.List Cons(T x, LinkList<Digit>.List ts)
        {
            return ConsTree(new Leaf(x), ts);
        }

        public static T Head(LinkList<Digit>.List ts)
        {
            var stuff = UnconsTree(ts);
            var leaf = stuff.Tree as Leaf;
            if (leaf != null) return leaf.Alpha;
            throw new Exception();
        }

        public static LinkList<Digit>.List Tail(LinkList<Digit>.List ts)
        {
            var stuff = UnconsTree(ts);
            return stuff.List;
        }

        private static T LookupTree(int i, Tree t)
        {
            var leaf = t as Leaf;
            if (leaf != null)
            {
                if (i == 0) return leaf.Alpha;
                throw new Exception("Subscript");
            }

            var node = t as Node;
            if (node != null)
            {
                if (i < node.Index / 2) return LookupTree(i, node.Tree1);
                return LookupTree(i - node.Index / 2, node.Tree2);
            }

            throw new Exception();
        }

        private static Tree UpdateTree(int i, T x, Tree t)
        {
            var leaf = t as Leaf;
            if (leaf != null)
            {
                if (i == 0) return new Leaf(x);
                throw new Exception("Subscript");
            }

            var node = t as Node;
            if (node != null)
            {
                if (i < node.Index / 2) return new Node(node.Index, UpdateTree(i, x, node.Tree1), node.Tree2);
                return new Node(node.Index, node.Tree1, UpdateTree(i - node.Index / 2, x, node.Tree2));
            }

            throw new Exception();
        }

        public static T Lookup(int i, LinkList<Digit>.List ts)
        {
            if (IsEmpty(ts)) throw new Exception("Subscript");
            if (ts.Element == Zero) return Lookup(i, ts.Next);
            if (i < Size(ts.Element.One)) return LookupTree(i, ts.Element.One);
            return Lookup(i - Size(ts.Element.One), ts.Next);
        }

        public static LinkList<Digit>.List Update(int i, T x, LinkList<Digit>.List ts)
        {
            if (IsEmpty(ts)) throw new Exception("Subscript");
            if (ts.Element == Zero) return LinkList<Digit>.Cons(Zero, Update(i, x, ts.Next));
            if (i < Size(ts.Element.One)) return LinkList<Digit>.Cons(new Digit(UpdateTree(i, x, ts.Element.One)), ts.Next);
            return LinkList<Digit>.Cons(new Digit(ts.Element.One), Update(i - Size(ts.Element.One), x, ts.Next));
        }
    }
}