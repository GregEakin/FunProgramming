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
// Okasaki, Chris. "5.2 Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 42-45. Print.

using FunProgLib.lists;

namespace FunProgLib.queue;

public static class BatchedQueue<T>
{
    public sealed class Queue
    {
        public Queue(FunList<T>.Node f, FunList<T>.Node r)
        {
            F = f;
            R = r;
        }

        public FunList<T>.Node F { get; }
        public FunList<T>.Node R { get; }
    }

    public static Queue Empty { get; } = new(FunList<T>.Empty, FunList<T>.Empty);

    public static bool IsEmpty(Queue q) => FunList<T>.IsEmpty(q.F);

    private static Queue CheckF(FunList<T>.Node f, FunList<T>.Node r)
    {
        if (FunList<T>.IsEmpty(f)) return new Queue(FunList<T>.Reverse(r), FunList<T>.Empty);
        return new Queue(f, r);
    }

    public static Queue Snoc(Queue q, T x) => CheckF(q.F, FunList<T>.Cons(x, q.R));

    public static T Head(Queue q)
    {
        if (FunList<T>.IsEmpty(q.F)) throw new ArgumentNullException(nameof(q));
        return q.F.Element;
    }

    public static Queue Tail(Queue q)
    {
        if (FunList<T>.IsEmpty(q.F)) throw new ArgumentNullException(nameof(q));
        return CheckF(q.F.Next, q.R);
    }
}