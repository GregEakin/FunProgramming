using System;

namespace FunProgLib.lists
{
    internal sealed class Leaf<T> : Tree<T>
    {
        private readonly T _alpha;

        public Leaf(T alpha)
        {
            _alpha = alpha;
        }

        internal override T Head
        {
            get { return _alpha; }
        }

        public override int Size
        {
            get { return 1; }
        }

        public override T LookupTree(int i)
        {
            if (i == 0) return _alpha;
            throw new Exception("Subscript");
        }

        public override Tree<T> UpdateTree(int i, T x)
        {
            if (i == 0) return new Leaf<T>(x);
            throw new Exception("Subscript");
        }
    }
}