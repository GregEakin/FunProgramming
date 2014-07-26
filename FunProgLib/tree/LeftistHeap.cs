// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming, page 20
// FILE:		LeftistHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;

    public static class LeftistHeap
    {
        public class Node
        {
            private readonly int rank;

            private readonly int min;

            private readonly Node heap1;

            private readonly Node heap2;

            public Node(int rank, int min, Node heap1, Node heap2)
            {
                this.rank = rank;
                this.min = min;
                this.heap1 = heap1;
                this.heap2 = heap2;
            }

            public int Rank
            {
                get { return this.rank; }
            }

            public int Min
            {
                get { return this.min; }
            }

            public Node Heap1
            {
                get { return this.heap1; }
            }

            public Node Heap2
            {
                get { return this.heap2; }
            }
        }

        private static readonly Node EmptyTree = new Node(0, 0, EmptyTree, EmptyTree);

        public static Node Empty
        {
            get { return EmptyTree; }
        }

        public static int Rank(Node node)
        {
            if (node == EmptyTree) return 0;
            return node.Rank;
        }

        private static Node MakeT(int x, Node a, Node b)
        {
            if (a.Rank >= b.Rank) return new Node(b.Rank + 1, x, a, b);
            return new Node(a.Rank + 1, x, b, a);
        }

        public static bool IsEmpty(Node node)
        {
            return node == EmptyTree;
        }

        private static Node Merge(Node heap1, Node heap2)
        {
            if (heap2 == EmptyTree) return heap1;
            if (heap1 == EmptyTree) return heap2;
            if (heap1.Min <= heap2.Min) return MakeT(heap1.Min, heap1.Heap1, Merge(heap1.Heap2, heap2));
            return MakeT(heap2.Min, heap2.Heap1, Merge(heap1, heap2.Heap2));
        }

        public static Node Insert(Node node, int x)
        {
            return Merge(new Node(1, x, EmptyTree, EmptyTree), node);
        }

        public static int FindMin(Node node)
        {
            if (node == EmptyTree) throw new Exception("Empty");
            return node.Min;
        }

        public static Node DeleteMin(Node node)
        {
            if (node == EmptyTree) throw new Exception("Empty");
            return Merge(node.Heap1, node.Heap2);
        }
    }
}