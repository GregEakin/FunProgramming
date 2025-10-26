// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Okasaki, Chris. "2.1 Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 7-11. Print.

namespace FunProgLib.lists;

public static class FunList<T> // : IStack<T>
{
    public sealed record Node(T Element, Node Next) : IEnumerable<T>
    {
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

    public static Node Empty => null;

    public static bool IsEmpty(Node list) => list == Empty;

    public static Node Cons(T element, Node list) => new(element, list);

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
        return list1 with { Next = Cat(list1.Next, list2) };
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
        return f(xs.Element, FoldRight(xs.Next, z, f));
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
        return FoldLeft(xs.Next, f(z, xs.Element), f);
    }

    public static TB FoldRightL<TB>(Node xs, TB z, Func<T, TB, TB> f) => 
        FoldLeft(xs, new Func<TB, TB>(b => b), (g, a) => b => g(f(a, b)))(z);
}