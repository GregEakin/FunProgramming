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

            public virtual Queue Exec2(int lenF, List2<T> f, int lenR, List2<T> r)
            {
                return new Queue(lenF, f, this, lenR, r);
            }
        }

        private sealed class Idle : RotationState
        { }

        private sealed class Reversing : RotationState
        {
            private readonly int ok;
            private readonly List2<T> f;
            private readonly List2<T> fp;
            private readonly List2<T> r;
            private readonly List2<T> rp;

            public Reversing(int ok, List2<T> f, List2<T> fp, List2<T> r, List2<T> rp)
            {
                this.ok = ok;
                this.f = f;
                this.fp = fp;
                this.r = r;
                this.rp = rp;
            }

            public override RotationState Exec()
            {
                if (!f.IsEmpty) // && !reversing.R.IsEmpty)
                {
                    var x1 = f.Head;
                    var f1 = f.Tail;
                    var y1 = r.Head;
                    var r1 = r.Tail;
                    return new Reversing(ok + 1, f1, fp.Cons(x1), r1, rp.Cons(y1));
                }

                var y2 = r.Head;
                return new Appending(ok, fp, rp.Cons(y2));
            }

            public override RotationState Invalidate()
            {
                return new Reversing(ok - 1, f, fp, r, rp);
            }
        }

        private sealed class Appending : RotationState
        {
            private readonly int ok;
            private readonly List2<T> fp;
            private readonly List2<T> rp;

            public Appending(int ok, List2<T> fp, List2<T> rp)
            {
                this.ok = ok;
                this.fp = fp;
                this.rp = rp;
            }

            public override RotationState Exec()
            {
                if (ok == 0)
                    return new Done(rp);

                var x1 = fp.Head;
                var fp1 = fp.Tail;
                return new Appending(ok - 1, fp1, rp.Cons(x1));
            }

            public override RotationState Invalidate()
            {
                if (ok == 0) return new Done(rp.Tail);
                return new Appending(ok - 1, fp, rp);
            }
        }

        private sealed class Done : RotationState
        {
            private readonly List2<T> f;

            public Done(List2<T> f)
            {
                this.f = f;
            }

            public override Queue Exec2(int lenF, List2<T> f1, int lenR, List2<T> r)
            {
                return new Queue(lenF, f, new Idle(), lenR, r);
            }
        }

        public sealed class Queue
        {
            private readonly int lenF;
            private readonly List2<T> f;
            private readonly RotationState state;
            private readonly int lenR;
            private readonly List2<T> r;

            public Queue(int lenF, List2<T> f, RotationState state, int lenR, List2<T> r)
            {
                this.lenF = lenF;
                this.f = f;
                this.state = state;
                this.lenR = lenR;
                this.r = r;
            }

            public int LenF { get { return lenF; } }
            public List2<T> F { get { return f; } }
            public RotationState State { get { return state; } }
            public int LenR { get { return lenR; } }
            public List2<T> R { get { return r; } }
        }

        private static Queue Exec2(int lenF, List2<T> f, RotationState state, int lenR, List2<T> r)
        {
            var newState = state.Exec().Exec();
            return newState.Exec2(lenF, f, lenR, r);
        }

        private static Queue Check(int lenF, List2<T> f, RotationState state, int lenR, List2<T> r)
        {
            if (lenR <= lenF) return Exec2(lenF, f, state, lenR, r);
            var newState = new Reversing(0, f, List2<T>.Empty, r, List2<T>.Empty);
            return Exec2(lenF + lenR, f, newState, 0, List2<T>.Empty);
        }

        private static readonly Queue EmptyQueue = new Queue(0, List2<T>.Empty, new Idle(), 0, List2<T>.Empty);

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
            return Check(q.LenF, q.F, q.State, q.LenR + 1, q.R.Cons(x));
        }

        public static T Head(Queue q)
        {
            if (IsEmpty(q)) throw new Exception("Empty");
            var x = q.F.Head;
            return x;
        }

        public static Queue Tail(Queue q)
        {
            if (q.F.IsEmpty) throw new Exception("Empty");
            var f = q.F.Tail;
            return Check(q.LenF - 1, f, q.State.Invalidate(), q.LenR, q.R);
        }
    }
}