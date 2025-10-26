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
// Okasaki, Chris. "5.4 Splay Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 46-52. Print.

namespace FunProgLib.heap;

public static class SplayHeap<T> where T : IComparable<T>
{
    public sealed record Heap(Heap A, T X, Heap B);

    public static Heap Empty => null;

    public static bool IsEmpty(Heap h) => h == Empty;

    private static (Heap, Heap) Partition(T pivot, Heap t)
    {
        if (IsEmpty(t)) return (Empty, Empty);
        if (t.X.CompareTo(pivot) <= 0)
        {
            if (IsEmpty(t.B)) return (t, Empty);
            if (t.B.X.CompareTo(pivot) <= 0)
            {
                var (small, big) = Partition(pivot, t.B.B);
                return (new Heap(new Heap(t.A, t.X, t.B.A), t.B.X, small), big);
            }
            else
            {
                var (small, big) = Partition(pivot, t.B.A);
                return (new Heap(t.A, t.X, small), new Heap(big, t.B.X, t.B.B));
            }
        }
        else
        {
            if (IsEmpty(t.A)) return (Empty, t);
            if (t.A.X.CompareTo(pivot) <= 0)
            {
                var (small, big) = Partition(pivot, t.A.B);
                return (new Heap(t.A.A, t.A.X, small), new Heap(big, t.X, t.B));
            }
            else
            {
                var (small, big) = Partition(pivot, t.A.A);
                return (small, new Heap(big, t.A.X, new Heap(t.A.B, t.X, t.B)));
            }
        }
    }

    public static Heap Insert(T x, Heap t)
    {
        var (a, b) = Partition(x, t);
        return new Heap(a, x, b);
    }

    public static Heap Merge(Heap s, Heap t)
    {
        if (IsEmpty(s)) return t;
        var (ta, tb) = Partition(s.X, t);
        return new Heap(Merge(ta, s.A), s.X, Merge(tb, s.B));
    }

    public static T FindMin(Heap t)
    {
        if (IsEmpty(t)) throw new ArgumentNullException(nameof(t));
        if (IsEmpty(t.A)) return t.X;
        return FindMin(t.A);
    }

    public static Heap DeleteMin(Heap t)
    {
        if (IsEmpty(t)) throw new ArgumentNullException(nameof(t));
        if (IsEmpty(t.A)) return t.B;
        if (IsEmpty(t.A.A)) return new Heap(t.A.B, t.X, t.B);
        return new Heap(DeleteMin(t.A.A), t.A.X, new Heap(t.A.B, t.X, t.B));
    }
}