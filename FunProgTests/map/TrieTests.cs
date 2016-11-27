﻿// Fun Programming Data Structures 1.0
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
        private static List<K>.Node ToList<K>(K[] value)
        {
            var car = value.Aggregate(List<K>.Empty, (current, letter) => List<K>.Cons(letter, current));
            return List<K>.Reverse(car);

        }

        private static string DumpMap<K, T>(Trie<K, T>.Map map) where K : IComparable<K> where T : class
        {
            if (map == null) return "";
            var buffer = new StringBuilder();
            buffer.Append("{");
            if (map.Option != null)
            {
                buffer.Append('\'');
                buffer.Append(map.Option.V);
                buffer.Append('\'');
            }
            buffer.Append(", ");
            buffer.Append(DumpMap(map.M));
            buffer.Append(", ");
            buffer.Append(DumpMap(map.Sibling));
            buffer.Append(", ");
            buffer.Append("\"");
            buffer.Append(map.V);
            buffer.Append("\"}");
            return buffer.ToString();
        }

        // Map Tests

        [TestMethod]
        public void MapLookupNullTest()
        {
            AssertThrows<NotFound>(() => Trie<char, string>.Map.Lookup('A', null));
        }

        [TestMethod]
        public void MapLookupSiblingTest()
        {
            var mm1 = new Trie<char, string>.Option('A');
            var list1 = new Trie<char, string>.Map("A", null, mm1, null);

            var mm2 = new Trie<char, string>.Option('B');
            var list2 = new Trie<char, string>.Map("B", null, mm2, list1);

            var aa = Trie<char, string>.Map.Lookup('A', list2);
            Assert.AreSame(list1, aa);

            var bb = Trie<char, string>.Map.Lookup('B', list2);
            Assert.AreSame(list2, bb);
        }

        [TestMethod]
        public void MapLookupChildTest()
        {
            var mm1 = new Trie<char, string>.Option('A');
            var list1 = new Trie<char, string>.Map("BA", null, mm1, null);

            var mm2 = new Trie<char, string>.Option('B');
            var list2 = new Trie<char, string>.Map("B", list1, mm2, null);

            AssertThrows<NotFound>(() => Trie<char, string>.Map.Lookup('A', list2));

            var bb = Trie<char, string>.Map.Lookup('B', list2);
            Assert.AreSame(list2, bb);
        }

        [TestMethod]
        public void MapBindSiblingTest()
        {
            var mapA = new Trie<char, string>.Map("A", null, null, null);
            var listA = new Trie<char, string>.Map(null, null, null, null);
            var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);

            Assert.AreEqual("A", list1.V);
            Assert.IsInstanceOfType(list1.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('A', list1.Option.V);

            var mapB = new Trie<char, string>.Map("B", null, null, null);
            var list2 = Trie<char, string>.Map.Bind('B', mapB, list1);

            Assert.AreEqual("B", list2.V);
            Assert.IsInstanceOfType(list2.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('B', list2.Option.V);

            var aa = Trie<char, string>.Map.Lookup('A', list2);
            Assert.AreSame(list1, aa);

            var bb = Trie<char, string>.Map.Lookup('B', list2);
            Assert.AreSame(list2, bb);
        }

        [TestMethod]
        public void MapBindChildTest()
        {
            var mapA = new Trie<char, string>.Map("BA", null, null, null);
            var listA = new Trie<char, string>.Map(null, null, null, null);
            var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);

            Assert.AreEqual("BA", list1.V);
            Assert.IsInstanceOfType(list1.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('A', list1.Option.V);

            var mapB = new Trie<char, string>.Map("B", list1, null, null);
            var list2 = Trie<char, string>.Map.Bind('B', mapB, null);

            Assert.AreEqual("B", list2.V);
            Assert.IsInstanceOfType(list2.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('B', list2.Option.V);

            AssertThrows<NotFound>(() => Trie<char, string>.Map.Lookup('A', list2));

            var bb = Trie<char, string>.Map.Lookup('B', list2);
            Assert.AreSame(list2, bb);
        }

        // Trie Tests

        [TestMethod]
        public void TrieEmptyTest()
        {
            var trie = Trie<char, string>.Empty;
        }

        [TestMethod]
        public void TrieLookupNullTest()
        {
            var trie = new Trie<char, string>.Map(null, null);
            AssertThrows<NotFound>(() => Trie<char, string>.Lookup(null, trie));
        }

        [TestMethod]
        public void TrieLookupEmptyTest()
        {
            var trie = Trie<char, string>.Empty;
            AssertThrows<NotFound>(() => Trie<char, string>.Lookup(List<char>.Empty, trie));
        }

        [TestMethod]
        public void TrieLookupSiblingTest()
        {
            var mm1 = new Trie<char, string>.Option('A');
            var list1 = new Trie<char, string>.Map("A", null, mm1, null);

            var mm2 = new Trie<char, string>.Option('B');
            var list2 = new Trie<char, string>.Map("B", null, mm2, list1);

            var list3 = new Trie<char, string>.Map(null, list2, null, null);

            var a = ToList("A".ToCharArray());
            var aa = Trie<char, string>.Lookup(a, list3);
            Assert.AreEqual("A", aa);

            var b = ToList("B".ToCharArray());
            var bb = Trie<char, string>.Lookup(b, list3);
            Assert.AreEqual("B", bb);
        }

        [TestMethod]
        public void TrieLookupChildTest()
        {
            var mm1 = new Trie<char, string>.Option('A');
            var list1 = new Trie<char, string>.Map("BA", null, mm1, null);

            var mm2 = new Trie<char, string>.Option('B');
            var list2 = new Trie<char, string>.Map("B", list1, mm2, null);

            var list3 = new Trie<char, string>.Map(null, list2, null, null);

            var b = ToList("B".ToCharArray());
            var bb = Trie<char, string>.Lookup(b, list3);
            Assert.AreEqual("B", bb);

            var a = ToList("BA".ToCharArray());
            var ab = Trie<char, string>.Lookup(a, list3);
            Assert.AreEqual("BA", ab);
        }

        [TestMethod]
        public void TrieBindSiblingTest()
        {
            var a = ToList("A".ToCharArray());
            var list01 = Trie<char, string>.Bind(a, "A", Trie<char, string>.Empty);
            var list1 = list01.M;
            Assert.AreEqual("A", list1.V);
            Assert.IsNull(list1.M);
            Assert.IsInstanceOfType(list1.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('A', list1.Option.V);
            Assert.IsNull(list1.Sibling);

            var b = ToList("B".ToCharArray());
            var list02 = Trie<char, string>.Bind(b, "B", list01);
            var list2 = list02.M;
            Assert.AreEqual("B", list2.V);
            Assert.IsNull(list2.M);
            Assert.IsInstanceOfType(list2.Option, typeof(Trie<char, string>.Option));
            Assert.AreEqual('B', list2.Option.V);
            Assert.AreSame(list1, list2.Sibling);

            var list3 = new Trie<char, string>.Map(null, list2, null, null);

            var aa = Trie<char, string>.Lookup(a, list3);
            Assert.AreEqual("A", aa);

            var bb = Trie<char, string>.Lookup(b, list3);
            Assert.AreEqual("B", bb);
        }

        [TestMethod]
        public void TrieBindChildTest()
        {
            var b = ToList("B".ToCharArray());
            var list02 = Trie<char, string>.Bind(b, "B", Trie<char, string>.Empty);
            {
                var list2 = list02.M;
                Assert.AreEqual("B", list2.V);
                Assert.IsNull(list2.M);
                Assert.IsInstanceOfType(list2.Option, typeof(Trie<char, string>.Option));
                Assert.AreEqual('B', list2.Option.V);
            }

            var a = ToList("BA".ToCharArray());
            var list01 = Trie<char, string>.Bind(a, "BA", list02);
            {
                Assert.AreEqual("B", list01.M.V);
                var list1 = list01.M.M;
                Assert.AreEqual("BA", list1.V);
                Assert.IsNull(list1.M);
                Assert.IsInstanceOfType(list1.Option, typeof(Trie<char, string>.Option));
                Assert.AreEqual('A', list1.Option.V);
                Assert.IsNull(list1.Sibling);
            }

            var bb = Trie<char, string>.Lookup(b, list01);
            Assert.AreEqual("B", bb);

            var ab = Trie<char, string>.Lookup(a, list01);
            Assert.AreEqual("BA", ab);
        }

        [TestMethod]
        public void TrieBindTest()
        {
            var a = ToList("BA".ToCharArray());
            var list01 = Trie<char, string>.Bind(a, "BA", Trie<char, string>.Empty);
            {
                Assert.IsNull(list01.M.V);
                var list1 = list01.M.M;
                Assert.AreEqual("BA", list1.V);
                Assert.IsNull(list1.M);
                Assert.IsInstanceOfType(list1.Option, typeof(Trie<char, string>.Option));
                Assert.AreEqual('A', list1.Option.V);
                Assert.IsNull(list1.Sibling);
            }

            var b = ToList("B".ToCharArray());
            AssertThrows<NotFound>(() => Trie<char, string>.Lookup(b, list01));

            var ab = Trie<char, string>.Lookup(a, list01);
            Assert.AreEqual("BA", ab);
        }

        [TestMethod]
        public void TrieLookupATest()
        {
            var trie = new Trie<char, string>.Map("a", null);
            var result = Trie<char, string>.Lookup(null, trie);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void TrieLookupAaTest()
        {
            var mm1 = new Trie<char, string>.Option('a');
            var list1 = new Trie<char, string>.Map("a", null, mm1, null);
            var list2 = new Trie<char, string>.Map(null, list1, null, null);

            var aNode = new List<char>.Node('a', null);
            var result = Trie<char, string>.Lookup(aNode, list2);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void TrieLookupAbTest()
        {
            var mm1 = new Trie<char, string>.Option('a');
            var list1 = new Trie<char, string>.Map("a", null, mm1, null);
            var mm2 = new Trie<char, string>.Option('b');
            var list2 = new Trie<char, string>.Map(null, list1, mm2, null);
            var list3 = new Trie<char, string>.Map(null, list2, null, null);

            var aNode = new List<char>.Node('b', new List<char>.Node('a', null));
            var result = Trie<char, string>.Lookup(aNode, list3);
            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void TrieLookupCTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = ToList("C".ToCharArray());
            trie = Trie<char, string>.Bind(c, "C", trie);

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);

            Assert.AreEqual("{, {'C', , , \"C\"}, , \"\"}", DumpMap(trie));
        }

        [TestMethod]
        public void TrieLookupDTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = ToList("C".ToCharArray());
            trie = Trie<char, string>.Bind(c, "C", trie);

            var d = ToList("D".ToCharArray());
            trie = Trie<char, string>.Bind(d, "D", trie);

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);

            var findD = Trie<char, string>.Lookup(d, trie);
            Assert.AreEqual("D", findD);

            Assert.AreEqual("{, {'D', , {'C', , , \"C\"}, \"D\"}, , \"\"}", DumpMap(trie));
        }

        [TestMethod]
        public void TrieLookupCbTest()
        {
            var trie = Trie<char, string>.Empty;

            var c = ToList("C".ToCharArray());
            trie = Trie<char, string>.Bind(c, "C", trie);

            var cb = ToList("CB".ToCharArray());
            trie = Trie<char, string>.Bind(cb, "CB", trie);

            var findC = Trie<char, string>.Lookup(c, trie);
            Assert.AreEqual("C", findC);

            var findCb = Trie<char, string>.Lookup(cb, trie);
            Assert.AreEqual("CB", findCb);

            Assert.AreEqual("{, {'C', {'B', , , \"CB\"}, {'C', , , \"C\"}, \"C\"}, , \"\"}", DumpMap(trie));
        }

        [TestMethod]
        public void TrieLookupDogTest()
        {
            var dog = ToList("DOG".ToCharArray());

            var trie = Trie<char, string>.Empty;
            trie = Trie<char, string>.Bind(dog, "Dog", trie);

            var findDog = Trie<char, string>.Lookup(dog, trie);
            Assert.AreEqual("Dog", findDog);
        }

        [TestMethod]
        public void TrieBindDogTest()
        {
            var trie1 = Trie<char, string>.Empty;
            var trie2 = Trie<char, string>.Bind(List<char>.Empty, "Dog", trie1);
            Assert.AreEqual("Dog", trie2.V);
            // Assert.AreSame(trie1, trie2.M);
            Assert.IsNull(trie2.Option);
            Assert.AreEqual("{, , , \"Dog\"}", DumpMap(trie2));
        }

        // Others

        [TestMethod]
        public void Test()
        {
            var trie1 = Trie<char, string>.Empty;
            Assert.AreEqual("{, , , \"\"}", DumpMap(trie1));

            var cart = ToList("CART".ToCharArray());
            var trie2 = Trie<char, string>.Bind(cart, "cart", trie1);
            Assert.AreEqual("{, {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, , \"\"}", DumpMap(trie2));

            var car = ToList("CAR".ToCharArray());
            var trie3 = Trie<char, string>.Bind(car, "car", trie2);
            Assert.AreEqual("{, {'C', {'A', {'R', {'T', , , \"cart\"}, {'R', {'T', , , \"cart\"}, , \"\"}, \"car\"}, {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, \"\"}, {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, \"\"}, , \"\"}", DumpMap(trie3));

            var dog = ToList("DOG".ToCharArray());
            var trie4 = Trie<char, string>.Bind(dog, "dog", trie3);

            var findCar = Trie<char, string>.Lookup(car, trie4);
            Assert.AreEqual("car", findCar);

            var findDog = Trie<char, string>.Lookup(dog, trie4);
            Assert.AreEqual("dog", findDog);

            var findCart = Trie<char, string>.Lookup(cart, trie4);
            Assert.AreEqual("cart", findCart);

            Console.WriteLine(DumpMap(trie3));
        }
    }
}