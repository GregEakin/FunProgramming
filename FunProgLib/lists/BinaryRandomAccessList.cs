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
        {
            internal virtual T Head
            {
                get { throw new Exception(); }
            }

            public abstract int Size { get; }

            public abstract T LookupTree(int i);

            public abstract Tree UpdateTree(int i, T x);

            internal virtual Stuff Cons(List<Digit>.Node list)
            {
                throw new Exception();
            }
        }

        private sealed class Leaf : Tree
        {
            private readonly T alpha;

            public Leaf(T alpha)
            {
                this.alpha = alpha;
            }

            internal override T Head
            {
                get { return alpha; }
            }

            public override int Size
            {
                get { return 1; }
            }

            public override T LookupTree(int i)
            {
                if (i == 0) return alpha;
                throw new Exception("Subscript");
            }

            public override Tree UpdateTree(int i, T x)
            {
                if (i == 0) return new Leaf(x);
                throw new Exception("Subscript");
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

            public override int Size
            {
                get { return index; }
            }

            public override T LookupTree(int i)
            {
                return i < index / 2
                    ? tree1.LookupTree(i)
                    : tree2.LookupTree(i - index / 2);
            }

            public override Tree UpdateTree(int i, T x)
            {
                return i < index / 2
                    ? new Node(index, tree1.UpdateTree(i, x), tree2)
                    : new Node(index, tree1, tree2.UpdateTree(i - index / 2, x));
            }

            internal override Stuff Cons(List<Digit>.Node list)
            {
                return new Stuff(tree1, List<Digit>.Cons(new Digit(tree2), list));
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
                get { return one; }
            }
        }

        private readonly static Digit Zero = new Digit(null);

        internal sealed class Stuff
        {
            private readonly Tree tree;

            private readonly List<Digit>.Node list;

            public Stuff(Tree tree, List<Digit>.Node list)
            {
                this.tree = tree;
                this.list = list;
            }

            public Tree Tree
            {
                get { return tree; }
            }

            public List<Digit>.Node List
            {
                get { return list; }
            }
        }

        public static List<Digit>.Node Empty
        {
            get { return List<Digit>.Empty; }
        }

        public static bool IsEmpty(List<Digit>.Node list)
        {
            return List<Digit>.IsEmpty(list);
        }

        private static Tree Link(Tree t1, Tree t2)
        {
            return new Node(t1.Size + t2.Size, t1, t2);
        }

        private static List<Digit>.Node ConsTree(Tree t, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) return List<Digit>.Cons(new Digit(t), List<Digit>.Empty);
            if (ts.Element == Zero) return List<Digit>.Cons(new Digit(t), ts.Next);
            return List<Digit>.Cons(Zero, ConsTree(Link(t, ts.Element.One), ts.Next));
        }

        private static Stuff UnconsTree(List<Digit>.Node list)
        {
            if (IsEmpty(list)) throw new Exception("Empty");
            if (list.Element == Zero)
            {
                var stuff = UnconsTree(list.Next);
                return stuff.Tree.Cons(stuff.List);
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
            return stuff.Tree.Head;
        }

        public static List<Digit>.Node Tail(List<Digit>.Node ts)
        {
            var stuff = UnconsTree(ts);
            return stuff.List;
        }

        public static T Lookup(int i, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) throw new Exception("Subscript");
            if (ts.Element == Zero) return Lookup(i, ts.Next);
            if (i < ts.Element.One.Size) return ts.Element.One.LookupTree(i);
            return Lookup(i - ts.Element.One.Size, ts.Next);
        }

        public static List<Digit>.Node Update(int i, T x, List<Digit>.Node ts)
        {
            if (IsEmpty(ts)) throw new Exception("Subscript");
            if (ts.Element == Zero) return List<Digit>.Cons(Zero, Update(i, x, ts.Next));
            if (i < ts.Element.One.Size) return List<Digit>.Cons(new Digit(ts.Element.One.UpdateTree(i, x)), ts.Next);
            return List<Digit>.Cons(new Digit(ts.Element.One), Update(i - ts.Element.One.Size, x, ts.Next));
        }
    }
}