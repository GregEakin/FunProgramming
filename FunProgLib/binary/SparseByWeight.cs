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
// Okasaki, Chris. "9.1 Positional Number System." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 116-17. Print.

using FunProgLib.lists;

namespace FunProgLib.binary;

public static class SparseByWeight
{
    private static FunList<int>.Node Carry(int w, FunList<int>.Node list)
    {
        if (FunList<int>.IsEmpty(list)) return FunList<int>.Cons(w, null);
        if (w < list.Element) return FunList<int>.Cons(w, list);
        return Carry(2 * w, list.Next);
    }

    private static FunList<int>.Node Borrow(int w, FunList<int>.Node list)
    {
        if (w == list.Element) return list.Next;
        return FunList<int>.Cons(w, Borrow(2 * w, list));
    }

    public static FunList<int>.Node Inc(FunList<int>.Node ws) => Carry(1, ws);

    public static FunList<int>.Node Dec(FunList<int>.Node ws) => Borrow(1, ws);

    public static FunList<int>.Node Add(FunList<int>.Node ds1, FunList<int>.Node ds2)
    {
        if (FunList<int>.IsEmpty(ds2)) return ds1;
        if (FunList<int>.IsEmpty(ds1)) return ds2;
        if (ds1.Element < ds2.Element) return FunList<int>.Cons(ds1.Element, Add(ds1.Next, ds2));
        if (ds2.Element < ds1.Element) return FunList<int>.Cons(ds2.Element, Add(ds1, ds2.Next));
        return Carry(2 * ds1.Element, Add(ds1.Next, ds2.Next));
    }
}