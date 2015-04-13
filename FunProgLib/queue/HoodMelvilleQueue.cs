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
        {
            public virtual RotationState Exec()
            {
                return this;
            }

            public virtual RotationState Invalidate()
            {
                return this;
            }

            public virtual Queue Exec2(int lenF, List<T>.Node f, int lenR, List<T>.Node r)
            {
                return new Queue(lenF, f, this, lenR, r);
            }
        }

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

            public override RotationState Exec()
            {
                if (!List<T>.IsEmpty(f)) // && !List<T>.IsEmpty(reversing.R))
                {
                    var x1 = List<T>.Head(f);
                    var f1 = List<T>.Tail(f);
                    var y1 = List<T>.Head(r);
                    var r1 = List<T>.Tail(r);
                    return new Reversing(ok + 1, f1, List<T>.Cons(x1, fp), r1, List<T>.Cons(y1, rp));
                }

                var y2 = List<T>.Head(r);
                return new Appending(ok, fp, List<T>.Cons(y2, rp));
            }

            public override RotationState Invalidate()
            {
                return new Reversing(ok - 1, f, fp, r, rp);
            }
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

            public override RotationState Exec()
            {
                if (ok == 0)
                    return new Done(rp);

                var x1 = List<T>.Head(fp);
                var fp1 = List<T>.Tail(fp);
                return new Appending(ok - 1, fp1, List<T>.Cons(x1, rp));
            }

            public override RotationState Invalidate()
            {
                if (ok == 0) return new Done(List<T>.Tail(rp));
                return new Appending(ok - 1, fp, rp);
            }
        }

        private sealed class Done : RotationState
        {
            private readonly List<T>.Node f;

            public Done(List<T>.Node f)
            {
                this.f = f;
            }

            public override Queue Exec2(int lenF, List<T>.Node f1, int lenR, List<T>.Node r)
            {
                return new Queue(lenF, f, new Idle(), lenR, r);
            }
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

        private static Queue Exec2(int lenF, List<T>.Node f, RotationState state, int lenR, List<T>.Node r)
        {
            var newState = state.Exec().Exec();
            return newState.Exec2(lenF, f, lenR, r);
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
            return Check(q.LenF - 1, f, q.State.Invalidate(), q.LenR, q.R);
        }
    }
}