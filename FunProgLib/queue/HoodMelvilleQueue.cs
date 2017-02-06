// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "8.2.1 Example: Hood-Melville Real-Time Queues." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 102-105. Print.

namespace FunProgLib.queue
{
    using lists;
    using System;

    public static class HoodMelvilleQueue<T>
    {
        public abstract class RotationState
        { }

        private sealed class Idle : RotationState
        { }

        private sealed class Reversing : RotationState
        {
            public Reversing(int ok, List<T>.Node f, List<T>.Node fp, List<T>.Node r, List<T>.Node rp)
            {
                Ok = ok;
                F = f;
                Fp = fp;
                R = r;
                Rp = rp;
            }

            public int Ok { get; }
            public List<T>.Node F { get; }
            public List<T>.Node Fp { get; }
            public List<T>.Node R { get; }
            public List<T>.Node Rp { get; }
        }

        private sealed class Appending : RotationState
        {
            public Appending(int ok, List<T>.Node fp, List<T>.Node rp)
            {
                Ok = ok;
                Fp = fp;
                Rp = rp;
            }

            public int Ok { get; }
            public List<T>.Node Fp { get; }
            public List<T>.Node Rp { get; }
        }

        private sealed class Done : RotationState
        {
            public Done(List<T>.Node f)
            {
                F = f;
            }

            public List<T>.Node F { get; }
        }

        public sealed class Queue
        {
            public Queue(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
            {
                LenF = lenF;
                F = f;
                State = state;
                LenR = lenR;
                R = r;
            }

            public int LenF { get; }
            public List<T>.Node F { get; }
            public RotationState State { get; }
            public int LenR { get; }
            public List<T>.Node R { get; }
        }

        private static RotationState Exec(RotationState state)
        {
            if (state is Reversing reversing)
            {
                if (!List<T>.IsEmpty(reversing.F)) // && !List<T>.IsEmpty(reversing.R))
                {
                    var x = List<T>.Head(reversing.F);
                    var f = List<T>.Tail(reversing.F);
                    var y = List<T>.Head(reversing.R);
                    var r = List<T>.Tail(reversing.R);
                    return new Reversing(reversing.Ok + 1, f, List<T>.Cons(x, reversing.Fp), r, List<T>.Cons(y, reversing.Rp));
                }

                var y2 = List<T>.Head(reversing.R);
                return new Appending(reversing.Ok, reversing.Fp, List<T>.Cons(y2, reversing.Rp));
            }

            if (state is Appending appending)
            {
                if (appending.Ok == 0)
                    return new Done(appending.Rp);

                var x = List<T>.Head(appending.Fp);
                var fp = List<T>.Tail(appending.Fp);
                return new Appending(appending.Ok - 1, fp, List<T>.Cons(x, appending.Rp));
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
                if (appending.Ok == 0) return new Done(List<T>.Tail(appending.Rp));
                return new Appending(appending.Ok - 1, appending.Fp, appending.Rp);
            }

            return state;
        }

        private static Queue Exec2(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
        {
            var newState = Exec(Exec(state));
            if (newState is Done done)
                return new Queue(lenF, done.F, new Idle(), lenR, r);
            return new Queue(lenF, f, newState, lenR, r);
        }

        private static Queue Check(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
        {
            if (lenR <= lenF) return Exec2(lenF, f, state, lenR, r);
            var newState = new Reversing(0, f, List<T>.Empty, r, List<T>.Empty);
            return Exec2(lenF + lenR, f, newState, 0, List<T>.Empty);
        }

        public static Queue Empty { get; } = new Queue(0, List<T>.Empty, new Idle(), 0, List<T>.Empty);

        public static bool IsEmpty(Queue queue) => queue.LenF == 0;

        public static Queue Snoc(Queue q, T x) => Check(q.LenF, q.F, q.State, q.LenR + 1, List<T>.Cons(x, q.R));

        public static T Head(Queue q)
        {
            if (IsEmpty(q)) throw new ArgumentNullException(nameof(q));
            var x = List<T>.Head(q.F);
            return x;
        }

        public static Queue Tail(Queue q)
        {
            if (List<T>.IsEmpty(q.F)) throw new ArgumentNullException(nameof(q));
            var f = List<T>.Tail(q.F);
            return Check(q.LenF - 1, f, Invalidate(q.State), q.LenR, q.R);
        }
    }
}
