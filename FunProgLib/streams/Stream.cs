// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		Stream.cs
// AUTHOR:		Greg Eakin
namespace FunProgLib.streams
{
    using System;

    public static class Stream<T>
    {
        public sealed class StreamCell
        {
            private readonly T element;
            private readonly Lazy<StreamCell> next;

            public StreamCell(T element, Lazy<StreamCell> next)
            {
                this.element = element;
                this.next = next;
            }

            public T Element { get { return element; } }
            public Lazy<StreamCell> Next { get { return next; } }
        }

        public static Lazy<StreamCell> Cons(T element, Lazy<StreamCell> s)
        {
            return new Lazy<StreamCell>(() => new StreamCell(element, s));
        }

        public static Lazy<StreamCell> Append(Lazy<StreamCell> s1, Lazy<StreamCell> s2)
        {
            if (s1 == null || s1.Value == null) return s2;
            return new Lazy<StreamCell>(() => new StreamCell(s1.Value.Element, Append(s1.Value.Next, s2)));
        }

        public static Lazy<StreamCell> Take(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return null;
            if (s == null || s.Value == null) return null;
            return new Lazy<StreamCell>(() => new StreamCell(s.Value.Element, Take(n - 1, s.Value.Next)));
        }

        private static StreamCell DropPrime(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return s.Value;
            if (s == null || s.Value == null) return null;
            return DropPrime(n - 1, s.Value.Next);
        }

        public static Lazy<StreamCell> Drop(int n, Lazy<StreamCell> s)
        {
            return new Lazy<StreamCell>(() => DropPrime(n, s));
        }

        private static StreamCell ReversePrime(Lazy<StreamCell> s, Lazy<StreamCell> r)
        {
            if (s == null || s.Value == null) return r.Value;
            return ReversePrime(s.Value.Next, new Lazy<StreamCell>(() => new StreamCell(s.Value.Element, r)));
        }

        public static Lazy<StreamCell> Reverse(Lazy<StreamCell> s)
        {
            return new Lazy<StreamCell>(() => ReversePrime(s, null));
        }
    }
}