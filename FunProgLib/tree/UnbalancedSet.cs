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
// Okasaki, Chris. "2.2 Binary Search Trees." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 11-15. Print.

namespace FunProgLib.tree;

public static class UnbalancedSet<T> where T : IComparable<T> // ISet
{
    public sealed class Tree // : IOrdered<T>
    {
        public Tree(Tree a, T y, Tree b)
        {
            A = a;
            Y = y;
            B = b;
        }

        public Tree A { get; }

        public T Y { get; }

        public Tree B { get; }
    }

    // type Set = Tree

    public static Tree Empty => null;

    public static bool Member(T x, Tree s)
    {
        if (s == Empty) return false;
        if (x.CompareTo(s.Y) < 0) return Member(x, s.A);
        if (s.Y.CompareTo(x) < 0) return Member(x, s.B);
        return true;
    }

    public static Tree Insert(T x, Tree s)
    {
        if (s == Empty) return new Tree(Empty, x, Empty);
        if (x.CompareTo(s.Y) < 0) return new Tree(Insert(x, s.A), s.Y, s.B);
        if (s.Y.CompareTo(x) < 0) return new Tree(s.A, s.Y, Insert(x, s.B));
        return s;
    }
}