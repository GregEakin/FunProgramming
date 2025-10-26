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
// Okasaki, Chris. "3.3 Red-Black Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 24-29. Print.

namespace FunProgLib.tree;

public static class RedBlackSet<T> where T : IComparable<T>
{
    public enum Color { R, B };

    public sealed record Tree(Color Color, Tree Tree1, T Elem, Tree Tree2);

    public static Tree EmptyTree => null;

    public static bool Member(T x, Tree t)
    {
        if (t == EmptyTree) return false;
        if (x.CompareTo(t.Elem) < 0) return Member(x, t.Tree1);
        if (t.Elem.CompareTo(x) < 0) return Member(x, t.Tree2);
        return true;
    }

    private static Tree Balance(Color color, Tree tree1, T x, Tree tree2)
    {
        if (color == Color.B)
        {
            if (tree1 != EmptyTree && tree1.Color == Color.R)
            {
                if (tree1.Tree1 != EmptyTree && tree1.Tree1.Color == Color.R)
                    return new Tree(Color.R, new Tree(Color.B, tree1.Tree1.Tree1, tree1.Tree1.Elem, tree1.Tree1.Tree2), tree1.Elem, new Tree(Color.B, tree1.Tree2, x, tree2));

                if (tree1.Tree2 != EmptyTree && tree1.Tree2.Color == Color.R)
                    return new Tree(Color.R, new Tree(Color.B, tree1.Tree1, tree1.Elem, tree1.Tree2.Tree1), tree1.Tree2.Elem, new Tree(Color.B, tree1.Tree2.Tree2, x, tree2));
            }

            if (tree2 != EmptyTree && tree2.Color == Color.R)
            {
                if (tree2.Tree1 != EmptyTree && tree2.Tree1.Color == Color.R)
                    return new Tree(Color.R, new Tree(Color.B, tree1, x, tree2.Tree1.Tree1), tree2.Tree1.Elem, new Tree(Color.B, tree2.Tree1.Tree2, tree2.Elem, tree2.Tree2));

                if (tree2.Tree2 != EmptyTree && tree2.Tree2.Color == Color.R)
                    return new Tree(Color.R, new Tree(Color.B, tree1, x, tree2.Tree1), tree2.Elem, new Tree(Color.B, tree2.Tree2.Tree2, tree2.Tree2.Elem, tree2.Tree2.Tree2));
            }
        }

        return new Tree(color, tree1, x, tree2);
    }

    public static Tree Insert(T x, Tree s)
    {
        var val = Ins(x, s);
        return new Tree(Color.B, val.Tree1, val.Elem, val.Tree2);
    }

    private static Tree Ins(T x, Tree s)
    {
        if (s == EmptyTree) return new Tree(Color.R, EmptyTree, x, EmptyTree);
        if (x.CompareTo(s.Elem) < 0) return Balance(s.Color, Ins(x, s.Tree1), s.Elem, s.Tree2);
        if (s.Elem.CompareTo(x) < 0) return Balance(s.Color, s.Tree1, s.Elem, Ins(x, s.Tree2));
        return s;
    }
}