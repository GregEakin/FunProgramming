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
// Okasaki, Chris. "6.4.2 Example: Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 72-74. Print.

using FunProgLib.lists;

namespace FunProgLib.queue;

public static class PhysicistsQueue<T>
{
    public sealed record Queue(FunList<T>.Node W, int LenF, Lazy<FunList<T>.Node> F, int LenR, FunList<T>.Node R);

    public static Queue Empty { get; } = new(FunList<T>.Empty, 0, new Lazy<FunList<T>.Node>(() => FunList<T>.Empty), 0, FunList<T>.Empty);

    public static bool IsEmpty(Queue queue) => queue.LenF == 0;

    private static Queue CheckW(FunList<T>.Node w, int LenF, Lazy<FunList<T>.Node> f, int LenR, FunList<T>.Node r)
    {
        if (FunList<T>.IsEmpty(w)) return new Queue(f.Value, LenF, f, LenR, r);
        return new Queue(w, LenF, f, LenR, r);
    }

    private static Queue Check(FunList<T>.Node w, int LenF, Lazy<FunList<T>.Node> f, int LenR, FunList<T>.Node r)
    {
        if (LenR <= LenF) return CheckW(w, LenF, f, LenR, r);
        return CheckW(f.Value, LenF + LenR, new Lazy<FunList<T>.Node>(() => FunList<T>.Cat(f.Value, FunList<T>.Reverse(r))), 0, FunList<T>.Empty);
    }

    public static Queue Snoc(Queue queue, T element) => 
        Check(queue.W, queue.LenF, queue.F, queue.LenR + 1, FunList<T>.Cons(element, queue.R));

    public static T Head(Queue queue)
    {
        if (FunList<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
        return queue.W.Element;
    }

    public static Queue Tail(Queue queue)
    {
        if (FunList<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
        return Check(queue.W.Next, queue.LenF - 1, new Lazy<FunList<T>.Node>(() => queue.F.Value.Next), queue.LenR, queue.R);
    }
}