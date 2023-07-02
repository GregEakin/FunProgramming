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

public class BinaryRandomAccessListTests
{
    [Fact]
    public void IsEmptyTest()
    {
        var list = BinaryRandomAccessList<string>.Empty;
        Assert.True(BinaryRandomAccessList<string>.IsEmpty(list));
        list = BinaryRandomAccessList<string>.Cons("A", list);
        Assert.False(BinaryRandomAccessList<string>.IsEmpty(list));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = BinaryRandomAccessList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BinaryRandomAccessList<string>.Head(list));
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = BinaryRandomAccessList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BinaryRandomAccessList<string>.Tail(list));
    }

    [Fact]
    public void LookupTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));

        Assert.Equal("now,", BinaryRandomAccessList<string>.Lookup(2, list));
    }

    [Fact]
    public void UpdateTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
        list = BinaryRandomAccessList<string>.Update(1, "green", list);
        Assert.Equal("green", BinaryRandomAccessList<string>.Lookup(1, list));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("cow?", BinaryRandomAccessList<string>.Head(list));

        list = BinaryRandomAccessList<string>.Update(0, "dog?", list);
        Assert.Equal("dog?", BinaryRandomAccessList<string>.Head(list));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
        list = BinaryRandomAccessList<string>.Tail(list);
        Assert.Equal("brown", BinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("now,", BinaryRandomAccessList<string>.Lookup(1, list));
        Assert.Equal("How", BinaryRandomAccessList<string>.Lookup(2, list));
    }

    [Fact]
    public void RoseTest()
    {
        const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var list = data.Split().Aggregate(BinaryRandomAccessList<string>.Empty, (current, word) => BinaryRandomAccessList<string>.Cons(word, current));
        Assert.Equal("sweet.", BinaryRandomAccessList<string>.Lookup(0, list));
        Assert.Equal("What's", BinaryRandomAccessList<string>.Lookup(17, list));
    }
}