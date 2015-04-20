using System;

namespace FunProgLib.lists
{
    internal class BinaryRandomAccessOne<T> : BinaryRandomAccessDigit<T>
    {
        protected readonly static BinaryRandomAccessDigit<T> Zero = new BinaryRandomAccessZero<T>();

        private readonly Tree<T> one;

        private static Tree<T> Link(Tree<T> t1, Tree<T> t2)
        {
            return new Node(t1.Size + t2.Size, t1, t2);
        }


        internal override List2<BinaryRandomAccessList<T>> ConsTree(Tree<T> t)
        {
            return Tail.ConsTree(Link(t, one)).Cons(Zero);
        }

        internal override Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>> UnconsTree
        {
            get
            {
                if (Tail.IsEmpty) return new Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>>(one, List2<BinaryRandomAccessList<T>>.Empty);
                return new Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>>(one, Tail.Cons(Zero));
            }
        }

        public override T Lookup(int i)
        {
            if (i < one.Size) return one.LookupTree(i);
            return Tail.Lookup(i - one.Size);
        }

        public override BinaryRandomAccessList<T> Update(int i, T x)
        {
            if (i < one.Size) return Tail.Cons(new BinaryRandomAccessDigit<T>(one.UpdateTree(i, x)));
            return Tail.Update(i - one.Size, x).Cons(new BinaryRandomAccessDigit<T>(one));
        }
    }
}