// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//
// Okasaki, Chris. "10.3.1 Tries." Purely Functional Data Structures. 
//     Cambridge, U.K.: Cambridge UP, 1998. 163-6. Print.

using System;
using FunProgLib.lists;

namespace FunProgLib.tree
{
    public class NotFound : Exception { }

    public static class Trie<K, T>
        where K : IComparable<K>
    {
        public sealed class Map
        {
            public Map(T v, Map m)
            {
                V = v;
                M = m;
                Option = default(K);
                Sibling = null;
            }

            public Map(T v, Map m, K option, Map sibling)
            {
                V = v;
                M = m;
                Option = option;
                Sibling = sibling;
            }

            public T V { get; }

            public Map M { get; }

            public K Option { get; }

            public Map Sibling { get; }

            public static Map Lookup(K item, Map sibling)
            {
                if (sibling == null) throw new NotFound();
                if (item.CompareTo(sibling.Option) == 0) return sibling;
                return Lookup(item, sibling.Sibling);
            }

            public static Map Bind(K item, Map child, Map sibling)
            {
                return new Map(child.V, child.M, item, sibling);
            }
        }

        public static Map Empty { get; } = new Map(default(T), null);

        public static T Lookup(List<K>.Node mKey, Map trie)
        {
            if (List<K>.IsEmpty(mKey) && trie.V == null) throw new NotFound();
            if (List<K>.IsEmpty(mKey)) return trie.V;
            return Lookup(List<K>.Tail(mKey), Map.Lookup(List<K>.Head(mKey), trie.M));
        }

        public static Map Bind(List<K>.Node mKey, T x, Map trie)
        {
            if (List<K>.IsEmpty(mKey)) return new Map(x, trie.M);
            Map t;
            try { t = Map.Lookup(List<K>.Head(mKey), trie.M); } catch (NotFound) { t = Empty; }
            var tp = Bind(List<K>.Tail(mKey), x, t);
            return new Map(trie.V, Map.Bind(List<K>.Head(mKey), tp, trie.M));
        }
    }
}