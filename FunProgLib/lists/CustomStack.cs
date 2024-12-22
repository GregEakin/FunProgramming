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

public static class CustomStack<T> // : IStack<T>
{
    public sealed class Node // : IEnumerable<T>
    {
        public Node(T element, Node next)
        {
            Element = element;
            Next = next;
        }

        public T Element { get; }

        public Node Next { get; }

        //public IEnumerator<T> GetEnumerator() => new StackEnum(this);

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //private sealed class StackEnum : IEnumerator<T>
        //{
        //    private readonly Node start;
        //    private Node stack;

        //    public StackEnum(Node stack)
        //    {
        //        this.start = new Node(default(T), stack);
        //        this.stack = this.start;
        //    }

        //    public bool MoveNext()
        //    {
        //        if (IsEmpty(this.stack)) return false;
        //        this.stack = this.stack.Next;
        //        return !IsEmpty(this.stack);
        //    }

        //    public void Reset() => this.stack = this.start;

        //    object IEnumerator.Current => stack.element;

        //    public T Current => stack.element;

        //    public void Dispose()
        //    {
        //    }
        //}
    }

    public static Node Empty => null;

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

    //public static Node Cat(Node list1, Node list2)
    //{
    //    if (IsEmpty(list1)) return list2;
    //    if (IsEmpty(list2)) return list1;
    //    return new Node(list1.Element, Cat(list1.Next, list2));
    //}

    //public static Node Reverse(Node list)
    //{
    //    if (IsEmpty(list)) return Empty;
    //    if (IsEmpty(list.Next)) return list;
    //    return Rev(list, Empty);
    //}

    //private static Node Rev(Node listIn, Node listOut)
    //{
    //    if (IsEmpty(listIn)) return listOut;
    //    var next = new Node(Head(listIn), listOut);
    //    return Rev(Tail(listIn), next);
    //}
}