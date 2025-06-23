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

public class UnbalancedSetTests
{
    private static string DumpTree<T>(UnbalancedSet<T>.Tree tree)
        where T : IComparable<T>
    {
        if (tree == UnbalancedSet<T>.Empty)
            return "\u2205";
        var results = new StringBuilder();

        results.Append('[');
        if (tree.A != UnbalancedSet<T>.Empty)
        {
            results.Append(DumpTree(tree.A));
            results.Append(',');
        }

        results.Append(tree.Y);

        if (tree.B != UnbalancedSet<T>.Empty)
        {
            results.Append(',');
            results.Append(DumpTree(tree.B));
        }
        results.Append(']');

        return results.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var tree = UnbalancedSet<string>.Empty;
        await Assert.That(DumpTree(tree)).IsEqualTo("\u2205");
    }

    [Test]
    public async Task SingleElementTest()
    {
        var tree = UnbalancedSet<string>.Empty;
        tree = UnbalancedSet<string>.Insert("a", tree);
        await Assert.That(DumpTree(tree)).IsEqualTo("[a]");
    }

    [Test]
    public async Task DuplicateElementTest()
    {
        var tree = UnbalancedSet<string>.Empty;
        tree = UnbalancedSet<string>.Insert("a", tree);
        tree = UnbalancedSet<string>.Insert("a", tree);
        await Assert.That(DumpTree(tree)).IsEqualTo("[a]");
    }

    [Test]
    public async Task DumpTreeTest()
    {
        const string data = "How now, brown cow?";
        var tree = data.Split().Aggregate(UnbalancedSet<string>.Empty, (current, word) => UnbalancedSet<string>.Insert(word, current));
        await Assert.That(DumpTree(tree)).IsEqualTo("[[brown,[cow?]],How,[now,]]");
    }

    [Test]
    public async Task ElementTest()
    {
        const string data = "How now, brown cow?";
        var tree = data.Split().Aggregate(UnbalancedSet<string>.Empty, (current, word) => UnbalancedSet<string>.Insert(word, current));
        foreach (var word in data.Split())
            await Assert.That(UnbalancedSet<string>.Member(word, tree)).IsTrue();
        await Assert.That(UnbalancedSet<string>.Member("wow", tree)).IsFalse();
    }
}