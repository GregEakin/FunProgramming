// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ParingHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    using FunProgLib.persistence;

    public static class ParingHeap<T> where T : IComparable
    {
        public class Heap
        {
            private readonly T root;

            private readonly List<Heap>.ListStructure list;

            public Heap(T root, List<Heap>.ListStructure list)
            {
                this.root = root;
                this.list = list;
            }

            public T Root
            {
                get { return root; }
            }

            public List<Heap>.ListStructure List
            {
                get { return list; }
            }
        }

        private static readonly List<Heap>.ListStructure EmptyList = null;

        private static readonly Heap EmptyHeap = null;

        public static Heap Empty
        {
            get { return EmptyHeap; }
        }

        public static bool IsEmapty(Heap list)
        {
            return list == EmptyHeap;
        }

        public static Heap Merge(Heap h1, Heap h2)
        {
            if (h2 == EmptyHeap) return h1;
            if (h1 == EmptyHeap) return h2;

            if (h1.Root.CompareTo(h2.Root) <= 0) return new Heap(h1.Root, List<Heap>.Cons(h1.List, h2));
            return new Heap(h2.Root, List<Heap>.Cons(h2.List, h1));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, EmptyList), h);
        }

        private static Heap MergePairs(List<Heap>.ListStructure hs)
        {
            if (hs == EmptyList) return EmptyHeap;
            if (hs.Next == EmptyList) return hs.Element;
            return Merge(Merge(hs.Element, hs.Next.Element), MergePairs(hs.Next.Element.List));
        }

        public static T FindMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return h.Root;
        }

        public static Heap DeleteMin(Heap h)
        {
            if (h == EmptyHeap) throw new Exception("Empty");
            return MergePairs(h.List);
        }
    }
}