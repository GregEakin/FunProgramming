// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		ParingHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.heap
{
    using System;

    using FunProgLib.persistence;

    public static class ParingHeap<T> where T : IComparable
    {
        public class Heap
        {
            private readonly T root;

            private readonly LinkList<Heap>.List list;

            public Heap(T root, LinkList<Heap>.List list)
            {
                this.root = root;
                this.list = list;
            }

            public T Root
            {
                get { return root; }
            }

            public LinkList<Heap>.List List
            {
                get { return list; }
            }
        }

        private static readonly LinkList<Heap>.List EmptyList = null;

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

            if (h1.Root.CompareTo(h2.Root) <= 0) return new Heap(h1.Root, LinkList<Heap>.Cons(h2, h1.List));
            return new Heap(h2.Root, LinkList<Heap>.Cons(h1, h2.List));
        }

        public static Heap Insert(T x, Heap h)
        {
            return Merge(new Heap(x, EmptyList), h);
        }

        private static Heap MergePairs(LinkList<Heap>.List hs)
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