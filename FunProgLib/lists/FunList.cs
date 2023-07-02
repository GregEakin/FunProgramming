// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

namespace FunProgLib.lists;

public static class FunList<T> // : IStack<T>
{
    public sealed class Node : IEnumerable<T>
    {
        public Node(T element, Node next)
        {
            Element = element;
            Next = next;
        }

        public T Element { get; }

        public Node Next { get; }

        public IEnumerator<T> GetEnumerator() => new ListEnum(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private sealed class ListEnum : IEnumerator<T>
        {
            private readonly Node _start;
            private Node _list;

            public ListEnum(Node list)
            {
                _start = new Node(default(T), list);
                _list = _start;
            }

            public bool MoveNext()
            {
                if (IsEmpty(_list)) return false;
                _list = _list.Next;
                return !IsEmpty(_list);
            }

            public void Reset() => _list = _start;

            object IEnumerator.Current => _list.Element;

            public T Current => _list.Element;

            public void Dispose()
            {
            }
        }
    }

    public static Node Empty { get; } = null;

    public static bool IsEmpty(Node list) => list == Empty;

    public static Node Cons(T element, Node list) => new Node(element, list);

    public static T Head(Node list)
    {
        if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        return list.Element;
    }

    public static Node Tail(Node list)
    {
        if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        return list.Next;
    }

    public static Node Cat(Node list1, Node list2)
    {
        if (IsEmpty(list1)) return list2;
        if (IsEmpty(list2)) return list1;
        return new Node(list1.Element, Cat(list1.Next, list2));
    }

    public static Node Reverse(Node list)
    {
        if (IsEmpty(list)) return Empty;
        if (IsEmpty(list.Next)) return list;
        return Rev(list, Empty);
    }

    private static Node Rev(Node listIn, Node listOut)
    {
        if (IsEmpty(listIn)) return listOut;
        var next = new Node(Head(listIn), listOut);
        return Rev(Tail(listIn), next);
    }

    public static TB FoldRight<TB>(Node xs, TB z, Func<T, TB, TB> f)
    {
        if (IsEmpty(xs)) return z;
        return f(xs.Element, FoldRight<TB>(xs.Next, z, f));
    }

    public static TB FoldLeftR<TB>(Node xs, TB z, Func<TB, T, TB> f)
    {
        var identity = new Func<TB, TB>(b => b);
        var combinerDelayer =
            new Func<T, Func<TB, TB>, Func<TB, TB>>((a, delayedExec) => b => delayedExec(f(b, a)));
        var chain = FoldRight(xs, identity, combinerDelayer);
        return chain(z);
    }

    public static TB FoldLeft<TB>(Node xs, TB z, Func<TB, T, TB> f)
    {
        // while (true)
        // {
        //     if (IsEmpty(xs)) return z;
        //     var xs1 = xs;
        //     xs = xs.Next;
        //     z = f(z, xs1.Element);
        // }

        if (IsEmpty(xs)) return z;
        return FoldLeft<TB>(xs.Next, f(z, xs.Element), f);
    }

    public static TB FoldRightL<TB>(Node xs, TB z, Func<T, TB, TB> f) => 
        FoldLeft(xs, new Func<TB, TB>(b => b), (g, a) => b => g(f(a, b)))(z);
}