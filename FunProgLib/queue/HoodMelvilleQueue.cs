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

using System;
using FunProgLib.lists;

namespace FunProgLib.queue
{
    public static class HoodMelvilleQueue<T>
    {
        public abstract class RotationState
        { }

        private sealed class Idle : RotationState
        { }

        private sealed class Reversing : RotationState
        {
            private readonly int ok;
            private readonly List<T>.Node f;
            private readonly List<T>.Node fp;
            private readonly List<T>.Node r;
            private readonly List<T>.Node rp;

            public Reversing(int ok, List<T>.Node f, List<T>.Node fp, List<T>.Node r, List<T>.Node rp)
            {
                this.ok = ok;
                this.f = f;
                this.fp = fp;
                this.r = r;
                this.rp = rp;
            }

            public int Ok { get { return ok; } }
            public List<T>.Node F { get { return f; } }
            public List<T>.Node Fp { get { return fp; } }
            public List<T>.Node R { get { return r; } }
            public List<T>.Node Rp { get { return rp; } }
        }

        private sealed class Appending : RotationState
        {
            private readonly int ok;
            private readonly List<T>.Node fp;
            private readonly List<T>.Node rp;

            public Appending(int ok, List<T>.Node fp, List<T>.Node rp)
            {
                this.ok = ok;
                this.fp = fp;
                this.rp = rp;
            }

            public int Ok { get { return ok; } }
            public List<T>.Node Fp { get { return fp; } }
            public List<T>.Node Rp { get { return rp; } }
        }

        private sealed class Done : RotationState
        {
            private readonly List<T>.Node f;

            public Done(List<T>.Node f)
            {
                this.f = f;
            }

            public List<T>.Node F { get { return f; } }
        }

        public sealed class Queue
        {
            private readonly int lenF;
            private readonly List<T>.Node f;
            private readonly RotationState state;
            private readonly int lenR;
            private readonly List<T>.Node r;

            public Queue(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
            {
                this.lenF = lenF;
                this.f = f;
                this.state = state;
                this.lenR = lenR;
                this.r = r;
            }

            public int LenF { get { return lenF; } }
            public List<T>.Node F { get { return f; } }
            public RotationState State { get { return state; } }
            public int LenR { get { return lenR; } }
            public List<T>.Node R { get { return r; } }
        }

        private static RotationState Exec(RotationState state)
        {
            var reversing = state as Reversing;
            if (reversing != null)
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

            var appending = state as Appending;
            if (appending != null)
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
            var reversing = state as Reversing;
            if (reversing != null)
            {
                return new Reversing(reversing.Ok - 1, reversing.F, reversing.Fp, reversing.R, reversing.Rp);
            }

            var appending = state as Appending;
            if (appending != null)
            {
                if (appending.Ok == 0) return new Done(List<T>.Tail(appending.Rp));
                return new Appending(appending.Ok - 1, appending.Fp, appending.Rp);
            }

            return state;
        }

        private static Queue Exec2(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
        {
            var newState = Exec(Exec(state));
            var done = newState as Done;
            if (done != null)
                return new Queue(lenF, done.F, new Idle(), lenR, r);
            return new Queue(lenF, f, newState, lenR, r);
        }

        private static Queue Check(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
        {
            if (lenR <= lenF) return Exec2(lenF, f, state, lenR, r);
            var newState = new Reversing(0, f, List<T>.Empty, r, List<T>.Empty);
            return Exec2(lenF + lenR, f, newState, 0, List<T>.Empty);
        }

        private static readonly Queue EmptyQueue = new Queue(0, List<T>.Empty, new Idle(), 0, List<T>.Empty);

        public static Queue Empty
        {
            get { return EmptyQueue; }
        }

        public static bool IsEmpty(Queue queue)
        {
            return queue.LenF == 0;
        }

        public static Queue Snoc(Queue q, T x)
        {
            return Check(q.LenF, q.F, q.State, q.LenR + 1, List<T>.Cons(x, q.R));
        }

        public static T Head(Queue q)
        {
            if (IsEmpty(q)) throw new Exception("Empty");
            var x = List<T>.Head(q.F);
            return x;
        }

        public static Queue Tail(Queue q)
        {
            if (List<T>.IsEmpty(q.F)) throw new Exception("Empty");
            var f = List<T>.Tail(q.F);
            return Check(q.LenF - 1, f, Invalidate(q.State), q.LenR, q.R);
        }
    }
}