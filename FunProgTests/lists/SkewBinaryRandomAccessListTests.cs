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

public class SkewBinaryRandomAccessListTests
{
    [Fact]
    public void IsEmptyTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        Assert.True(SkewBinaryRandomAccessList<string>.IsEmpty(list));
        list = SkewBinaryRandomAccessList<string>.Cons("A", list);
        Assert.False(SkewBinaryRandomAccessList<string>.IsEmpty(list));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => SkewBinaryRandomAccessList<string>.Head(list));
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => SkewBinaryRandomAccessList<string>.Tail(list));
    }

    [Fact]
    public void LookupTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));

        Assert.Equal("now,", SkewBinaryRandomAccessList<string>.Lookup(2, list));
    }

    [Fact]
    public void UpdateTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        list = SkewBinaryRandomAccessList<string>.Update(1, "green", list);
        Assert.Equal("green", SkewBinaryRandomAccessList<string>.Lookup(1, list));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("cow?", SkewBinaryRandomAccessList<string>.Head(list));

        list = SkewBinaryRandomAccessList<string>.Update(0, "dog?", list);
        Assert.Equal("dog?", SkewBinaryRandomAccessList<string>.Head(list));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        list = SkewBinaryRandomAccessList<string>.Tail(list);
        Assert.Equal("brown", SkewBinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("now,", SkewBinaryRandomAccessList<string>.Lookup(1, list));
        Assert.Equal("How", SkewBinaryRandomAccessList<string>.Lookup(2, list));
    }

    [Fact]
    public void RoseTest()
    {
        const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("sweet.", SkewBinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("What's", SkewBinaryRandomAccessList<string>.Lookup(17, list));
    }
}