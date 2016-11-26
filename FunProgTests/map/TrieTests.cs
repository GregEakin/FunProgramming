// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Linq;
using System.Text;
using FunProgLib.lists;
using FunProgLib.map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FunProgTests.utilities.ExpectedException;

namespace FunProgTests.map
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            var trie = Trie<char, string>.Empty;
        }

        [TestMethod]
        public void BindTest()
        {
            var trie = Trie<char, string>.Empty;
            trie = Trie<char, string>.Bind(List<char>.Empty, "Dog", trie);
            Assert.AreEqual("map: Dog, ", DumpMap<char, string>(trie));
        }

        [TestMethod]
        public void NullLookupTest()
        {
            var trie = Trie<char, string>.Empty;
            var x = AssertThrows<Exception>(() => Trie<char, string>.Lookup(List<char>.Empty, trie));
        }

        [TestMethod]
        public void LookupTest()
        {
            var dog = "GOD".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            var trie = Trie<char, string>.Empty;
            trie = Trie<char, string>.Bind(dog, "Dog", trie);
            var x = Trie<char, string>.Lookup(dog, trie);
        }

        [TestMethod]
        public void Test()
        {
            var car = "RAC".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            var cart = "TARC".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            var dog = "GOD".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            var trie = Trie<char, string>.Empty;
            trie = Trie<char, string>.Bind(cart, "cart", trie);
            trie = Trie<char, string>.Bind(car, "car", trie);
            trie = Trie<char, string>.Bind(dog, "dog", trie);

            var result = DumpMap(trie);
            Console.Write(result);
        }

        [TestMethod]
        public void Test2()
        {
            var trie = Trie<char, string>.Empty;
            var dog = "DOG".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(dog, "Dog", trie);
            var result = DumpMap<char, string>(trie);
            Console.WriteLine(result);
        }

        private static string DumpMap<K, T>(Trie<K, T>.Map map) where K : IComparable<K> where T : class
        {
            if (map == null) return "";
            var buffer = new StringBuilder();
            buffer.Append($"map: {map.V}, ");
            buffer.Append(DumpMap(map.M));
            return buffer.ToString();
        }
    }
}