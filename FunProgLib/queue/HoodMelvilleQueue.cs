// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.2.1 Example: Hood-Melville Real-Time Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 102-105. Print.

using FunProgLib.lists;

namespace FunProgLib.queue;

public static class HoodMelvilleQueue<T>
{
    public abstract class RotationState
    { }

    private sealed class Idle : RotationState
    { }

    private sealed class Reversing : RotationState
    {
        public Reversing(int ok, FunList<T>.Node f, FunList<T>.Node fp, FunList<T>.Node r, FunList<T>.Node rp)
        {
            Ok = ok;
            F = f;
            Fp = fp;
            R = r;
            Rp = rp;
        }

        public int Ok { get; }
        public FunList<T>.Node F { get; }
        public FunList<T>.Node Fp { get; }
        public FunList<T>.Node R { get; }
        public FunList<T>.Node Rp { get; }
    }

    private sealed class Appending : RotationState
    {
        public Appending(int ok, FunList<T>.Node fp, FunList<T>.Node rp)
        {
            Ok = ok;
            Fp = fp;
            Rp = rp;
        }

        public int Ok { get; }
        public FunList<T>.Node Fp { get; }
        public FunList<T>.Node Rp { get; }
    }

    private sealed class Done : RotationState
    {
        public Done(FunList<T>.Node f)
        {
            F = f;
        }

        public FunList<T>.Node F { get; }
    }

    public sealed class Queue
    {
        public Queue(int lenF, FunList<T>.Node f, RotationState state, int lenR, FunList<T>.Node r)
        {
            LenF = lenF;
            F = f;
            State = state;
            LenR = lenR;
            R = r;
        }

        public int LenF { get; }
        public FunList<T>.Node F { get; }
        public RotationState State { get; }
        public int LenR { get; }
        public FunList<T>.Node R { get; }
    }

    private static RotationState Exec(RotationState state)
    {
        if (state is Reversing reversing)
        {
            if (!FunList<T>.IsEmpty(reversing.F)) // && !FunList<T>.IsEmpty(reversing.R))
            {
                var x = FunList<T>.Head(reversing.F);
                var f = FunList<T>.Tail(reversing.F);
                var y = FunList<T>.Head(reversing.R);
                var r = FunList<T>.Tail(reversing.R);
                return new Reversing(reversing.Ok + 1, f, FunList<T>.Cons(x, reversing.Fp), r, FunList<T>.Cons(y, reversing.Rp));
            }

            var y2 = FunList<T>.Head(reversing.R);
            return new Appending(reversing.Ok, reversing.Fp, FunList<T>.Cons(y2, reversing.Rp));
        }

        if (state is Appending appending)
        {
            if (appending.Ok == 0)
                return new Done(appending.Rp);

            var x = FunList<T>.Head(appending.Fp);
            var fp = FunList<T>.Tail(appending.Fp);
            return new Appending(appending.Ok - 1, fp, FunList<T>.Cons(x, appending.Rp));
        }

        return state;
    }

    private static RotationState Invalidate(RotationState state)
    {
        if (state is Reversing reversing)
        {
            return new Reversing(reversing.Ok - 1, reversing.F, reversing.Fp, reversing.R, reversing.Rp);
        }

        if (state is Appending appending)
        {
            if (appending.Ok == 0) return new Done(FunList<T>.Tail(appending.Rp));
            return new Appending(appending.Ok - 1, appending.Fp, appending.Rp);
        }

        return state;
    }

    private static Queue Exec2(int lenF, FunList<T>.Node f, RotationState state, int lenR, FunList<T>.Node r)
    {
        var newState = Exec(Exec(state));
        if (newState is Done done)
            return new Queue(lenF, done.F, new Idle(), lenR, r);
        return new Queue(lenF, f, newState, lenR, r);
    }

    private static Queue Check(int lenF, FunList<T>.Node f, RotationState state, int lenR, FunList<T>.Node r)
    {
        if (lenR <= lenF) return Exec2(lenF, f, state, lenR, r);
        var newState = new Reversing(0, f, FunList<T>.Empty, r, FunList<T>.Empty);
        return Exec2(lenF + lenR, f, newState, 0, FunList<T>.Empty);
    }

    public static Queue Empty { get; } = new Queue(0, FunList<T>.Empty, new Idle(), 0, FunList<T>.Empty);

    public static bool IsEmpty(Queue queue) => queue.LenF == 0;

    public static Queue Snoc(Queue q, T x) => Check(q.LenF, q.F, q.State, q.LenR + 1, FunList<T>.Cons(x, q.R));

    public static T Head(Queue q)
    {
        if (IsEmpty(q)) throw new ArgumentNullException(nameof(q));
        var x = FunList<T>.Head(q.F);
        return x;
    }

    public static Queue Tail(Queue q)
    {
        if (FunList<T>.IsEmpty(q.F)) throw new ArgumentNullException(nameof(q));
        var f = FunList<T>.Tail(q.F);
        return Check(q.LenF - 1, f, Invalidate(q.State), q.LenR, q.R);
    }
}