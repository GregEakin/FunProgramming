// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "5.4 Splay Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 46-52. Print.

namespace FunProgLib.heap
{
    using System;

    public static class SplayHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            public Heap(Heap a, T x, Heap b)
            {
                A = a;
                X = x;
                B = b;
            }

            public Heap A { get; }

            public T X { get; }

            public Heap B { get; }
        }

        public static Heap Empty { get; } = null;

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
}