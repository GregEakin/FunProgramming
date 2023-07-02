// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "6.4.2 Example: Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 72-74. Print.

using FunProgLib.lists;

namespace FunProgLib.queue;

public static class PhysicistsQueue<T>
{
    public sealed class Queue
    {
        public Queue(FunList<T>.Node w, int lenf, Lazy<FunList<T>.Node> f, int lenr, FunList<T>.Node r)
        {
            W = w;
            Lenf = lenf;
            F = f;
            Lenr = lenr;
            R = r;
        }

        public FunList<T>.Node W { get; }
        public int Lenf { get; }
        public Lazy<FunList<T>.Node> F { get; }
        public int Lenr { get; }
        public FunList<T>.Node R { get; }
    }

    public static Queue Empty { get; } = new Queue(FunList<T>.Empty, 0, new Lazy<FunList<T>.Node>(() => FunList<T>.Empty), 0, FunList<T>.Empty);

    public static bool IsEmpty(Queue queue) => queue.Lenf == 0;

    private static Queue CheckW(FunList<T>.Node w, int lenf, Lazy<FunList<T>.Node> f, int lenr, FunList<T>.Node r)
    {
        if (FunList<T>.IsEmpty(w)) return new Queue(f.Value, lenf, f, lenr, r);
        return new Queue(w, lenf, f, lenr, r);
    }

    private static Queue Check(FunList<T>.Node w, int lenf, Lazy<FunList<T>.Node> f, int lenr, FunList<T>.Node r)
    {
        if (lenr <= lenf) return CheckW(w, lenf, f, lenr, r);
        return CheckW(f.Value, lenf + lenr, new Lazy<FunList<T>.Node>(() => FunList<T>.Cat(f.Value, FunList<T>.Reverse(r))), 0, FunList<T>.Empty);
    }

    public static Queue Snoc(Queue queue, T element) => 
        Check(queue.W, queue.Lenf, queue.F, queue.Lenr + 1, FunList<T>.Cons(element, queue.R));

    public static T Head(Queue queue)
    {
        if (FunList<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
        return queue.W.Element;
    }

    public static Queue Tail(Queue queue)
    {
        if (FunList<T>.IsEmpty(queue.W)) throw new ArgumentNullException(nameof(queue));
        return Check(queue.W.Next, queue.Lenf - 1, new Lazy<FunList<T>.Node>(() => queue.F.Value.Next), queue.Lenr, queue.R);
    }
}