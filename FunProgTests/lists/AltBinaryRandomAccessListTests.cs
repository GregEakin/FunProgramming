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

namespace FunProgTests.lists;

public class AltBinaryRandomAccessListTests
{
    private static string DumpTree<T>(AltBinaryRandomAccessList<T>.DataType tree)
        where T : IComparable<T>
    {
        return tree switch
        {
            null => "null",
            AltBinaryRandomAccessList<T>.Zero zero => $"[Zero: {DumpList(zero.RList)}]",
            AltBinaryRandomAccessList<T>.One one => $"[One: {one.Alpha}, {DumpList(one.RList)}]",
            _ => throw new ArgumentException("Unknown tree type element.", nameof(tree))};
    }

    private static string DumpList<T>(RList<Tuple<T, T>>.Node list)
    {
        var result = new StringBuilder();
        result.Append('{');
        var separator = string.Empty;
        while (true)
        {
            if (list == null)
                break;
            result.Append(separator);
            separator = ", ";
            var head = RList<Tuple<T, T>>.Head(list);
            result.Append(head);
            list = RList<Tuple<T, T>>.Tail(list);
        }

        result.Append('}');
        return result.ToString();
    }

    [Test]
    public async Task DumpEmptyTree()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        await Assert.That(DumpTree(list)).IsEqualTo("null");
    }

    [Test]
    public async Task DumpOddTree()
    {
        const string data = "a b c";
        var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(DumpTree(tree)).IsEqualTo("[One: c, {(b, a)}]");
    }

    [Test]
    public async Task DumpEvenTree()
    {
        const string data = "a b c d";
        var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(DumpTree(tree)).IsEqualTo("[Zero: {(d, c), (b, a)}]");
    }

    [Test]
    public async Task IsEmptyTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        await Assert.That(AltBinaryRandomAccessList<string>.IsEmpty(list)).IsTrue();
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        await Assert.That(AltBinaryRandomAccessList<string>.IsEmpty(list)).IsFalse();
    }

    [Test]
    public async Task ConsEmptyTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        await Assert.That(DumpTree(list)).IsEqualTo("[One: A, {}]");
    }

    [Test]
    public async Task ConsOneTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(B, A)}]");
    }

    [Test]
    public async Task ConsTwoTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        list = AltBinaryRandomAccessList<string>.Cons("C", list);
        await Assert.That(DumpTree(list)).IsEqualTo("[One: C, {(B, A)}]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = await Assert.That(() => AltBinaryRandomAccessList<string>.Head(list)).Throws<ArgumentException>();
        await Assert.That(ex.Message).IsEqualTo("must be Zero or One (Parameter 'dataType')");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = await Assert.That(() => AltBinaryRandomAccessList<string>.Tail(list)).Throws<ArgumentException>();
        await Assert.That(ex.Message).IsEqualTo("must be Zero or One (Parameter 'dataType')");
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(AltBinaryRandomAccessList<string>.Head(list)).IsEqualTo("cow?");
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        var tail = AltBinaryRandomAccessList<string>.Tail(list);
        await Assert.That(DumpTree(tail)).IsEqualTo("[One: brown, {(now,, How)}]");
    }

    [Test]
    public async Task LookupNullTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = await Assert.That(() => AltBinaryRandomAccessList<string>.Lookup(0, list)).Throws<ArgumentException>();
        await Assert.That(ex.Message).IsEqualTo("must be Zero or One (Parameter 'ts')");
    }

    [Test]
    public async Task LookupSingleTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(0, list)).IsEqualTo("A");
    }

    [Test]
    public async Task LookupDoubleTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(B, A)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(0, list)).IsEqualTo("B");
    }

    [Test]
    public async Task UpdateTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        list = AltBinaryRandomAccessList<string>.Update(1, "green", list);
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(cow?, green), (now,, How)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(1, list)).IsEqualTo("green");
    }

    [Test]
    public async Task FUpdateOneTest()
    {
        const string data = "How now, cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 1, list);
        await Assert.That(DumpTree(list)).IsEqualTo("[One: cow?, {(now,-now,, How)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(1, list)).IsEqualTo("now,-now,");
    }

    [Test]
    public async Task FUpdateZeroEvenTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 2, list);
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(cow?, brown), (now,-now,, How)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(2, list)).IsEqualTo("now,-now,");
    }

    [Test]
    public async Task FUpdateZeroOddTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 1, list);
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(cow?, brown-brown), (now,, How)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(1, list)).IsEqualTo("brown-brown");
    }

    [Test]
    public async Task RoseTest()
    {
        const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(DumpTree(list)).IsEqualTo("[Zero: {(sweet., as), (smell, would), (name, other), (any, by), (rose, a), (call, we), (which, That), (name?, a), (in, What's)}]");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(0, list)).IsEqualTo("sweet.");
        await Assert.That(AltBinaryRandomAccessList<string>.Lookup(17, list)).IsEqualTo("What's");
    }

    [Test]
    public async Task Test1()
    {
        var list = AltBinaryRandomAccessList<int>.Empty;
        for (var i = 0; i < 11; i++)
            list = AltBinaryRandomAccessList<int>.Cons(i, list);
        await Assert.That(DumpTree(list)).IsEqualTo("[One: 10, {(9, 8), (7, 6), (5, 4), (3, 2), (1, 0)}]");
    }
}