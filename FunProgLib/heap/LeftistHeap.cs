// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.1 Leftist Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 17-20. Print.

namespace FunProgLib.heap
{
    using System;

    public static class LeftistHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            public Heap(int r, T x, Heap a, Heap b)
            {
                R = r;
                X = x;
                A = a;
                B = b;
            }

            public int R { get; }

            public T X { get; }

            public Heap A { get; }

            public Heap B { get; }
        }

        private static int Rank(Heap h)
        {
            if (IsEmpty(h)) return 0;
            return h.R;
        }

        private static Heap MakeT(T x, Heap a, Heap b)
        {
            if (Rank(a) >= Rank(b)) return new Heap(Rank(b) + 1, x, a, b);
            return new Heap(Rank(a) + 1, x, b, a);
        }

        public static Heap Empty { get; } = null;

        public static bool IsEmpty(Heap h) => h == Empty;

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (IsEmpty(h2)) return h1;
            if (IsEmpty(h1)) return h2;
            if (h1.X.CompareTo(h2.X) <= 0) return MakeT(h1.X, h1.A, Merge(h1.B, h2));
            return MakeT(h2.X, h2.A, Merge(h1, h2.B));
        }

        public static Heap Insert(T x, Heap h) => Merge(new Heap(1, x, Empty, Empty), h);

        public static T FindMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
            return h.X;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentNullException(nameof(h));
            return Merge(h.A, h.B);
        }
    }
}
