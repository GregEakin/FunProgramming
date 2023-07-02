// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.lists;
using FunProgLib.tree;

namespace FunProgTests.tree;

public class TrieTests
{
    private static FunList<TKey>.Node ToList<TKey>(TKey[] value)
    {
        var car = value.Aggregate(FunList<TKey>.Empty, (current, letter) => FunList<TKey>.Cons(letter, current));
        return FunList<TKey>.Reverse(car);
    }

    private static string DumpMap<TKey, TValue>(Trie<TKey, TValue>.Map map)
        where TKey : IComparable<TKey> where TValue : class
    {
        return map == null
            ? string.Empty
            : $"{{'{map.Option}', {DumpMap(map.M)}, {DumpMap(map.Sibling)}, \"{map.V}\"}}";
    }

    // Map Tests

    [Fact]
    public void MapLookupNullTest()
    {
        Assert.Throws<NotFound>(() => Trie<char, string>.Map.Lookup('A', null));
    }

    [Fact]
    public void MapLookupSiblingTest()
    {
        var list1 = new Trie<char, string>.Map("A", null, 'A', null);

        var list2 = new Trie<char, string>.Map("B", null, 'B', list1);

        var aa = Trie<char, string>.Map.Lookup('A', list2);
        Assert.Same(list1, aa);

        var bb = Trie<char, string>.Map.Lookup('B', list2);
        Assert.Same(list2, bb);
    }

    [Fact]
    public void MapLookupChildTest()
    {
        var list1 = new Trie<char, string>.Map("BA", null, 'A', null);

        var list2 = new Trie<char, string>.Map("B", list1, 'B', null);

        Assert.Throws<NotFound>(() => Trie<char, string>.Map.Lookup('A', list2));

        var bb = Trie<char, string>.Map.Lookup('B', list2);
        Assert.Same(list2, bb);
    }

    [Fact]
    public void MapBindSiblingTest()
    {
        var mapA = new Trie<char, string>.Map("A", null);
        var listA = new Trie<char, string>.Map(null, null);
        var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);

        Assert.Equal("A", list1.V);
        Assert.Equal('A', list1.Option);

        var mapB = new Trie<char, string>.Map("B", null);
        var list2 = Trie<char, string>.Map.Bind('B', mapB, list1);

        Assert.Equal("B", list2.V);
        Assert.Equal('B', list2.Option);

        var aa = Trie<char, string>.Map.Lookup('A', list2);
        Assert.Same(list1, aa);

        var bb = Trie<char, string>.Map.Lookup('B', list2);
        Assert.Same(list2, bb);
    }

    [Fact]
    public void MapBindChildTest()
    {
        var mapA = new Trie<char, string>.Map("BA", null);
        var listA = new Trie<char, string>.Map(null, null);
        var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);

        Assert.Equal("BA", list1.V);
        Assert.Equal('A', list1.Option);

        var mapB = new Trie<char, string>.Map("B", list1);
        var list2 = Trie<char, string>.Map.Bind('B', mapB, null);

        Assert.Equal("B", list2.V);
        Assert.Equal('B', list2.Option);

        Assert.Throws<NotFound>(() => Trie<char, string>.Map.Lookup('A', list2));

        var bb = Trie<char, string>.Map.Lookup('B', list2);
        Assert.Same(list2, bb);
    }

    // Trie Tests

    [Fact]
    public void TrieEmptyTest()
    {
        var trie = Trie<char, string>.Empty;
        Assert.Null(trie.V);  // default(string)
        Assert.Null(trie.M);
        Assert.Equal('\0', trie.Option); // default(char)
        Assert.Null(trie.Sibling);
    }

    [Fact]
    public void TrieLookupNullTest()
    {
        var trie = new Trie<char, string>.Map(null, null);
        Assert.Throws<NotFound>(() => Trie<char, string>.Lookup(null, trie));
    }

    [Fact]
    public void TrieLookupEmptyTest()
    {
        var trie = Trie<char, string>.Empty;
        Assert.Throws<NotFound>(() => Trie<char, string>.Lookup(FunList<char>.Empty, trie));
    }

    [Fact]
    public void TrieLookupSiblingTest()
    {
        var list1 = new Trie<char, string>.Map("A", null, 'A', null);

        var list2 = new Trie<char, string>.Map("B", null, 'B', list1);

        var list3 = new Trie<char, string>.Map(null, list2);

        var a = ToList("A".ToCharArray());
        var aa = Trie<char, string>.Lookup(a, list3);
        Assert.Equal("A", aa);

        var b = ToList("B".ToCharArray());
        var bb = Trie<char, string>.Lookup(b, list3);
        Assert.Equal("B", bb);
    }

    [Fact]
    public void TrieLookupChildTest()
    {
        var list1 = new Trie<char, string>.Map("BA", null, 'A', null);

        var list2 = new Trie<char, string>.Map("B", list1, 'B', null);

        var list3 = new Trie<char, string>.Map(null, list2);

        var b = ToList("B".ToCharArray());
        var bb = Trie<char, string>.Lookup(b, list3);
        Assert.Equal("B", bb);

        var a = ToList("BA".ToCharArray());
        var ab = Trie<char, string>.Lookup(a, list3);
        Assert.Equal("BA", ab);
    }

    [Fact]
    public void TrieBindSiblingTest()
    {
        var a = ToList("A".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "A", Trie<char, string>.Empty);
        var list1 = list01.M;
        Assert.Equal("A", list1.V);
        Assert.Null(list1.M);
        Assert.Equal('A', list1.Option);
        Assert.Null(list1.Sibling);

        var b = ToList("B".ToCharArray());
        var list02 = Trie<char, string>.Bind(b, "B", list01);
        var list2 = list02.M;
        Assert.Equal("B", list2.V);
        Assert.Null(list2.M);
        Assert.Equal('B', list2.Option);
        Assert.Same(list1, list2.Sibling);

        var list3 = new Trie<char, string>.Map(null, list2);

        var aa = Trie<char, string>.Lookup(a, list3);
        Assert.Equal("A", aa);

        var bb = Trie<char, string>.Lookup(b, list3);
        Assert.Equal("B", bb);
    }

    [Fact]
    public void TrieBindChildTest()
    {
        var b = ToList("B".ToCharArray());
        var list02 = Trie<char, string>.Bind(b, "B", Trie<char, string>.Empty);
        {
            var list2 = list02.M;
            Assert.Equal("B", list2.V);
            Assert.Null(list2.M);
            Assert.Equal('B', list2.Option);
        }

        var a = ToList("BA".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "BA", list02);
        {
            Assert.Equal("B", list01.M.V);
            var list1 = list01.M.M;
            Assert.Equal("BA", list1.V);
            Assert.Null(list1.M);
            Assert.Equal('A', list1.Option);
            Assert.Null(list1.Sibling);
        }

        var bb = Trie<char, string>.Lookup(b, list01);
        Assert.Equal("B", bb);

        var ab = Trie<char, string>.Lookup(a, list01);
        Assert.Equal("BA", ab);
    }

    [Fact]
    public void TrieBindTest()
    {
        var a = ToList("BA".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "BA", Trie<char, string>.Empty);
        {
            Assert.Null(list01.M.V);
            var list1 = list01.M.M;
            Assert.Equal("BA", list1.V);
            Assert.Null(list1.M);
            Assert.Equal('A', list1.Option);
            Assert.Null(list1.Sibling);
        }

        var b = ToList("B".ToCharArray());
        Assert.Throws<NotFound>(() => Trie<char, string>.Lookup(b, list01));

        var ab = Trie<char, string>.Lookup(a, list01);
        Assert.Equal("BA", ab);
    }

    [Fact]
    public void TrieLookupATest()
    {
        var trie = new Trie<char, string>.Map("a", null);
        var result = Trie<char, string>.Lookup(null, trie);
        Assert.Equal("a", result);
    }

    [Fact]
    public void TrieLookupAaTest()
    {
        var list1 = new Trie<char, string>.Map("a", null, 'a', null);
        var list2 = new Trie<char, string>.Map(null, list1);

        var aNode = new FunList<char>.Node('a', null);
        var result = Trie<char, string>.Lookup(aNode, list2);
        Assert.Equal("a", result);
    }

    [Fact]
    public void TrieLookupAbTest()
    {
        var list1 = new Trie<char, string>.Map("a", null, 'a', null);
        var list2 = new Trie<char, string>.Map(null, list1, 'b', null);
        var list3 = new Trie<char, string>.Map(null, list2);

        var aNode = new FunList<char>.Node('b', new FunList<char>.Node('a', null));
        var result = Trie<char, string>.Lookup(aNode, list3);
        Assert.Equal("a", result);
    }

    [Fact]
    public void TrieLookupCTest()
    {
        var trie = Trie<char, string>.Empty;

        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);

        var findC = Trie<char, string>.Lookup(c, trie);
        Assert.Equal("C", findC);

        Assert.Equal("{'\0', {'C', , , \"C\"}, , \"\"}", DumpMap(trie));
    }

    [Fact]
    public void TrieLookupDTest()
    {
        var trie = Trie<char, string>.Empty;

        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);

        var d = ToList("D".ToCharArray());
        trie = Trie<char, string>.Bind(d, "D", trie);

        var findC = Trie<char, string>.Lookup(c, trie);
        Assert.Equal("C", findC);

        var findD = Trie<char, string>.Lookup(d, trie);
        Assert.Equal("D", findD);

        Assert.Equal("{'\0', {'D', , {'C', , , \"C\"}, \"D\"}, , \"\"}", DumpMap(trie));
    }

    [Fact]
    public void TrieLookupBcTest()
    {
        var trie = Trie<char, string>.Empty;

        var cb = ToList("CB".ToCharArray());
        trie = Trie<char, string>.Bind(cb, "CB", trie);

        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);

        var findC = Trie<char, string>.Lookup(c, trie);
        Assert.Equal("C", findC);

        var findCb = Trie<char, string>.Lookup(cb, trie);
        Assert.Equal("CB", findCb);

        //Assert.Equal("{'\0', {'C', {'B', , , \"CB\"}, {'C', , , \"C\"}, \"C\"}, , \"\"}", DumpMap(trie));
        Assert.Equal("{'\0', {'C', {'B', , , \"CB\"}, {'C', {'B', , , \"CB\"}, , \"\"}, \"C\"}, , \"\"}",
            DumpMap(trie));
    }

    [Fact]
    public void TrieLookupCbTest()
    {
        var trie = Trie<char, string>.Empty;

        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);

        var cb = ToList("CB".ToCharArray());
        trie = Trie<char, string>.Bind(cb, "CB", trie);

        var findC = Trie<char, string>.Lookup(c, trie);
        Assert.Equal("C", findC);

        var findCb = Trie<char, string>.Lookup(cb, trie);
        Assert.Equal("CB", findCb);

        Assert.Equal("{'\0', {'C', {'B', , , \"CB\"}, {'C', , , \"C\"}, \"C\"}, , \"\"}", DumpMap(trie));
    }

    [Fact]
    public void TrieLookupDogTest()
    {
        var dog = ToList("DOG".ToCharArray());

        var trie = Trie<char, string>.Empty;
        trie = Trie<char, string>.Bind(dog, "Dog", trie);

        var findDog = Trie<char, string>.Lookup(dog, trie);
        Assert.Equal("Dog", findDog);
    }

    [Fact]
    public void TrieBindDogTest()
    {
        var trie1 = Trie<char, string>.Empty;
        var trie2 = Trie<char, string>.Bind(FunList<char>.Empty, "Dog", trie1);
        Assert.Equal("Dog", trie2.V);
        // Assert.Same(trie1, trie2.M);
        Assert.Equal('\0', trie2.Option);
        Assert.Equal("{'\0', , , \"Dog\"}", DumpMap(trie2));
    }

    // Others

    [Fact]
    public void Test()
    {
        var trie1 = Trie<char, string>.Empty;
        Assert.Equal("{'\0', , , \"\"}", DumpMap(trie1));

        var cart = ToList("CART".ToCharArray());
        var trie2 = Trie<char, string>.Bind(cart, "cart", trie1);
        Assert.Equal("{'\0', {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, , \"\"}",
            DumpMap(trie2));

        var car = ToList("CAR".ToCharArray());
        var trie3 = Trie<char, string>.Bind(car, "car", trie2);
        Assert.Equal(
            "{'\0', {'C', {'A', {'R', {'T', , , \"cart\"}, {'R', {'T', , , \"cart\"}, , \"\"}, \"car\"}, {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, \"\"}, {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, \"\"}, , \"\"}",
            DumpMap(trie3));

        var dog = ToList("DOG".ToCharArray());
        var trie4 = Trie<char, string>.Bind(dog, "dog", trie3);

        var findCar = Trie<char, string>.Lookup(car, trie4);
        Assert.Equal("car", findCar);

        var findDog = Trie<char, string>.Lookup(dog, trie4);
        Assert.Equal("dog", findDog);

        var findCart = Trie<char, string>.Lookup(cart, trie4);
        Assert.Equal("cart", findCart);

        Console.WriteLine(DumpMap(trie3));
    }
}