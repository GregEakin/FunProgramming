using System;

namespace FunProgLib.lists
{
    internal class BinaryRandomAccessZero<T> : BinaryRandomAccessDigit<T>
    {
        internal override List2<BinaryRandomAccessList<T>> ConsTree(Tree<T> t)
        {
            return Tail.Cons(new BinaryRandomAccessOne<T>(t));
        }

        internal override Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>> UnconsTree
        {
            get
            {
                var stuff = ((BinaryRandomAccessList<T>)Tail).UnconsTree;
                return stuff.Item1.Cons(stuff.Item2);
            }
        }

        public override T Lookup(int i)
        {
            if (i < Head.One.Size) return Head.One.LookupTree(i);
            return Lookup(i - Head.One.Size, Tail);
        }

        public override List2<BinaryRandomAccessList<T>> Update(int i, T x)
        {
            if (i < ts.Element.One.Size) return List<BinaryRandomAccessList<T>>.Cons(new BinaryRandomAccessList<T>(ts.Element.One.UpdateTree(i, x)), ts.Next);
            return List<BinaryRandomAccessList<T>>.Cons(new BinaryRandomAccessList<T>(ts.Element.One), Update(i - ts.Element.One.Size, x, ts.Next));
        }
    }
}