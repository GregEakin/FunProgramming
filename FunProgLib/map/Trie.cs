﻿// Fun Programming Data Structures 1.0
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

namespace FunProgLib.map
{
    public class NotFound : Exception { }

    public static class Trie<K, T>
        where K : IComparable<K>
        where T : class
    {
        public sealed class Map
        {
            public Map(T v, Map m)
            {
                V = v;
                M = m;
                MM = null;
            }

            public Map(T v, Map m, MMap mm)
            {
                V = v;
                M = m;
                MM = mm;
            }

            public T V { get; }

            public Map M { get; }

            public MMap MM { get; }

            public static Map Lookup(K item, Map list)
            {
                if (list?.MM == null) throw new NotFound(); // return null;  // Not Found
                if (item.CompareTo(list.MM.V) == 0) return list;
                return Lookup(item, list.MM.Map);
            }

            public static Map Bind(K item, Map map, Map list)
            {
                var mm = new MMap(item, list);
                var m = new Map(map.V, map.M, mm);
                return m;
            }
        }

        public sealed class MMap
        {
            public MMap(K item, Map map)
            {
                V = item;
                Map = map;
            }

            public K V { get; }
            public Map Map { get; }
        }

        public static Map Empty { get; } = new Map(null, null, null);

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