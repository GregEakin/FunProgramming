// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.3.1 Skew Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 132-34. Print.

namespace FunProgLib.lists
{
    using System;

    public static class SkewBinaryRandomAccessList<T> // : IRandomAccessList<T>
    {
        public abstract class Tree
        {
            protected Tree(T alpha)
            {
                Alpha = alpha;
            }

            public T Alpha { get; }
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
            public Node(T alpha, Tree tree1, Tree tree2)
                : base(alpha)
            {
                Tree1 = tree1;
                Tree2 = tree2;
            }

            public Tree Tree1 { get; }

            public Tree Tree2 { get; }
        }

        public sealed class Stuff
        {
            public Stuff(int weight, Tree tree)
            {
                Weight = weight;
                Tree = tree;
            }

            public int Weight { get; }

            public Tree Tree { get; }
        }

        public static List<Stuff>.Node Empty => List<Stuff>.Empty;

        public static bool IsEmpty(List<Stuff>.Node list) => List<Stuff>.IsEmpty(list);

        public static List<Stuff>.Node Cons(T x, List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts))
                return List<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);

            var head1 = List<Stuff>.Head(ts);
            var tail1 = List<Stuff>.Tail(ts);
            if (List<Stuff>.IsEmpty(tail1))
                return List<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);

            var head2 = List<Stuff>.Head(tail1);
            var tail2 = List<Stuff>.Tail(tail1);

            if (head1.Weight == head2.Weight) return List<Stuff>.Cons(new Stuff(1 + head1.Weight + head2.Weight, new Node(x, head1.Tree, head2.Tree)), tail2);
            return List<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);
        }

        public static T Head(List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new ArgumentNullException(nameof(ts));
            var head = List<Stuff>.Head(ts);
            return head.Tree.Alpha;
        }

        public static List<Stuff>.Node Tail(List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new ArgumentNullException(nameof(ts));
            var head = List<Stuff>.Head(ts);
            if (head.Tree is Leaf) return List<Stuff>.Tail(ts);
            if (head.Tree is Node node) return List<Stuff>.Cons(new Stuff(head.Weight / 2, node.Tree1), List<Stuff>.Cons(new Stuff(head.Weight / 2, node.Tree2), List<Stuff>.Tail(ts)));
            throw new ArgumentException("head.Tree as Node is null", nameof(ts));
        }

        private static T LookupTree(int w, int i, Tree t)
        {
            if (t is Leaf leaf)
            {
                if (i == 0) return leaf.Alpha;
                throw new Exception("Subscript");
            }

            if (t is Node node)
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

            if (t is Node node)
            {
                if (i == 0) return new Node(x, node.Tree1, node.Tree2);
                if (i <= w / 2) return new Node(node.Alpha, UpdateTree(w / 2, i - 1, x, node.Tree1), node.Tree2);
                return new Node(node.Alpha, node.Tree1, UpdateTree(w / 2, i - 1 - w / 2, x, node.Tree2));
            }

            throw new Exception();
        }

        public static T Lookup(int i, List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
            var head = List<Stuff>.Head(ts);
            if (i < head.Weight) return LookupTree(head.Weight, i, head.Tree);
            var tail = List<Stuff>.Tail(ts);
            return Lookup(i - head.Weight, tail);
        }

        public static List<Stuff>.Node Update(int i, T x, List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
            var head = List<Stuff>.Head(ts);
            var tail = List<Stuff>.Tail(ts);
            if (i < head.Weight) return List<Stuff>.Cons(new Stuff(head.Weight, UpdateTree(head.Weight, i, x, head.Tree)), tail);
            return List<Stuff>.Cons(head, Update(i - head.Weight, x, tail));
        }
    }
}