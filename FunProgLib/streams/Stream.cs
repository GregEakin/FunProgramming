﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "4.2 Streams." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 34-37. Print.

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

        private static readonly Lazy<StreamCell> NilStreamCell = new Lazy<StreamCell>(() => null);

        public static Lazy<StreamCell> DollarNil { get { return NilStreamCell; } }

        public static Lazy<StreamCell> DollarCons(T element, Lazy<StreamCell> s)
        {
            return new Lazy<StreamCell>(() => new StreamCell(element, s));
        }

        public static Lazy<StreamCell> Append(Lazy<StreamCell> s1, Lazy<StreamCell> t)
        {
            if (s1 == null || s1 == DollarNil) return t;
            if (s1.Value == null) throw new Exception("Found it");
            return DollarCons(s1.Value.Element, Append(s1.Value.Next, t));
        }

        public static Lazy<StreamCell> Take(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return DollarNil;
            if (s == DollarNil) return DollarNil;
            if (s.Value == null) throw new Exception("Found it");
            return DollarCons(s.Value.Element, Take(n - 1, s.Value.Next));
        }

        private static Lazy<StreamCell> DropPrime(int n, Lazy<StreamCell> s)
        {
            if (n == 0) return s;
            if (s == DollarNil) return DollarNil;
            if (s.Value == null) throw new Exception("Found it");
            return DropPrime(n - 1, s.Value.Next);
        }

        public static Lazy<StreamCell> Drop(int n, Lazy<StreamCell> s)
        {
            return DropPrime(n, s);
        }

        private static Lazy<StreamCell> ReversePrime(Lazy<StreamCell> s, Lazy<StreamCell> r)
        {
            if (s == null || s == DollarNil) return r;
            if (s.Value == null) throw new Exception("Found it");
            return ReversePrime(s.Value.Next, DollarCons(s.Value.Element, r));
        }

        public static Lazy<StreamCell> Reverse(Lazy<StreamCell> s)
        {
            return ReversePrime(s, DollarNil);
        }
    }
}
