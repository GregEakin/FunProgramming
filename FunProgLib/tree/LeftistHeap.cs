// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming, page 20
// FILE:		LeftistHeap.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.tree
{
    using System;
    using System.Text;

    public class LeftistHeap
    {
        private static readonly LeftistHeap EmptyTree = new LeftistHeap(0, 0, EmptyTree, EmptyTree);

        public static LeftistHeap Empty
        {
            get
            {
                return EmptyTree;
            }
        }

        private readonly int rank;

        private readonly int min;

        private readonly LeftistHeap heap1;

        private readonly LeftistHeap heap2;

        private LeftistHeap(int rank, int min, LeftistHeap heap1, LeftistHeap heap2)
        {
            this.rank = rank;
            this.min = min;
            this.heap1 = heap1;
            this.heap2 = heap2;
        }

        public int Rank()
        {
            if (this == EmptyTree) return 0;
            return this.rank;
        }

        private static LeftistHeap MakeT(int x, LeftistHeap a, LeftistHeap b)
        {
            if (a.rank >= b.rank) return new LeftistHeap(b.rank + 1, x, a, b);
            return new LeftistHeap(a.rank + 1, x, b, a);
        }

        public bool IsEmapty
        {
            get
            {
                return this == EmptyTree;
            }
        }

        private LeftistHeap Merge(LeftistHeap heap)
        {
            if (heap == EmptyTree) return this;
            if (this == EmptyTree) return heap;
            if (this.min <= heap.min) return MakeT(this.min, this.heap1, this.heap2.Merge(heap));
            return MakeT(heap.min, heap.heap1, this.Merge(heap.heap2));
        }

        public LeftistHeap Insert(int x)
        {
            return new LeftistHeap(1, x, EmptyTree, EmptyTree).Merge(this);
        }

        public int FindMin()
        {
            if (this == EmptyTree)
                throw new Exception("Empty");

            return this.min;
        }

        public LeftistHeap DeleteMin()
        {
            if (this == EmptyTree)
                throw new Exception("Empty");

            return this.heap1.Merge(this.heap2);
        }

        public override string ToString()
        {
            return DumpTree(this);
        }

        private static string DumpTree(LeftistHeap tree)
        {
            if (tree == EmptyTree) return "\u2205";

            var results = new StringBuilder();

            if (tree.heap1 != EmptyTree)
            {
                results.Append(tree.heap1);
            }

            results.Append(tree.min);
            //results.Append(" [");
            //results.Append(tree.rank);
            //results.Append("]");
            results.Append(", ");

            if (tree.heap2 != EmptyTree)
            {
                results.Append(tree.heap2);
            }

            return results.ToString();
        }
    }
}