// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		SkewBinaryRandomAccessList.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.persistence
{
    using System;

    public static class SkewBinaryRandomAccessList<T>
    {
        public abstract class Tree
        {
            private readonly T alpha;

            protected Tree(T alpha)
            {
                this.alpha = alpha;
            }

            public T Alpha
            {
                get { return this.alpha; }
            }
        }

        private sealed class Leaf : Tree
        {
            public Leaf(T alpha)
                : base(alpha)
            {
            }
        }

        private sealed class Node : Tree
        {
            private readonly Tree tree1;

            private readonly Tree tree2;

            public Node(T alpha, Tree tree1, Tree tree2)
                : base(alpha)
            {
                this.tree1 = tree1;
                this.tree2 = tree2;
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

        public sealed class Stuff
        {
            private readonly int weight;

            private readonly Tree tree;

            public Stuff(int weight, Tree tree)
            {
                this.weight = weight;
                this.tree = tree;
            }

            public int Weight
            {
                get { return this.weight; }
            }

            public Tree Tree
            {
                get { return this.tree; }
            }
        }

        private static readonly LinkList<Stuff>.List EmptyList = null;

        public static LinkList<Stuff>.List Empty
        {
            get { return EmptyList; }
        }

        public static bool IsEmpty(LinkList<Stuff>.List list)
        {
            return list == EmptyList;
        }

        public static LinkList<Stuff>.List Cons(T x, LinkList<Stuff>.List ts)
        {
            if (ts == EmptyList)
                return LinkList<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);

            var head1 = LinkList<Stuff>.Head(ts);
            var tail1 = LinkList<Stuff>.Tail(ts);
            if (tail1 == EmptyList)
                return LinkList<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);

            var head2 = LinkList<Stuff>.Head(tail1);
            var tail2 = LinkList<Stuff>.Tail(tail1);

            if (head1.Weight == head2.Weight) return LinkList<Stuff>.Cons(new Stuff(1 + head1.Weight + head2.Weight, new Node(x, head1.Tree, head2.Tree)), tail2);
            return LinkList<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);
        }

        public static T Head(LinkList<Stuff>.List ts)
        {
            if (ts == EmptyList) throw new Exception("Empty");
            var head = LinkList<Stuff>.Head(ts);
            return head.Tree.Alpha;
        }

        public static LinkList<Stuff>.List Tail(LinkList<Stuff>.List ts)
        {
            if (ts == EmptyList) throw new Exception("Empty");
            var head = LinkList<Stuff>.Head(ts);
            if (head.Tree is Leaf) return LinkList<Stuff>.Tail(ts);
            var node = head.Tree as Node;
            if (node != null) return LinkList<Stuff>.Cons(new Stuff(head.Weight / 2, node.Tree1), LinkList<Stuff>.Cons(new Stuff(head.Weight / 2, node.Tree2), LinkList<Stuff>.Tail(ts)));
            throw new Exception();
        }

        private static T LookupTree(int w, int i, Tree t)
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
                if (i == 0) return node.Alpha;
                if (i <= w / 2) return LookupTree(w / 2, i - 1, node.Tree1);
                return LookupTree(w / 2, i - 1 - w / 2, node.Tree2);
            }

            throw new Exception();
        }

        private static Tree UpdateTree(int w, int i, T x, Tree t)
        {
            var leaf = t as Leaf;
            if (w == 1 && leaf != null)
            {
                if (i == 0) return new Leaf(x);
                throw new Exception("Subscript");
            }

            var node = t as Node;
            if (node != null)
            {
                if (i == 0) return new Node(x, node.Tree1, node.Tree2);
                if (i <= w / 2) return new Node(node.Alpha, UpdateTree(w / 2, i - 1, x, node.Tree1), node.Tree2);
                return new Node(node.Alpha, node.Tree1, UpdateTree(w / 2, i - 1 - w / 2, x, node.Tree2));
            }

            throw new Exception();
        }

        public static T Lookup(int i, LinkList<Stuff>.List ts)
        {
            if (ts == null) throw new Exception("Subscript");
            var head = LinkList<Stuff>.Head(ts);
            if (i < head.Weight) return LookupTree(head.Weight, i, head.Tree);
            var tail = LinkList<Stuff>.Tail(ts);
            return Lookup(i - head.Weight, tail);
        }

        public static LinkList<Stuff>.List Update(int i, T x, LinkList<Stuff>.List ts)
        {
            if (ts == null) throw new Exception("Subscript");
            var head = LinkList<Stuff>.Head(ts);
            var tail = LinkList<Stuff>.Tail(ts);
            if (i < head.Weight) return LinkList<Stuff>.Cons(new Stuff(head.Weight, UpdateTree(head.Weight, i, x, head.Tree)), tail);
            return LinkList<Stuff>.Cons(head, Update(i - head.Weight, x, tail));
        }
    }
}