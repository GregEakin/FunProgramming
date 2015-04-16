// Fun Programming Data Structures 1.0
// 
// Copyright © 2015 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.1.2 Binary Random-Access Lists Revisited." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 144-7. Print.

using System;

namespace FunProgLib.queue
{
    public static class CatenableList<T> // where T : IQueue<T> // : ICatenableList<T> where T is Queue
    {
        // dataype alpha Cat = E | C of alpha X alpha Cat susp Q.Queue

        public sealed class Cat
        {
            // T x
            // IQueue<T> q
        }

        public static Cat Empty
        {
            get { return null; }
        }

        public static bool IsEmpty(Cat rlist)
        {
            return rlist == null;
        }

        // fun link (C(x, q), s) = C(x, Q.snoc(q, s))
        // fun linkAll q = let val $t = Q.head q
        //                     val qp = Q.tail q
        //                 in if Q.isEmpty qp then t else link (t, $linkAll qp) end
        // fun xs cat E = xs
        //   | E cat xs = xs
        //   | xs cat ys = link(xs, $ys)

        public static Cat Cons(T element, Cat cat)
        {
            // C(x, Q.empty) cat xs
            // return T.Cons(new Cat(element, Q.Empty), cat.Q);
            throw new NotImplementedException();
        }

        public static Cat Snoc(Cat cat, T element)
        {
            // xs cat C(x, Q.empty)    
            // return T.Snoc(cat.Q, new Cat(element, Q.Empty));
            throw new NotImplementedException();
        }

        public static Cat Concat(Cat left, Cat right)
        {
            throw new NotImplementedException();
        }

        public static T Head(Cat cat)
        {
            if (IsEmpty(cat)) throw new Exception("Empty");
            // head(C(x, _)) = x
            // return cat.X;
            throw new NotImplementedException();
        }

        public static Cat Tail(Cat cat)
        {
            if (IsEmpty(cat)) throw new Exception("Empty");
            // tail(C(x, q)) = if Q.isEmpty q then E else linkAll q 
            // return T.IsEmpty(cat.Q) 
            //    ? Empty 
            //    : LinkAll(cat.Q);
            throw new NotImplementedException();
        }
    }
}