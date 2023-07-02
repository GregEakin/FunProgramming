// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.lists;
using FunProgLib.Utilities;

namespace FunProgTests.lists;

public class RListTests
{
    [Fact]
    public void IsEmptyTest()
    {
        var list = RList<string>.Empty;
        Assert.True(RList<string>.IsEmpty(list));
        list = RList<string>.Cons("A", list);
        Assert.False(RList<string>.IsEmpty(list));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = RList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RList<string>.Head(list));
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = RList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RList<string>.Tail(list));
    }

    [Fact]
    public void EnumeratorTest()
    {
        const string data = "a b c";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        Assert.Equal("[c, b, a]", list.ToReadableString());
    }

    [Fact]
    public void ReverseEmptyListTest()
    {
        var list = RList<string>.Reverse(RList<string>.Empty);
        Assert.True(RList<string>.IsEmpty(list));
    }

    [Fact]
    public void ReverseSingleListTest()
    {
        var list = RList<string>.Cons("Wow", RList<string>.Empty);
        var reverse = RList<string>.Reverse(list);
        Assert.Same(list, reverse);
    }

    [Fact]
    public void ReverseListTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var reverse = RList<string>.Reverse(list);
        Assert.Equal("[How, now,, brown, cow?]", reverse.ToReadableString());
    }

    [Fact]
    public void CatBothEmptyTest()
    {
        var list = RList<string>.Cat(RList<string>.Empty, RList<string>.Empty);
        Assert.True(RList<string>.IsEmpty(list));
    }

    [Fact]
    public void CatLeftEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var list2 = RList<string>.Cat(RList<string>.Empty, list);
        Assert.Same(list, list2);
    }

    [Fact]
    public void CatRightEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var list2 = RList<string>.Cat(list, RList<string>.Empty);
        Assert.Same(list, list2);
    }

    [Fact]
    public void CatTest()
    {
        const string data1 = "How now,";
        var list1 = data1.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        const string data2 = "brown cow?";
        var list2 = data2.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var list3 = RList<string>.Cat(list1, list2);
        Assert.Equal("[now,, How, cow?, brown]", list3.ToReadableString());
    }

    [Fact]
    public void LookupEmptyTest()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => RList<string>.Lookup(0, RList<string>.Empty));
        Assert.Equal("Value cannot be null. (Parameter 'list')", exception.Message);
    }

    [Fact]
    public void LookupNegativeTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var exception = Assert.Throws<ArgumentException>(() => RList<string>.Lookup(-1, list));
        Assert.Equal("neg (Parameter 'i')", exception.Message);
    }

    [Fact]
    public void LookupZeroTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var item = RList<string>.Lookup(0, list);
        Assert.Equal("cow?", item);
    }

    [Fact]
    public void LookupOneTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var item = RList<string>.Lookup(1, list);
        Assert.Equal("brown", item);
    }

    [Fact]
    public void UpdateEmptyTest()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => RList<string>.Fupdate(null, 0, RList<string>.Empty));
        Assert.Equal("Value cannot be null. (Parameter 'ts')", exception.Message);
    }

    [Fact]
    public void UpdateNegativeTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));

        var exception = Assert.Throws<ArgumentException>(() => RList<string>.Fupdate(null, -1, list));
        Assert.Equal("Negative (Parameter 'i')", exception.Message);
    }
}