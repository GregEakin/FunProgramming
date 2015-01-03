// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "4.2 Streams." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 34-37. Print.

using System.Threading.Tasks;

namespace FunProgLib.streams
{
    using System;

    public static class Stream<T>
    {
        public sealed class StreamCell
        {
            private readonly T element;
            private readonly Lazy<Task<StreamCell>> next;

            public StreamCell(T element, Lazy<Task<StreamCell>> next)
            {
                if (next == null) throw new Exception("Can't be null, use Stream<T>.DollarNil instead.");
                this.element = element;
                this.next = next;
            }

            public T Element { get { return element; } }
            public Lazy<Task<StreamCell>> Next { get { return next; } }
        }

        private static readonly Lazy<Task<StreamCell>> NilStreamCell = new Lazy<Task<StreamCell>>(() => null);

        public static Lazy<Task<StreamCell>> DollarNil { get { return NilStreamCell; } }

        public static Lazy<Task<StreamCell>> Append(Lazy<Task<StreamCell>> s1, Lazy<Task<StreamCell>> t)
        {
            if (s1 == DollarNil) return t;
            return new Lazy<Task<StreamCell>>(() => Task.Factory.StartNew(() => new StreamCell(s1.Value.Result.Element, Append(s1.Value.Result.Next, t))));
        }

        public static Lazy<Task<StreamCell>> Take(int n, Lazy<Task<StreamCell>> s)
        {
            if (n == 0) return DollarNil;
            if (s == DollarNil) return DollarNil;
            return new Lazy<Task<StreamCell>>(() => Task.Factory.StartNew(() => new StreamCell(s.Value.Result.Element, Take(n - 1, s.Value.Result.Next))));
        }

        private static Lazy<Task<StreamCell>> DropPrime(int n, Lazy<Task<StreamCell>> s)
        {
            if (n == 0) return s;
            if (s == DollarNil) return DollarNil;
            return DropPrime(n - 1, s.Value.Result.Next);
        }

        public static Lazy<Task<StreamCell>> Drop(int n, Lazy<Task<StreamCell>> s)
        {
            return DropPrime(n, s);
        }

        private static Lazy<Task<StreamCell>> ReversePrime(Lazy<Task<StreamCell>> s, Lazy<Task<StreamCell>> r)
        {
            if (s == DollarNil) return r;
            return ReversePrime(s.Value.Result.Next, new Lazy<Task<StreamCell>>(() => Task.Factory.StartNew(() => new StreamCell(s.Value.Result.Element, r))));
        }

        public static Lazy<Task<StreamCell>> Reverse(Lazy<Task<StreamCell>> s)
        {
            return ReversePrime(s, DollarNil);
        }
    }
}
