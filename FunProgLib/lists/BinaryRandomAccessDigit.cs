using System;

namespace FunProgLib.lists
{
    public class BinaryRandomAccessDigit<T>
    {
        // private readonly Tree<T> _one;

        //private BinaryRandomAccessDigit(Tree<T> tree)
        //{
        //    _one = tree;
        //}

        //internal Tree<T> One
        //{
        //    get { return _one; }
        //}

        //private Tree<T> Link(Tree<T> t1)
        //{
        //    return new Node<T>(t1.Size, t1, null);
        //}

        internal virtual List2<BinaryRandomAccessList<T>> ConsTree(Tree<T> t)
        {
            return List2<BinaryRandomAccessDigit<T>>.Empty.Cons(new BinaryRandomAccessOne<T>(t));
        }

        internal virtual Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>> UnconsTree
        {
            get { throw new Exception("Empty"); }
        }

        public List2<BinaryRandomAccessList<T>> Cons(T x)
        {
            return ConsTree(new Leaf<T>(x));
        }

        public virtual T Head
        {
            get { return UnconsTree.Item1.Head; }
        }

        public List2<BinaryRandomAccessList<T>> Tail
        {
            get { return UnconsTree.Item2; }
        }

        public virtual T Lookup(int i)
        {
            throw new Exception("Subscript");
        }

        public virtual BinaryRandomAccessList<T> Update(int i, T x)
        {
            throw new Exception("Subscript");
        }
    }
}