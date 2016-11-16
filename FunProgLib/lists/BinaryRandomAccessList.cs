// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists
{
    using System;

    public static class BinaryRandomAccessList<T> // : IRandomAccessList<T>
    {
        public abstract class Tree
        {}

        private sealed class Leaf : Tree
        {
            public Leaf(T alpha)
            {
                Alpha = alpha;
            }

            public T Alpha { get; }
        }

        private sealed class Node : Tree
        {
            public Node(int index, Tree tree1, Tree tree2)
            {
                Index = index;
                Tree1 = tree1;
                Tree2 = tree2;
            }

            public int Index { get; }

            public Tree Tree1 { get; }

            public Tree Tree2 { get; }
        }

        public sealed class Digit
        {
            public Digit(Tree tree)
            {
                One = tree;
            }

            public Tree One { get; }
        }

        private static readonly Digit Zero = new Digit(null);

        private sealed class Stuff
        {
            public Stuff(Tree tree, List<Digit>.Node list)
            {
                Tree = tree;
                List = list;
            }

            public Tree Tree { get; }

            public List<Digit>.Node List { get; }
        }

        public static List<Digit>.Node Empty => List<Digit>.Empty;

        public static bool IsEmpty(List<Digit>.Node list)
        {
            return List<Digit>.IsEmpty(list);
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

        private static List<Digit>.Node ConsTree(Tree t, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) return List<Digit>.Cons(new Digit(t), List<Digit>.Empty);
            if (ts.Element == Zero) return List<Digit>.Cons(new Digit(t), ts.Next);
            return List<Digit>.Cons(Zero, ConsTree(Link(t, ts.Element.One), ts.Next));
        }

        private static Stuff UnconsTree(List<Digit>.Node list)
        {
            if (IsEmpty(list)) throw new ArgumentException("Empty", nameof(list));
            if (list.Element == Zero)
            {
                var stuff = UnconsTree(list.Next);
                var node = stuff.Tree as Node;
                if (node != null) return new Stuff(node.Tree1, List<Digit>.Cons(new Digit(node.Tree2), stuff.List));
                throw new Exception();
            }
            if (IsEmpty(list.Next)) return new Stuff(list.Element.One, List<Digit>.Empty);
            return new Stuff(list.Element.One, List<Digit>.Cons(Zero, list.Next));
        }

        public static List<Digit>.Node Cons(T x, List<Digit>.Node ts)
        {
            return ConsTree(new Leaf(x), ts);
        }

        public static T Head(List<Digit>.Node ts)
        {
            var stuff = UnconsTree(ts);
            var leaf = stuff.Tree as Leaf;
            if (leaf != null) return leaf.Alpha;
            throw new Exception();
        }

        public static List<Digit>.Node Tail(List<Digit>.Node ts)
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

            throw new ArgumentException("Argument t needs to be a Leaf or None.", nameof(t));
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

            throw new ArgumentException("Argument t needs to be a Leaf or None.", nameof(t));
        }

        public static T Lookup(int i, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
            if (ts.Element == Zero) return Lookup(i, ts.Next);
            if (i < Size(ts.Element.One)) return LookupTree(i, ts.Element.One);
            return Lookup(i - Size(ts.Element.One), ts.Next);
        }

        public static List<Digit>.Node Update(int i, T x, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
            if (ts.Element == Zero) return List<Digit>.Cons(Zero, Update(i, x, ts.Next));
            if (i < Size(ts.Element.One)) return List<Digit>.Cons(new Digit(UpdateTree(i, x, ts.Element.One)), ts.Next);
            return List<Digit>.Cons(new Digit(ts.Element.One), Update(i - Size(ts.Element.One), x, ts.Next));
        }
    }
}