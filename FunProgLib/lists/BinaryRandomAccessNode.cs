using System;

namespace FunProgLib.lists
{
    internal sealed class Node<T> : Tree<T>
    {
        private readonly int _index;

        private readonly Tree<T> _tree1;

        private readonly Tree<T> _tree2;

        public Node(int index, Tree<T> tree1, Tree<T> tree2)
        {
            _index = index;
            _tree1 = tree1;
            _tree2 = tree2;
        }

        public override int Size
        {
            get { return _index; }
        }

        public override T LookupTree(int i)
        {
            return i < _index / 2
                ? _tree1.LookupTree(i)
                : _tree2.LookupTree(i - _index / 2);
        }

        public override Tree<T> UpdateTree(int i, T x)
        {
            return i < _index / 2
                ? new Node<T>(_index, _tree1.UpdateTree(i, x), _tree2)
                : new Node<T>(_index, _tree1, _tree2.UpdateTree(i - _index / 2, x));
        }

        internal override Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>> Cons(List2<BinaryRandomAccessList<T>> list)
        {
            return new Tuple<Tree<T>, List2<BinaryRandomAccessList<T>>>(_tree1, list.Cons(new BinaryRandomAccessList<T>(_tree2)));
        }
    }
}