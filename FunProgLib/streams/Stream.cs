// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Okasaki, Chris. "4.2 Streams." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 34-37. Print.

namespace FunProgLib.streams;

public static class Stream<T>
{
    public sealed class StreamCell
    {
        public StreamCell(T element, Lazy<StreamCell> next)
        {
            Element = element;
            Next = next ?? throw new ArgumentException("Can't be null, use Stream<T>.DollarNil instead.", nameof(next));
        }

        public T Element { get; }
        public Lazy<StreamCell> Next { get; }
    }

    // public static Lazy<StreamCell> DollarCons(T x, Lazy<StreamCell> r) => new Lazy<StreamCell>(() => new StreamCell(x, r));

    public static Lazy<StreamCell> DollarNil { get; } = new Lazy<StreamCell>(() => null);

    public static Lazy<StreamCell> Append(Lazy<StreamCell> s1, Lazy<StreamCell> t)
    {
        if (s1 == DollarNil) return t;
        return new Lazy<StreamCell>(() => new StreamCell(s1.Value.Element, Append(s1.Value.Next, t)));
    }

    public static Lazy<StreamCell> Take(int n, Lazy<StreamCell> s)
    {
        if (n == 0) return DollarNil;
        if (s == DollarNil) return DollarNil;
        return new Lazy<StreamCell>(() => new StreamCell(s.Value.Element, Take(n - 1, s.Value.Next)));
    }

    private static Lazy<StreamCell> DropPrime(int n, Lazy<StreamCell> s)
    {
        if (n == 0) return s;
        if (s == DollarNil) return DollarNil;
        return DropPrime(n - 1, s.Value.Next);
    }

    public static Lazy<StreamCell> Drop(int n, Lazy<StreamCell> s) => DropPrime(n, s);

    private static Lazy<StreamCell> ReversePrime(Lazy<StreamCell> s, Lazy<StreamCell> r)
    {
        if (s == DollarNil) return r;
        return ReversePrime(s.Value.Next, new Lazy<StreamCell>(() => new StreamCell(s.Value.Element, r)));
    }

    public static Lazy<StreamCell> Reverse(Lazy<StreamCell> s) => ReversePrime(s, DollarNil);
}