// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.tree;

namespace FunProgTests.tree;

public class RedBlackSetTests
{
    private static string DumpSet<T>(RedBlackSet<T>.Tree s) where T : IComparable<T>
    {
        if (s == RedBlackSet<T>.EmptyTree) return "\u2205";
        return DumpTree(s);
    }

    private static string DumpTree<T>(RedBlackSet<T>.Tree s) where T : IComparable<T>
    {
        var results = new StringBuilder();
        results.Append('(');
        results.Append(s.Color);
        results.Append(": ");

        if (s.Tree1 != RedBlackSet<T>.EmptyTree)
        {
            results.Append(DumpTree(s.Tree1));
            results.Append(' ');
        }

        results.Append(s.Elem);

        if (s.Tree2 != RedBlackSet<T>.EmptyTree)
        {
            results.Append(' ');
            results.Append(DumpTree(s.Tree2));
        }

        results.Append(')');
        return results.ToString();
    }

    [Fact]
    public void EmptyTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        Assert.Equal("∅", DumpSet(t));
    }

    [Fact]
    public void EmptyLeafTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        Assert.Equal("(B: C)", DumpSet(x1));
    }

    [Fact]
    public void DuplicateRootMemberTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        var x2 = RedBlackSet<string>.Insert("C", x1);
        Assert.Equal("(B: C)", DumpSet(x2));
        Assert.NotSame(x1, x2);
    }

    [Fact]
    public void DuplicateLeafMemberTest()
    {
        var empty = RedBlackSet<string>.EmptyTree;
        var a = RedBlackSet<string>.Insert("A", empty);
        var b = RedBlackSet<string>.Insert("B", a);
        var c1 = RedBlackSet<string>.Insert("C", b);
        Assert.Equal("(B: (B: A) B (B: C))", DumpSet(c1));
        var c2 = RedBlackSet<string>.Insert("C", c1);
        Assert.Equal("(B: (B: A) B (B: C))", DumpSet(c2));
        Assert.NotSame(c1, c2);
    }

    [Fact]
    public void MemberTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        var x2 = RedBlackSet<string>.Insert("B", x1);
        Assert.True(RedBlackSet<string>.Member("B", x2));
        Assert.False(RedBlackSet<string>.Member("A", x2));
        Assert.False(RedBlackSet<string>.Member("D", x2));
    }

    [Fact]
    public void BalanceTest1()
    {
        const string data = "z y x";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (B: x) y (B: z))", DumpSet(t));
    }

    [Fact]
    public void BalanceTest2()
    {
        const string data = "z x y";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (B: x) y (B: z))", DumpSet(t));
    }

    [Fact]
    public void BalanceTest3()
    {
        const string data = "x z y";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (B: x) y (B: z))", DumpSet(t));
    }

    [Fact]
    public void BalanceTest4()
    {
        const string data = "x y z";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (B: x) y (B: z))", DumpSet(t));
    }

    [Fact]
    public void BalanceTest5()
    {
        const string data = "y x z";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (R: x) y (R: z))", DumpSet(t));
    }

    [Fact]
    public void BalanceTest6()
    {
        const string data = "y z x";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        Assert.Equal("(B: (R: x) y (R: z))", DumpSet(t));
    }
}