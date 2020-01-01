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

namespace FunProgLib.tree
{
    using lists;
    using System;

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

        public static TValue Lookup(List<TKey>.Node mKey, Map trie)
        {
            if (List<TKey>.IsEmpty(mKey) && trie.V == null) throw new NotFound();
            if (List<TKey>.IsEmpty(mKey)) return trie.V;
            return Lookup(List<TKey>.Tail(mKey), Map.Lookup(List<TKey>.Head(mKey), trie.M));
        }

        public static Map Bind(List<TKey>.Node mKey, TValue x, Map trie)
        {
            if (List<TKey>.IsEmpty(mKey)) return new Map(x, trie.M);
            Map t;
            try { t = Map.Lookup(List<TKey>.Head(mKey), trie.M); } catch (NotFound) { t = Empty; }
            var tp = Bind(List<TKey>.Tail(mKey), x, t);
            return new Map(trie.V, Map.Bind(List<TKey>.Head(mKey), tp, trie.M));
        }
    }
}
