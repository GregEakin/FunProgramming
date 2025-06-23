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

using FunProgLib.tree;

namespace FunProgTests.tree;

public class RedBlackSetTests
{
    private static string DumpSet<T>(RedBlackSet<T>.Tree s)
        where T : IComparable<T>
    {
        if (s == RedBlackSet<T>.EmptyTree)
            return "\u2205";
        return DumpTree(s);
    }

    private static string DumpTree<T>(RedBlackSet<T>.Tree s)
        where T : IComparable<T>
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

    [Test]
    public async Task EmptyTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        await Assert.That(DumpSet(t)).IsEqualTo("\u2205");
    }

    [Test]
    public async Task EmptyLeafTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        await Assert.That(DumpSet(x1)).IsEqualTo("(B: C)");
    }

    [Test]
    public async Task DuplicateRootMemberTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        var x2 = RedBlackSet<string>.Insert("C", x1);
        await Assert.That(DumpSet(x2)).IsEqualTo("(B: C)");
        await Assert.That(x2).IsNotSameReferenceAs(x1);
    }

    [Test]
    public async Task DuplicateLeafMemberTest()
    {
        var empty = RedBlackSet<string>.EmptyTree;
        var a = RedBlackSet<string>.Insert("A", empty);
        var b = RedBlackSet<string>.Insert("B", a);
        var c1 = RedBlackSet<string>.Insert("C", b);
        await Assert.That(DumpSet(c1)).IsEqualTo("(B: (B: A) B (B: C))");
        var c2 = RedBlackSet<string>.Insert("C", c1);
        await Assert.That(DumpSet(c2)).IsEqualTo("(B: (B: A) B (B: C))");
        await Assert.That(c2).IsNotSameReferenceAs(c1);
    }

    [Test]
    public async Task MemberTest()
    {
        var t = RedBlackSet<string>.EmptyTree;
        var x1 = RedBlackSet<string>.Insert("C", t);
        var x2 = RedBlackSet<string>.Insert("B", x1);
        await Assert.That(RedBlackSet<string>.Member("B", x2)).IsTrue();
        await Assert.That(RedBlackSet<string>.Member("A", x2)).IsFalse();
        await Assert.That(RedBlackSet<string>.Member("D", x2)).IsFalse();
    }

    [Test]
    public async Task BalanceTest1()
    {
        const string data = "z y x";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (B: x) y (B: z))");
    }

    [Test]
    public async Task BalanceTest2()
    {
        const string data = "z x y";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (B: x) y (B: z))");
    }

    [Test]
    public async Task BalanceTest3()
    {
        const string data = "x z y";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (B: x) y (B: z))");
    }

    [Test]
    public async Task BalanceTest4()
    {
        const string data = "x y z";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (B: x) y (B: z))");
    }

    [Test]
    public async Task BalanceTest5()
    {
        const string data = "y x z";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (R: x) y (R: z))");
    }

    [Test]
    public async Task BalanceTest6()
    {
        const string data = "y z x";
        var t = data.Split().Aggregate(RedBlackSet<string>.EmptyTree, (current, word) => RedBlackSet<string>.Insert(word, current));
        await Assert.That(DumpSet(t)).IsEqualTo("(B: (R: x) y (R: z))");
    }
}