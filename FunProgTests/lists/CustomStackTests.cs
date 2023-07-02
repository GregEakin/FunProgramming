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

public class CustomStackTests
{
    [Fact]
    public void EmptyTest()
    {
        var stack = CustomStack<string>.Empty;
        Assert.True(CustomStack<string>.IsEmpty(stack));
    }

    [Fact]
    public void NotEmptyTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        Assert.False(CustomStack<string>.IsEmpty(stack));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var stack = CustomStack<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => CustomStack<string>.Head(stack));
    }

    [Fact]
    public void HeadTest()
    {
        var stack = CustomStack<string>.Cons("Hello", CustomStack<string>.Empty);
        var head = CustomStack<string>.Head(stack);
        Assert.Equal("Hello", head);
    }

    [Fact]
    public void TailTest()
    {
        var empty = CustomStack<string>.Empty;
        var hello = CustomStack<string>.Cons("Hello", empty);
        var world = CustomStack<string>.Cons("World", hello);
        var stack = CustomStack<string>.Tail(world);
        Assert.Equal(hello, stack);
    }
}