// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.3.1 Tries." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 163-6. Print.

using FunProgLib.lists;

namespace FunProgLib.tree;

public class NotFound : Exception { }

public static class Trie<TKey, TValue>
    where TKey : IComparable<TKey>
{
    public sealed class Map
    {
        public Map(TValue v, Map m)
        {
            V = v;
            M = m;
            Option = default(TKey);
            Sibling = null;
        }

        public Map(TValue v, Map m, TKey option, Map sibling)
        {
            V = v;
            M = m;
            Option = option;
            Sibling = sibling;
        }

        public TValue V { get; }

        public Map M { get; }

        public TKey Option { get; }

        public Map Sibling { get; }

        public static Map Lookup(TKey item, Map sibling)
        {
            if (sibling == null) throw new NotFound();
            if (item.CompareTo(sibling.Option) == 0) return sibling;
            return Lookup(item, sibling.Sibling);
        }

        public static Map Bind(TKey item, Map child, Map sibling) => new Map(child.V, child.M, item, sibling);
    }

    public static Map Empty { get; } = new Map(default(TValue), null);

    public static TValue Lookup(FunList<TKey>.Node mKey, Map trie)
    {
        if (FunList<TKey>.IsEmpty(mKey) && trie.V == null) throw new NotFound();
        if (FunList<TKey>.IsEmpty(mKey)) return trie.V;
        return Lookup(FunList<TKey>.Tail(mKey), Map.Lookup(FunList<TKey>.Head(mKey), trie.M));
    }

    public static Map Bind(FunList<TKey>.Node mKey, TValue x, Map trie)
    {
        if (FunList<TKey>.IsEmpty(mKey)) return new Map(x, trie.M);
        Map t;
        try { t = Map.Lookup(FunList<TKey>.Head(mKey), trie.M); } catch (NotFound) { t = Empty; }
        var tp = Bind(FunList<TKey>.Tail(mKey), x, t);
        return new Map(trie.V, Map.Bind(FunList<TKey>.Head(mKey), tp, trie.M));
    }
}