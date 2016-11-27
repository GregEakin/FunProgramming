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
        private static string DumpMap<K, T>(Trie<K, T>.Map map) where K : IComparable<K> where T : class
        {
            if (map == null) return "";
            var buffer = new StringBuilder();
            buffer.Append("{ \"");
            buffer.Append(map.V);
            buffer.Append("\": ");
            buffer.Append(DumpMap(map.M));
            buffer.Append("; ");
            buffer.Append(DumpMMap(map.MM));
            buffer.Append(" }");
            return buffer.ToString();
        }

        private static string DumpMMap<K, T>(Trie<K, T>.MMap map) where K : IComparable<K> where T : class
        {
            if (map == null) return "";
            var buffer = new StringBuilder();
            buffer.Append("'");
            buffer.Append(map.V);
            buffer.Append("'");
            return buffer.ToString();
        }

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
            Assert.AreEqual("{ 2 \"Dog\": ;  }", DumpMap<char, string>(trie));
        }

        [TestMethod]
        public void NullLookupTest()
        {
            var trie = Trie<char, string>.Empty;
            AssertThrows<NotFound>(() => Trie<char, string>.Lookup(List<char>.Empty, trie));
        }

        [TestMethod]
        public void LookupTest()
        {
            var dog = "GOD".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));

            var trie = Trie<char, string>.Empty;
            trie = Trie<char, string>.Bind(dog, "Dog", trie);

            var findDog = Trie<char, string>.Lookup(dog, trie);
            Assert.AreEqual("Dog", findDog);
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

            var findCar = Trie<char, string>.Lookup(car, trie);
            Assert.AreEqual("car", findCar);

            var findDog = Trie<char, string>.Lookup(dog, trie);
            Assert.AreEqual("dog", findDog);

            var findCart = Trie<char, string>.Lookup(cart, trie);
            Assert.AreEqual("cart", findCart);
        }

        [TestMethod]
        public void Test2()
        {
            var trie = Trie<char, string>.Empty;
            var dog = "GOD".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(dog, "Dog", trie);

            var findDog = Trie<char, string>.Lookup(dog, trie);
            Assert.AreEqual("Dog", findDog);

            var result = DumpMap(trie);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void Test3()
        {
            var trie = Trie<char, string>.Empty;
            Console.WriteLine($"1: {DumpMap(trie)}");

            var c = "C".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(c, "C", trie);
            Console.WriteLine($"2: {DumpMap(trie)}");

            var d = "D".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(d, "D", trie);
            Console.WriteLine($"3: {DumpMap(trie)}");

            var ce = "EC".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(ce, "CE", trie);
            Console.WriteLine($"4: {DumpMap(trie)}");
        }

        [TestMethod]
        public void FindCTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = "C".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(c, "C", trie);

            Console.WriteLine(DumpMap(trie));

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);
        }

        [TestMethod]
        public void FindDTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = "C".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(c, "C", trie);

            var d = "D".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(d, "D", trie);

            Console.WriteLine(DumpMap(trie));

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);

            var findD = Trie<char, string>.Lookup(d, trie);
            Assert.AreEqual("D", findD);
        }

        [TestMethod]
        public void FindCbTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = "C".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(c, "C", trie);

            var cb = "BC".ToCharArray().Aggregate(List<char>.Empty, (current, letter) => List<char>.Cons(letter, current));
            trie = Trie<char, string>.Bind(cb, "CB", trie);

            Console.WriteLine(DumpMap(trie));

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);

            var findCb = Trie<char, string>.Lookup(cb, trie);
            Assert.AreEqual("CB", findCb);
        }

        [TestMethod]
        public void LookupEmptyMap()
        {
            AssertThrows<NotFound>(() => Trie<char, string>.Map.Lookup('A', null));
        }

        [TestMethod]
        public void LookupMap()
        {
            var mm1 = new Trie<char, string>.MMap('A');
            var list1 = new Trie<char, string>.Map(null, null, mm1);

            var mm2 = new Trie<char, string>.MMap('B');
            var list2 = new Trie<char, string>.Map(null, list1, mm2);

            var aa = Trie<char, string>.Map.Lookup('A', list2);
            Assert.AreSame(list1, aa);

            var bb = Trie<char, string>.Map.Lookup('B', list2);
            Assert.AreSame(list2, bb);

            AssertThrows<NotFound>(() => Trie<char, string>.Map.Lookup('C', list2));
        }

        [TestMethod]
        public void LookupTrie()
        {
            var mm1 = new Trie<char, string>.MMap('a');
            var list1 = new Trie<char, string>.Map("a", null, mm1);
            var list2 = new Trie<char, string>.Map(null, list1, null);

            var aNode = new List<char>.Node('a', null);
            var result = Trie<char, string>.Lookup(aNode, list2);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void Lookup2Trie()
        {
            var mm1 = new Trie<char, string>.MMap('a');
            var list1 = new Trie<char, string>.Map("a", null, mm1);
            var mm2 = new Trie<char, string>.MMap('b');
            var list2 = new Trie<char, string>.Map(null, list1, mm2);
            var list3 = new Trie<char, string>.Map(null, list2, null);

            var aNode = new List<char>.Node('b', new List<char>.Node('a', null));
            var result = Trie<char, string>.Lookup(aNode, list3);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void LookupNullTrie()
        {
            var trie = new Trie<char, string>.Map(null, null);
            AssertThrows<NotFound>(() => Trie<char, string>.Lookup(null, trie));
        }

        [TestMethod]
        public void LookupEmptyTrie()
        {
            var trie = new Trie<char, string>.Map("a", null);
            var result = Trie<char, string>.Lookup(null, trie);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void Lookup1Trie()
        {
            var mm1 = new Trie<char, string>.MMap('A');
            var list1 = new Trie<char, string>.Map(null, null, mm1);

            var mm2 = new Trie<char, string>.MMap('B');
            var list2 = new Trie<char, string>.Map(null, null, mm2);

            var aNode = new List<char>.Node('A', null);
            var aa = Trie<char, string>.Lookup(aNode, list1);

            //var bNode = new List<char>.Node('B', null);
            //var bb = Trie<char, string>.Lookup('B', list2);
            //Assert.AreSame(list2, bb);

            //var cNode = new List<char>.Node('C', null);
            //AssertThrows<NotFound>(() => Trie<char, string>.Lookup('C', list2));
        }
    }
}