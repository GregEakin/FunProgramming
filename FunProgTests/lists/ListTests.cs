// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;
using FunProgLib.lists;

namespace FunProgTests.lists;

public class ListTests
{
    [Fact]
    public void IsEmptyTest()
    {
        var list = FunList<string>.Empty;
        Assert.True(FunList<string>.IsEmpty(list));
        list = FunList<string>.Cons("A", list);
        Assert.False(FunList<string>.IsEmpty(list));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = FunList<string>.Empty;
        var exception = Assert.Throws<ArgumentNullException>(() => FunList<string>.Head(list));
        Assert.Equal("Value cannot be null. (Parameter 'list')", exception.Message);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = FunList<string>.Empty;
        var exception = Assert.Throws<ArgumentNullException>(() => FunList<string>.Tail(list));
        Assert.Equal("Value cannot be null. (Parameter 'list')", exception.Message);
    }

    [Fact]
    public void EnumeratorTest()
    {
        const string data = "a b c";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        Assert.Equal("[c, b, a]", list.ToReadableString());
    }

    [Fact]
    public void ReverseEmptyListTest()
    {
        var list = FunList<string>.Reverse(FunList<string>.Empty);
        Assert.True(FunList<string>.IsEmpty(list));
    }

    [Fact]
    public void ReverseSingleListTest()
    {
        var list = FunList<string>.Cons("Wow", FunList<string>.Empty);
        var reverse = FunList<string>.Reverse(list);
        Assert.Same(list, reverse);
    }

    [Fact]
    public void ReverseListTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        var reverse = FunList<string>.Reverse(list);
        Assert.Equal("[How, now,, brown, cow?]", reverse.ToReadableString());
    }

    [Fact]
    public void CatBothEmptyTest()
    {
        var list = FunList<string>.Cat(FunList<string>.Empty, FunList<string>.Empty);
        Assert.True(FunList<string>.IsEmpty(list));
    }

    [Fact]
    public void CatLeftEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));

        var list2 = FunList<string>.Cat(FunList<string>.Empty, list);
        Assert.Same(list, list2);
    }

    [Fact]
    public void CatRightEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));

        var list2 = FunList<string>.Cat(list, FunList<string>.Empty);
        Assert.Same(list, list2);
    }

    [Fact]
    public void CatTest()
    {
        const string data1 = "How now,";
        var list1 = data1.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));

        const string data2 = "brown cow?";
        var list2 = data2.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));

        var list3 = FunList<string>.Cat(list1, list2);
        Assert.Equal("[now,, How, cow?, brown]", list3.ToReadableString());
    }

    [Fact]
    public void FoldRightSumTest()
    {
        var data = new[]{ 1, 2, 3, 4, 5 };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));

        var sum = FunList<int>.FoldRight(list, 0, (x, y) => x + y);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void FoldLeftSumTest()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));

        var sum = FunList<int>.FoldLeft(list, 0, (x, y) => x + y);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void FoldLeftRSumTest()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));

        var sum = FunList<int>.FoldLeftR(list, 0, (x, y) => x + y);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void FoldRightLSumTest()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));

        var sum = FunList<int>.FoldRightL(list, 0, (x, y) => x + y);
        Assert.Equal(15, sum);
    }

    [Fact]
    public void FoldRightProductTest()
    {
        var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));

        var product = FunList<double>.FoldRight(list, 1.0, (x, y) => x * y);
        Assert.Equal(120.0, product);
    }

    [Fact]
    public void FoldLeftProductTest()
    {
        var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));

        var product = FunList<double>.FoldLeft(list, 1.0, (x, y) => x * y);
        Assert.Equal(120.0, product);
    }

    [Fact]
    public void FoldLeftRProductTest()
    {
        var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));

        var product = FunList<double>.FoldLeftR(list, 1.0, (x, y) => x * y);
        Assert.Equal(120.0, product);
    }

    [Fact]
    public void FoldRightLProductTest()
    {
        var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));

        var product = FunList<double>.FoldRightL(list, 1.0, (x, y) => x * y);
        Assert.Equal(120.0, product);
    }
}