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

using System;

namespace FunProgLib.lists
{
    public static class SkewBinaryRandomAccessList<T> // : IRandomAccessList<T>
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
                get { return alpha; }
            }

            public abstract T LookupTree(int w, int i);

            public abstract Tree UpdateTree(int w, int i, T x);

            public abstract List<Stuff>.Node Tail(int weight, List<Stuff>.Node ts);
        }

        private sealed class Leaf : Tree
        {
            public Leaf(T alpha)
                : base(alpha)
            {
            }

            public override T LookupTree(int w, int i)
            {
                if (i == 0) return Alpha;
                throw new Exception("Subscript");
            }

            public override Tree UpdateTree(int w, int i, T x)
            {
                if (w != 1) throw new Exception();
                if (i != 0) throw new Exception("Subscript");
                return new Leaf(x);
            }

            public override List<Stuff>.Node Tail(int weight, List<Stuff>.Node ts)
            {
                return List<Stuff>.Tail(ts);
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

            public override T LookupTree(int w, int i)
            {
                if (i == 0) return Alpha;
                return i <= w / 2
                    ? tree1.LookupTree(w / 2, i - 1)
                    : tree2.LookupTree(w / 2, i - 1 - w / 2);
            }

            public override Tree UpdateTree(int w, int i, T x)
            {
                if (i == 0) return new Node(x, tree1, tree2);
                return i <= w / 2
                    ? new Node(Alpha, tree1.UpdateTree(w / 2, i - 1, x), tree2)
                    : new Node(Alpha, tree1, tree2.UpdateTree(w / 2, i - 1 - w / 2, x));
            }

            public override List<Stuff>.Node Tail(int weight, List<Stuff>.Node ts)
            {
                var element = new Stuff(weight / 2, tree1);
                var list = List<Stuff>.Cons(new Stuff(weight / 2, tree2), List<Stuff>.Tail(ts));
                return List<Stuff>.Cons(element, list);
            }
        }

        public struct Stuff
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
                get { return weight; }
            }

            public Tree Tree
            {
                get { return tree; }
            }
        }

        public static List<Stuff>.Node Empty
        {
            get { return List<Stuff>.Empty; }
        }

        public static bool IsEmpty(List<Stuff>.Node list)
        {
            return List<Stuff>.IsEmpty(list);
        }

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
            return head1.Weight == head2.Weight
                ? List<Stuff>.Cons(new Stuff(1 + head1.Weight + head2.Weight, new Node(x, head1.Tree, head2.Tree)), tail2)
                : List<Stuff>.Cons(new Stuff(1, new Leaf(x)), ts);
        }

        public static T Head(List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new Exception("Empty");
            var head = List<Stuff>.Head(ts);
            return head.Tree.Alpha;
        }

        public static List<Stuff>.Node Tail(List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new Exception("Empty");
            var head = List<Stuff>.Head(ts);
            return head.Tree.Tail(head.Weight, ts);
        }

        public static T Lookup(int i, List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new Exception("Subscript");
            var head = List<Stuff>.Head(ts);
            if (i < head.Weight) return head.Tree.LookupTree(head.Weight, i);
            var tail = List<Stuff>.Tail(ts);
            return Lookup(i - head.Weight, tail);
        }

        public static List<Stuff>.Node Update(int i, T x, List<Stuff>.Node ts)
        {
            if (List<Stuff>.IsEmpty(ts)) throw new Exception("Subscript");
            var head = List<Stuff>.Head(ts);
            var tail = List<Stuff>.Tail(ts);
            return i < head.Weight
                ? List<Stuff>.Cons(new Stuff(head.Weight, head.Tree.UpdateTree(head.Weight, i, x)), tail)
                : List<Stuff>.Cons(head, Update(i - head.Weight, x, tail));
        }
    }
}
