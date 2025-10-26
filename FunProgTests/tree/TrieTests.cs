// Copyright 2025 Gregory Eakin <greg@eakin.dev>
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
        return map == null ? string.Empty : $"{{'{map.Option}', {DumpMap(map.M)}, {DumpMap(map.Sibling)}, \"{map.V}\"}}";
    }

    // Map Tests
    [Test]
    public async Task MapLookupNullTest()
    {
        await Assert.That(() => Trie<char, string>.Map.Lookup('A', null)).Throws<KeyNotFoundException>();
    }

    [Test]
    public async Task MapLookupSiblingTest()
    {
        var list1 = new Trie<char, string>.Map("A", null, 'A', null);
        var list2 = new Trie<char, string>.Map("B", null, 'B', list1);
        var aa = Trie<char, string>.Map.Lookup('A', list2);
        await Assert.That(aa).IsSameReferenceAs(list1);
        var bb = Trie<char, string>.Map.Lookup('B', list2);
        await Assert.That(bb).IsSameReferenceAs(list2);
    }

    [Test]
    public async Task MapLookupChildTest()
    {
        var list1 = new Trie<char, string>.Map("BA", null, 'A', null);
        var list2 = new Trie<char, string>.Map("B", list1, 'B', null);
        await Assert.That(() => Trie<char, string>.Map.Lookup('A', list2)).Throws<KeyNotFoundException>();
        var bb = Trie<char, string>.Map.Lookup('B', list2);
        await Assert.That(bb).IsSameReferenceAs(list2);
    }

    [Test]
    public async Task MapBindSiblingTest()
    {
        var mapA = new Trie<char, string>.Map("A", null);
        var listA = new Trie<char, string>.Map(null, null);
        var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);
        await Assert.That(list1.V).IsEqualTo("A");
        await Assert.That(list1.Option).IsEqualTo('A');
        var mapB = new Trie<char, string>.Map("B", null);
        var list2 = Trie<char, string>.Map.Bind('B', mapB, list1);
        await Assert.That(list2.V).IsEqualTo("B");
        await Assert.That(list2.Option).IsEqualTo('B');
        var aa = Trie<char, string>.Map.Lookup('A', list2);
        await Assert.That(aa).IsSameReferenceAs(list1);
        var bb = Trie<char, string>.Map.Lookup('B', list2);
        await Assert.That(bb).IsSameReferenceAs(list2);
    }

    [Test]
    public async Task MapBindChildTest()
    {
        var mapA = new Trie<char, string>.Map("BA", null);
        var listA = new Trie<char, string>.Map(null, null);
        var list1 = Trie<char, string>.Map.Bind('A', mapA, listA);
        await Assert.That(list1.V).IsEqualTo("BA");
        await Assert.That(list1.Option).IsEqualTo('A');
        var mapB = new Trie<char, string>.Map("B", list1);
        var list2 = Trie<char, string>.Map.Bind('B', mapB, null);
        await Assert.That(list2.V).IsEqualTo("B");
        await Assert.That(list2.Option).IsEqualTo('B');
        await Assert.That(() => Trie<char, string>.Map.Lookup('A', list2)).Throws<KeyNotFoundException>();
        var bb = Trie<char, string>.Map.Lookup('B', list2);
        await Assert.That(bb).IsSameReferenceAs(list2);
    }

    // Trie Tests
    [Test]
    public async Task TrieEmptyTest()
    {
        var trie = Trie<char, string>.Empty;
        await Assert.That(trie.V).IsNull(); // default(string)
        await Assert.That(trie.M).IsNull();
        await Assert.That(trie.Option).IsEqualTo('\0'); // default(char)
        await Assert.That(trie.Sibling).IsNull();
    }

    [Test]
    public async Task TrieLookupNullTest()
    {
        var trie = new Trie<char, string>.Map(null, null);
        await Assert.That(() => Trie<char, string>.Lookup(null, trie)).Throws<KeyNotFoundException>();
    }

    [Test]
    public async Task TrieLookupEmptyTest()
    {
        var trie = Trie<char, string>.Empty;
        await Assert.That(() => Trie<char, string>.Lookup(FunList<char>.Empty, trie)).Throws<KeyNotFoundException>();
    }

    [Test]
    public async Task TrieLookupSiblingTest()
    {
        var list1 = new Trie<char, string>.Map("A", null, 'A', null);
        var list2 = new Trie<char, string>.Map("B", null, 'B', list1);
        var list3 = new Trie<char, string>.Map(null, list2);
        var a = ToList("A".ToCharArray());
        var aa = Trie<char, string>.Lookup(a, list3);
        await Assert.That(aa).IsEqualTo("A");
        var b = ToList("B".ToCharArray());
        var bb = Trie<char, string>.Lookup(b, list3);
        await Assert.That(bb).IsEqualTo("B");
    }

    [Test]
    public async Task TrieLookupChildTest()
    {
        var list1 = new Trie<char, string>.Map("BA", null, 'A', null);
        var list2 = new Trie<char, string>.Map("B", list1, 'B', null);
        var list3 = new Trie<char, string>.Map(null, list2);
        var b = ToList("B".ToCharArray());
        var bb = Trie<char, string>.Lookup(b, list3);
        await Assert.That(bb).IsEqualTo("B");
        var a = ToList("BA".ToCharArray());
        var ab = Trie<char, string>.Lookup(a, list3);
        await Assert.That(ab).IsEqualTo("BA");
    }

    [Test]
    public async Task TrieBindSiblingTest()
    {
        var a = ToList("A".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "A", Trie<char, string>.Empty);
        var list1 = list01.M;
        await Assert.That(list1.V).IsEqualTo("A");
        await Assert.That(list1.M).IsNull();
        await Assert.That(list1.Option).IsEqualTo('A');
        await Assert.That(list1.Sibling).IsNull();
        var b = ToList("B".ToCharArray());
        var list02 = Trie<char, string>.Bind(b, "B", list01);
        var list2 = list02.M;
        await Assert.That(list2.V).IsEqualTo("B");
        await Assert.That(list2.M).IsNull();
        await Assert.That(list2.Option).IsEqualTo('B');
        await Assert.That(list2.Sibling).IsSameReferenceAs(list1);
        var list3 = new Trie<char, string>.Map(null, list2);
        var aa = Trie<char, string>.Lookup(a, list3);
        await Assert.That(aa).IsEqualTo("A");
        var bb = Trie<char, string>.Lookup(b, list3);
        await Assert.That(bb).IsEqualTo("B");
    }

    [Test]
    public async Task TrieBindChildTest()
    {
        var b = ToList("B".ToCharArray());
        var list02 = Trie<char, string>.Bind(b, "B", Trie<char, string>.Empty);
        {
            var list2 = list02.M;
            await Assert.That(list2.V).IsEqualTo("B");
            await Assert.That(list2.M).IsNull();
            await Assert.That(list2.Option).IsEqualTo('B');
        }

        var a = ToList("BA".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "BA", list02);
        {
            await Assert.That(list01.M.V).IsEqualTo("B");
            var list1 = list01.M.M;
            await Assert.That(list1.V).IsEqualTo("BA");
            await Assert.That(list1.M).IsNull();
            await Assert.That(list1.Option).IsEqualTo('A');
            await Assert.That(list1.Sibling).IsNull();
        }

        var bb = Trie<char, string>.Lookup(b, list01);
        await Assert.That(bb).IsEqualTo("B");
        var ab = Trie<char, string>.Lookup(a, list01);
        await Assert.That(ab).IsEqualTo("BA");
    }

    [Test]
    public async Task TrieBindTest()
    {
        var a = ToList("BA".ToCharArray());
        var list01 = Trie<char, string>.Bind(a, "BA", Trie<char, string>.Empty);
        {
            await Assert.That(list01.M.V).IsNull();
            var list1 = list01.M.M;
            await Assert.That(list1.V).IsEqualTo("BA");
            await Assert.That(list1.M).IsNull();
            await Assert.That(list1.Option).IsEqualTo('A');
            await Assert.That(list1.Sibling).IsNull();
        }

        var b = ToList("B".ToCharArray());
        await Assert.That(() => Trie<char, string>.Lookup(b, list01)).Throws<KeyNotFoundException>();
        var ab = Trie<char, string>.Lookup(a, list01);
        await Assert.That(ab).IsEqualTo("BA");
    }

    [Test]
    public async Task TrieLookupATest()
    {
        var trie = new Trie<char, string>.Map("a", null);
        var result = Trie<char, string>.Lookup(null, trie);
        await Assert.That(result).IsEqualTo("a");
    }

    [Test]
    public async Task TrieLookupAaTest()
    {
        var list1 = new Trie<char, string>.Map("a", null, 'a', null);
        var list2 = new Trie<char, string>.Map(null, list1);
        var aNode = new FunList<char>.Node('a', null);
        var result = Trie<char, string>.Lookup(aNode, list2);
        await Assert.That(result).IsEqualTo("a");
    }

    [Test]
    public async Task TrieLookupAbTest()
    {
        var list1 = new Trie<char, string>.Map("a", null, 'a', null);
        var list2 = new Trie<char, string>.Map(null, list1, 'b', null);
        var list3 = new Trie<char, string>.Map(null, list2);
        var aNode = new FunList<char>.Node('b', new FunList<char>.Node('a', null));
        var result = Trie<char, string>.Lookup(aNode, list3);
        await Assert.That(result).IsEqualTo("a");
    }

    [Test]
    public async Task TrieLookupCTest()
    {
        var trie = Trie<char, string>.Empty;
        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);
        var findC = Trie<char, string>.Lookup(c, trie);
        await Assert.That(findC).IsEqualTo("C");
        await Assert.That(DumpMap(trie)).IsEqualTo("{'\0', {'C', , , \"C\"}, , \"\"}");
    }

    [Test]
    public async Task TrieLookupDTest()
    {
        var trie = Trie<char, string>.Empty;
        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);
        var d = ToList("D".ToCharArray());
        trie = Trie<char, string>.Bind(d, "D", trie);
        var findC = Trie<char, string>.Lookup(c, trie);
        await Assert.That(findC).IsEqualTo("C");
        var findD = Trie<char, string>.Lookup(d, trie);
        await Assert.That(findD).IsEqualTo("D");
        await Assert.That(DumpMap(trie)).IsEqualTo("{'\0', {'D', , {'C', , , \"C\"}, \"D\"}, , \"\"}");
    }

    [Test]
    public async Task TrieLookupBcTest()
    {
        var trie = Trie<char, string>.Empty;
        var cb = ToList("CB".ToCharArray());
        trie = Trie<char, string>.Bind(cb, "CB", trie);
        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);
        var findC = Trie<char, string>.Lookup(c, trie);
        await Assert.That(findC).IsEqualTo("C");
        var findCb = Trie<char, string>.Lookup(cb, trie);
        await Assert.That(findCb).IsEqualTo("CB");
        //await Assert.That(DumpMap(trie)).IsEqualTo("{'\0', {'C', {'B', , , \"CB\"}, {'C', , , \"C\"}, \"C\"}, , \"\"}");
        await Assert.That(DumpMap(trie)).IsEqualTo("{'\0', {'C', {'B', , , \"CB\"}, {'C', {'B', , , \"CB\"}, , \"\"}, \"C\"}, , \"\"}");
    }

    [Test]
    public async Task TrieLookupCbTest()
    {
        var trie = Trie<char, string>.Empty;
        var c = ToList("C".ToCharArray());
        trie = Trie<char, string>.Bind(c, "C", trie);
        var cb = ToList("CB".ToCharArray());
        trie = Trie<char, string>.Bind(cb, "CB", trie);
        var findC = Trie<char, string>.Lookup(c, trie);
        await Assert.That(findC).IsEqualTo("C");
        var findCb = Trie<char, string>.Lookup(cb, trie);
        await Assert.That(findCb).IsEqualTo("CB");
        await Assert.That(DumpMap(trie)).IsEqualTo("{'\0', {'C', {'B', , , \"CB\"}, {'C', , , \"C\"}, \"C\"}, , \"\"}");
    }

    [Test]
    public async Task TrieLookupDogTest()
    {
        var dog = ToList("DOG".ToCharArray());
        var trie = Trie<char, string>.Empty;
        trie = Trie<char, string>.Bind(dog, "Dog", trie);
        var findDog = Trie<char, string>.Lookup(dog, trie);
        await Assert.That(findDog).IsEqualTo("Dog");
    }

    [Test]
    public async Task TrieBindDogTest()
    {
        var trie1 = Trie<char, string>.Empty;
        var trie2 = Trie<char, string>.Bind(FunList<char>.Empty, "Dog", trie1);
        await Assert.That(trie2.V).IsEqualTo("Dog");
        // await Assert.That(trie2.M).IsSameReferenceAs(trie1);
        await Assert.That(trie2.Option).IsEqualTo('\0');
        await Assert.That(DumpMap(trie2)).IsEqualTo("{'\0', , , \"Dog\"}");
    }

    // Others
    [Test]
    public async Task Test()
    {
        var trie1 = Trie<char, string>.Empty;
        await Assert.That(DumpMap(trie1)).IsEqualTo("{'\0', , , \"\"}");
        var cart = ToList("CART".ToCharArray());
        var trie2 = Trie<char, string>.Bind(cart, "cart", trie1);
        await Assert.That(DumpMap(trie2)).IsEqualTo("{'\0', {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, , \"\"}");
        var car = ToList("CAR".ToCharArray());
        var trie3 = Trie<char, string>.Bind(car, "car", trie2);
        await Assert.That(DumpMap(trie3)).IsEqualTo("{'\0', {'C', {'A', {'R', {'T', , , \"cart\"}, {'R', {'T', , , \"cart\"}, , \"\"}, \"car\"}, {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, \"\"}, {'C', {'A', {'R', {'T', , , \"cart\"}, , \"\"}, , \"\"}, , \"\"}, \"\"}, , \"\"}");
        var dog = ToList("DOG".ToCharArray());
        var trie4 = Trie<char, string>.Bind(dog, "dog", trie3);
        var findCar = Trie<char, string>.Lookup(car, trie4);
        await Assert.That(findCar).IsEqualTo("car");
        var findDog = Trie<char, string>.Lookup(dog, trie4);
        await Assert.That(findDog).IsEqualTo("dog");
        var findCart = Trie<char, string>.Lookup(cart, trie4);
        await Assert.That(findCart).IsEqualTo("cart");
        Console.WriteLine(DumpMap(trie3));
    }
}