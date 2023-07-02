// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//


using FunProgLib.lists;

namespace FunProgTests.lists;

public class AltBinaryRandomAccessListTests
{
    private static string DumpTree<T>(AltBinaryRandomAccessList<T>.DataType tree) where T : IComparable<T>
    {
        if (tree == null)
            return "null";

        if (tree is AltBinaryRandomAccessList<T>.Zero zero)
            return $"[Zero: {DumpList(zero.RList)}]";

        if (tree is AltBinaryRandomAccessList<T>.One one)
            return $"[One: {one.Alpha}, {DumpList(one.RList)}]";

        throw new ArgumentException();
    }

    private static string DumpList<T>(RList<Tuple<T, T>>.Node list)
    {
        var result = new StringBuilder();
        result.Append("{");
        var separator = "";
        while (true)
        {
            if (list == null) break;
            result.Append(separator);
            separator = ", ";
            var head = RList<Tuple<T, T>>.Head(list);
            result.Append(head);
            list = RList<Tuple<T, T>>.Tail(list);
        }
        result.Append("}");
        return result.ToString();
    }

    [Fact]
    public void DumpEmptyTree()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        Assert.Equal("null", DumpTree(list));
    }

    [Fact]
    public void DumpOddTree()
    {
        const string data = "a b c";
        var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("[One: c, {(b, a)}]", DumpTree(tree));
    }

    [Fact]
    public void DumpEvenTree()
    {
        const string data = "a b c d";
        var tree = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("[Zero: {(d, c), (b, a)}]", DumpTree(tree));
    }

    [Fact]
    public void IsEmptyTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        Assert.True(AltBinaryRandomAccessList<string>.IsEmpty(list));
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        Assert.False(AltBinaryRandomAccessList<string>.IsEmpty(list));
    }

    [Fact]
    public void ConsEmptyTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        Assert.Equal("[One: A, {}]", DumpTree(list));
    }

    [Fact]
    public void ConsOneTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        Assert.Equal("[Zero: {(B, A)}]", DumpTree(list));
    }

    [Fact]
    public void ConsTwoTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        list = AltBinaryRandomAccessList<string>.Cons("C", list);
        Assert.Equal("[One: C, {(B, A)}]", DumpTree(list));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = Assert.Throws<ArgumentException>(() => AltBinaryRandomAccessList<string>.Head(list));
        Assert.Equal("must be Zero or One (Parameter 'dataType')", ex.Message);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = Assert.Throws<ArgumentException>(() => AltBinaryRandomAccessList<string>.Tail(list));
        Assert.Equal("must be Zero or One (Parameter 'dataType')", ex.Message);
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("cow?", AltBinaryRandomAccessList<string>.Head(list));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        var tail = AltBinaryRandomAccessList<string>.Tail(list);
        Assert.Equal("[One: brown, {(now,, How)}]", DumpTree(tail));
    }

    [Fact]
    public void LookupNullTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        var ex = Assert.Throws<ArgumentException>(() => AltBinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("must be Zero or One (Parameter 'ts')", ex.Message);
    }

    [Fact]
    public void LookupSingleTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        Assert.Equal("A", AltBinaryRandomAccessList<string>.Lookup(0, list));
    }

    [Fact]
    public void LookupDoubleTest()
    {
        var list = AltBinaryRandomAccessList<string>.Empty;
        list = AltBinaryRandomAccessList<string>.Cons("A", list);
        list = AltBinaryRandomAccessList<string>.Cons("B", list);
        Assert.Equal("[Zero: {(B, A)}]", DumpTree(list));

        Assert.Equal("B", AltBinaryRandomAccessList<string>.Lookup(0, list));
    }

    [Fact]
    public void UpdateTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        list = AltBinaryRandomAccessList<string>.Update(1, "green", list);
        Assert.Equal("[Zero: {(cow?, green), (now,, How)}]", DumpTree(list));
        Assert.Equal("green", AltBinaryRandomAccessList<string>.Lookup(1, list));
    }

    [Fact]
    public void FUpdateOneTest()
    {
        const string data = "How now, cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 1, list);
        Assert.Equal("[One: cow?, {(now,-now,, How)}]", DumpTree(list));
        Assert.Equal("now,-now,", AltBinaryRandomAccessList<string>.Lookup(1, list));
    }

    [Fact]
    public void FUpdateZeroEvenTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 2, list);
        Assert.Equal("[Zero: {(cow?, brown), (now,-now,, How)}]", DumpTree(list));
        Assert.Equal("now,-now,", AltBinaryRandomAccessList<string>.Lookup(2, list));
    }

    [Fact]
    public void FUpdateZeroOddTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));

        list = AltBinaryRandomAccessList<string>.Fupdate(value => value + "-" + value, 1, list);
        Assert.Equal("[Zero: {(cow?, brown-brown), (now,, How)}]", DumpTree(list));
        Assert.Equal("brown-brown", AltBinaryRandomAccessList<string>.Lookup(1, list));
    }

    [Fact]
    public void RoseTest()
    {
        const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var list = data.Split().Aggregate(AltBinaryRandomAccessList<string>.Empty, (current, word) => AltBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("[Zero: {(sweet., as), (smell, would), (name, other), (any, by), (rose, a), (call, we), (which, That), (name?, a), (in, What's)}]", DumpTree(list));
        Assert.Equal("sweet.", AltBinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("What's", AltBinaryRandomAccessList<string>.Lookup(17, list));
    }

    [Fact]
    public void Test1()
    {
        var list = AltBinaryRandomAccessList<int>.Empty;
        for (var i = 0; i < 11; i++)
            list = AltBinaryRandomAccessList<int>.Cons(i, list);
        Assert.Equal("[One: 10, {(9, 8), (7, 6), (5, 4), (3, 2), (1, 0)}]", DumpTree(list));
    }
}