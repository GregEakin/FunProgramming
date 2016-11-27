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
        // type Key = M.Key list
        // datatype α option = None | Some of α
        // datatype α Map = Trie of α option x α Map M.Map

        // val empty = Trie (None, M.Empty)

        // fun lookup([], Trie(None, m)) = raise NotFound
        //   | lookup([], Trie(Some x, m)) = x
        //   | lookup(k :: ks, Trie(v, m)) = lookup(ks, M.Lookup(k, m))

        // fun bind([], x, Trie(_, m)) = Trie(Some x, m)
        //   | bind (k :: ks, x, Trie(v, m) =
        //       let val t = M.Lookup(k, m) handle NotFound => empty
        //           val t' = bind(ks, x, t)
        //       in Trie(v, M.bind(k, t', m)) end

        // end

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
                if (list == null) throw new NotFound(); // return null;  // Not Found
                if (item.CompareTo(list.MM.V) == 0) return list;
                return Lookup(item, list.M);
            }

            public static Map Bind(K item, Map map, Map list)
            {
                var mm = new MMap(item);
                var m = new Map(map.V, map.M, mm);
                return m;
            }
        }

        public sealed class MMap
        {
            public MMap(K item)
            {
                V = item;
            }

            public K V { get; }
        }

        public static Map Empty { get; } = new Map(null, null, null);

        // fun lookup([], Trie(None, m)) = raise NotFound
        //   | lookup([], Trie(Some x, m)) = x
        //   | lookup(k :: ks, Trie(v, m)) = lookup(ks, M.Lookup(k, m))
        public static T Lookup(List<K>.Node mKey, Map trie)
        {
            if (List<K>.IsEmpty(mKey) && trie.V == null) throw new NotFound(); // return null;  // not found
            if (List<K>.IsEmpty(mKey)) return trie.V;
            return Lookup(List<K>.Tail(mKey), Map.Lookup(List<K>.Head(mKey), trie.M));
        }

        // fun bind([], x, Trie(_, m)) = Trie(Some x, m)
        //   | bind (k :: ks, x, Trie(v, m) =
        //       let val t = M.Lookup(k, m) handle NotFound => empty
        //           val t' = bind(ks, x, t)
        //       in Trie(v, M.bind(k, t', m)) end
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