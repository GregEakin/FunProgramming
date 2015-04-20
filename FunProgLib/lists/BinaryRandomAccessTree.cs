using System;

namespace FunProgLib.lists
{
    internal abstract class Tree<T>
    {
        internal virtual T Head
        {
            get { throw new Exception(); }
        }

        public abstract int Size { get; }

        public abstract T LookupTree(int i);

        public abstract Tree<T> UpdateTree(int i, T x);

        internal virtual Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>> Cons(List2<BinaryRandomAccessList<T>> list)
        {
            throw new Exception();
        }
    }
}