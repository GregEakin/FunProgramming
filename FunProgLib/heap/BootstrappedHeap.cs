// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.2.2 Bootstrapped Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 158-62. Print.

using System;

namespace FunProgLib.heap
{
    public static class BootstrappedHeap<T> where T : IComparable<T>
    {
        public sealed class Heap
        {
            public Heap(T x, PrimH.Element p)
            {
                X = x;
                P = p;
            }

            public T X { get; }

            public PrimH.Element P { get; }
        }

        // recursive structures not supported in C#
        public static class PrimH
        {
            public sealed class Element
            {
                public Element(Heap h1, Element h2)
                {
                    H1 = h1;
                    H2 = h2;
                }

                public Heap H1 { get; }

                public Element H2 { get; }
            }

            public static Element Empty { get; } = null;

            public static bool IsEmpty(Element h)
            {
                return h == Empty;
            }

            public static Element Merge(Element h1, Element h2)
            {
                if (IsEmpty(h1)) return h2;
                if (IsEmpty(h2)) return h1;
                if (h1.H1.X.CompareTo(h2.H1.X) <= 0) return new Element(h1.H1, Insert(h2.H1, h1.H2));
                return new Element(h2.H1, Insert(h1.H1, h2.H2));
            }

            public static Element Insert(Heap e1, Element h2)
            {
                return Merge(new Element(e1, Empty), h2);
            }

            public static Heap FindMin(Element h)
            {
                if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
                return h.H1;
            }

            public static Element DeleteMin(Element h)
            {
                if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
                if (IsEmpty(h.H2)) return Empty;
                var p1 = FindMin(h.H2).P;
                var p2 = DeleteMin(h.H2);
                return new Element(h.H1, Merge(p1, p2));
            }
        }

        public static Heap Empty { get; } = null;

        public static bool IsEmpty(Heap heap)
        {
            return heap == Empty;
        }

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (IsEmpty(h1)) return h2;
            if (IsEmpty(h2)) return h1;
            if (h1.X.CompareTo(h2.X) <= 0) return new Heap(h1.X, PrimH.Insert(h2, h1.P));
            return new Heap(h2.X, PrimH.Insert(h1, h2.P));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, PrimH.Empty), h);
        }

        public static T FindMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
            return h.X;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (IsEmpty(h)) throw new ArgumentException("Empty", nameof(h));
            if (PrimH.IsEmpty(h.P)) return Empty;
            var p1 = PrimH.FindMin(h.P);
            var p2 = PrimH.DeleteMin(h.P);
            return new Heap(p1.X, PrimH.Merge(p1.P, p2));
        }
    }
}