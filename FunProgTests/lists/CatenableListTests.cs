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

public class CatenableListTests
{
    private static string DumpList<T>(CatenableList<T>.C list)
    {
        if (CatenableList<T>.IsEmpty(list))
            return "\u2205";

        var result = new StringBuilder();
        while(!CatenableList<T>.IsEmpty(list))
        {
            result.Append(CatenableList<T>.Head(list));
            list = CatenableList<T>.Tail(list);
            result.Append(", ");
        }
        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    [Fact]
    public void IsEmptyTest()
    {
        var list = CatenableList<string>.Empty;
        Assert.True(CatenableList<string>.IsEmpty(list));
        Assert.Equal("\u2205", DumpList(list));

        list = CatenableList<string>.Cons("A", list);
        Assert.False(CatenableList<string>.IsEmpty(list));
        Assert.Equal("A", DumpList(list));
    }

    [Fact]
    public void Cat1Test()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(list1, CatenableList<string>.Empty);
        Assert.Same(list1, list3);
    }

    [Fact]
    public void Cat2Test()
    {
        var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(CatenableList<string>.Empty, list2);
        Assert.Same(list2, list3);
    }

    [Fact]
    public void Cat3Test()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(list1, list2);
        Assert.Equal("c, b, a, z, y, x", DumpList(list3));
    }

    [Fact]
    public void ConsTest()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        Assert.Equal("c, b, a", DumpList(list1));
    }

    [Fact]
    public void SnocTest()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Snoc(current, word));
        Assert.Equal("a, b, c", DumpList(list1));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var list = CatenableList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => CatenableList<string>.Head(list));
    }

    [Fact]
    public void HeadTest()
    {
        const string head = "Head";
        var list0 = CatenableList<string>.Empty;
        var list1 = CatenableList<string>.Cons("Rest", list0);
        var list2 = CatenableList<string>.Cons(head, list1);
        Assert.Same(head, CatenableList<string>.Head(list2));
    }

    [Fact]
    public void EmptyTailTest()
    {
        var list = CatenableList<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => CatenableList<string>.Tail(list));
    }

    [Fact]
    public void TailTest()
    {
        var list0 = CatenableList<string>.Empty;
        var list1 = CatenableList<string>.Cons("Rest", list0);
        var list2 = CatenableList<string>.Cons("Head", list1);
        Assert.Same(list1, CatenableList<string>.Tail(list2));
    }
}