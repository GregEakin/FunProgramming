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
// Okasaki, Chris. "9.2.1 Binary Random-Access Lists." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 119-22. Print.

namespace FunProgLib.lists;

public static class BinaryRandomAccessList<T> // : IRandomAccessList<T>
{
    public abstract record Tree();

    private sealed record Leaf(T Alpha) : Tree;

    private sealed record Node(int Index, Tree Tree1, Tree Tree2) : Tree;

    public sealed record Digit(Tree One);

    private static readonly Digit Zero = new(null);

    private sealed record Stuff(Tree Tree, FunList<Digit>.Node FunList);

    public static FunList<Digit>.Node Empty => FunList<Digit>.Empty;

    public static bool IsEmpty(FunList<Digit>.Node list) => FunList<Digit>.IsEmpty(list);

    private static int Size(Tree tree)
    {
        if (tree is Node node) return node.Index;
        return 1;
    }

    private static Tree Link(Tree t1, Tree t2) => new Node(Size(t1) + Size(t2), t1, t2);

    private static FunList<Digit>.Node ConsTree(Tree t, FunList<Digit>.Node ts)
    {
        if (IsEmpty(ts)) return FunList<Digit>.Cons(new Digit(t), FunList<Digit>.Empty);
        if (ts.Element == Zero) return FunList<Digit>.Cons(new Digit(t), ts.Next);
        return FunList<Digit>.Cons(Zero, ConsTree(Link(t, ts.Element.One), ts.Next));
    }

    private static Stuff UnconsTree(FunList<Digit>.Node list)
    {
        if (IsEmpty(list)) throw new ArgumentNullException(nameof(list));
        if (list.Element == Zero)
        {
            var stuff = UnconsTree(list.Next);
            if (stuff.Tree is Node node) return new Stuff(node.Tree1, FunList<Digit>.Cons(new Digit(node.Tree2), stuff.FunList));
            throw new Exception();
        }
        if (IsEmpty(list.Next)) return new Stuff(list.Element.One, FunList<Digit>.Empty);
        return new Stuff(list.Element.One, FunList<Digit>.Cons(Zero, list.Next));
    }

    public static FunList<Digit>.Node Cons(T x, FunList<Digit>.Node ts) => ConsTree(new Leaf(x), ts);

    public static T Head(FunList<Digit>.Node ts)
    {
        if (UnconsTree(ts).Tree is Leaf leaf) return leaf.Alpha;
        throw new Exception();
    }

    public static FunList<Digit>.Node Tail(FunList<Digit>.Node ts) => UnconsTree(ts).FunList;

    private static T LookupTree(int i, Tree t)
    {
        if (t is Leaf leaf)
        {
            if (i == 0) return leaf.Alpha;
            throw new Exception("Subscript");
        }

        if (t is Node node)
        {
            if (i < node.Index / 2) return LookupTree(i, node.Tree1);
            return LookupTree(i - node.Index / 2, node.Tree2);
        }

        throw new ArgumentException("Argument t needs to be a Leaf or None.", nameof(t));
    }

    private static Tree UpdateTree(int i, T x, Tree t)
    {
        if (t is Leaf leaf)
        {
            if (i == 0) return new Leaf(x);
            throw new Exception("Subscript");
        }

        if (t is Node node)
        {
            if (i < node.Index / 2) return new Node(node.Index, UpdateTree(i, x, node.Tree1), node.Tree2);
            return new Node(node.Index, node.Tree1, UpdateTree(i - node.Index / 2, x, node.Tree2));
        }

        throw new ArgumentException("Argument t needs to be a Leaf or None.", nameof(t));
    }

    public static T Lookup(int i, FunList<Digit>.Node ts)
    {
        if (IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
        if (ts.Element == Zero) return Lookup(i, ts.Next);
        if (i < Size(ts.Element.One)) return LookupTree(i, ts.Element.One);
        return Lookup(i - Size(ts.Element.One), ts.Next);
    }

    public static FunList<Digit>.Node Update(int i, T x, FunList<Digit>.Node ts)
    {
        if (IsEmpty(ts)) throw new ArgumentException("Subscript", nameof(ts));
        if (ts.Element == Zero) return FunList<Digit>.Cons(Zero, Update(i, x, ts.Next));
        if (i < Size(ts.Element.One)) return FunList<Digit>.Cons(new Digit(UpdateTree(i, x, ts.Element.One)), ts.Next);
        return FunList<Digit>.Cons(new Digit(ts.Element.One), Update(i - Size(ts.Element.One), x, ts.Next));
    }
}