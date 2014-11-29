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
            private readonly Heap a;
            private readonly T x;
            private readonly Heap b;

            public Heap(Heap a, T x, Heap b)
            {
                this.a = a;
                this.x = x;
                this.b = b;
            }

            public Heap A
            {
                get { return this.a; }
            }

            public T X
            {
                get { return this.x; }
            }

            public Heap B
            {
                get { return this.b; }
            }
        }

        private sealed class Pair
        {
            private readonly Heap a;
            private readonly Heap b;

            public Pair(Heap a, Heap b)
            {
                this.a = a;
                this.b = b;
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

        public static bool IsEmpty(Heap h)
        {
            return h == EmptyHeap;
        }

        private static Pair Partition(T pivot, Heap t)
        {
            if (IsEmpty(t)) return new Pair(EmptyHeap, EmptyHeap);
            if (t.X.CompareTo(pivot) <= 0)
            {
                if (IsEmpty(t.B)) return new Pair(t, EmptyHeap);
                if (t.B.X.CompareTo(pivot) <= 0)
                {
                    var pair = Partition(pivot, t.B.B);
                    return new Pair(new Heap(new Heap(t.A, t.X, t.B.A), t.B.X, pair.A), pair.B);
                }
                else
                {
                    var pair = Partition(pivot, t.B.A);
                    return new Pair(new Heap(t.A, t.X, pair.A), new Heap(pair.B, t.B.X, t.B.B));
                }
            }
            else
            {
                if (IsEmpty(t.A)) return new Pair(EmptyHeap, t);
                if (t.A.X.CompareTo(pivot) <= 0)
                {
                    var pair = Partition(pivot, t.A.B);
                    return new Pair(new Heap(t.A.A, t.A.X, pair.A), new Heap(pair.B, t.X, t.B));
                }
                else
                {
                    var pair = Partition(pivot, t.A.A);
                    return new Pair(pair.A, new Heap(pair.B, t.A.X, new Heap(t.A.B, t.X, t.B)));
                }
            }
        }

        public static Heap Insert(T x, Heap t)
        {
            var pair = Partition(x, t);
            return new Heap(pair.A, x, pair.B);
        }

        public static Heap Merge(Heap s, Heap t)
        {
            if (IsEmpty(s)) return t;
            var pair = Partition(s.X, t);
            return new Heap(Merge(pair.A, s.A), s.X, Merge(pair.B, s.B));
        }

        public static T FindMin(Heap t)
        {
            if (IsEmpty(t)) throw new Exception("Empty");
            if (IsEmpty(t.A)) return t.X;
            return FindMin(t.A);
        }

        public static Heap DeleteMin(Heap t)
        {
            if (IsEmpty(t)) throw new Exception("Empty");
            if (IsEmpty(t.A)) return t.B;
            if (IsEmpty(t.A.A)) return new Heap(t.A.B, t.X, t.B);
            return new Heap(DeleteMin(t.A.A), t.A.X, new Heap(t.A.B, t.X, t.B));
        }
    }
}