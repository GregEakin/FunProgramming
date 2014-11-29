// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "3.1 Leftist Heaps." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 17-20. Print.

namespace FunProgLib.heap
{
    using System;

    public static class LeftistHeap<T> where T : IComparable
    {
        public sealed class Heap
        {
            private readonly int r;
            private readonly T x;
            private readonly Heap a;
            private readonly Heap b;

            public Heap(int r, T x, Heap a, Heap b)
            {
                this.r = r;
                this.x = x;
                this.a = a;
                this.b = b;
            }

            public int R
            {
                get { return this.r; }
            }

            public T X
            {
                get { return this.x; }
            }

            public Heap A
            {
                get { return this.a; }
            }

            public Heap B
            {
                get { return this.b; }
            }
        }

        private static readonly Heap EmptyHeap = null;

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

        private static int Rank(Heap h)
        {
            if (h == EmptyHeap) return 0;
            return h.R;
        }

        private static Heap MakeT(T x, Heap a, Heap b)
        {
            if (Rank(a) >= Rank(b)) return new Heap(Rank(b) + 1, x, a, b);
            return new Heap(Rank(a) + 1, x, b, a);
        }

        public static bool IsEmpty(Heap h)
        {
            return h == EmptyHeap;
        }

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (h2 == EmptyHeap) return h1;
            if (h1 == EmptyHeap) return h2;
            if (h1.X.CompareTo(h2.X) <= 0) return MakeT(h1.X, h1.A, Merge(h1.B, h2));
            return MakeT(h2.X, h2.A, Merge(h1, h2.B));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(1, x, EmptyHeap, EmptyHeap), h);
        }

        public static T FindMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return h.X;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return Merge(h.A, h.B);
        }
    }
}