// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.1.3 Bootstrapped Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 146-50. Print.

using FunProgLib.lists;

namespace FunProgLib.queue;

// assumes polymorphic recursion!
public static class BootstrappedQueue<T>
{
    public sealed class Queue
    {
        public Queue(int lenfm, FunList<T>.Node f, BootstrappedQueue<Lazy<FunList<T>.Node>>.Queue m, int lenr, FunList<T>.Node r)
        {
            LenFM = lenfm;
            F = f;
            M = m;
            LenR = lenr;
            R = r;
        }

        public int LenFM { get; }

        public FunList<T>.Node F { get; }

        public BootstrappedQueue<Lazy<FunList<T>.Node>>.Queue M { get; }

        public int LenR { get; }

        public FunList<T>.Node R { get; }
    }

    public static Queue Empty { get; } = null;

    public static bool IsEmpty(Queue queue) => queue == Empty;

    private static Queue CheckQ(int lenfm, FunList<T>.Node f, BootstrappedQueue<Lazy<FunList<T>.Node>>.Queue m, int lenr, FunList<T>.Node r)
    {
        if (lenr <= lenfm) return CheckF(lenfm, f, m, lenr, r);
        return CheckF(lenfm + lenr, f, BootstrappedQueue<Lazy<FunList<T>.Node>>.Snoc(m, new Lazy<FunList<T>.Node>(() => FunList<T>.Reverse(r))), 0, FunList<T>.Empty);
    }

    private static Queue CheckF(int lenfm, FunList<T>.Node f, BootstrappedQueue<Lazy<FunList<T>.Node>>.Queue m, int lenr, FunList<T>.Node r)
    {
        if (FunList<T>.IsEmpty(f) && m == null) return Empty;
        if (FunList<T>.IsEmpty(f)) return new Queue(lenfm, BootstrappedQueue<Lazy<FunList<T>.Node>>.Head(m).Value, BootstrappedQueue<Lazy<FunList<T>.Node>>.Tail(m), lenr, r);
        return new Queue(lenfm, f, m, lenr, r);
    }

    public static Queue Snoc(Queue queue, T item)
    {
        if (queue == Empty) return new Queue(1, new FunList<T>.Node(item, FunList<T>.Empty), null, 0, FunList<T>.Empty);
        return CheckQ(queue.LenFM, queue.F, queue.M, queue.LenR + 1, FunList<T>.Cons(item, queue.R));
    }

    public static T Head(Queue queue)
    {
        if (queue == Empty) throw new ArgumentNullException(nameof(queue));
        return FunList<T>.Head(queue.F);
    }

    public static Queue Tail(Queue queue)
    {
        if (queue == Empty) throw new ArgumentNullException(nameof(queue));
        return CheckQ(queue.LenFM - 1, FunList<T>.Tail(queue.F), queue.M, queue.LenR, queue.R);
    }
}