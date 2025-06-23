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

using FunProgLib.lists;

namespace FunProgTests.lists;

public class SkewBinaryRandomAccessListTests
{
    [Test]
    public async Task IsEmptyTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        await Assert.That(SkewBinaryRandomAccessList<string>.IsEmpty(list)).IsTrue();
        list = SkewBinaryRandomAccessList<string>.Cons("A", list);
        await Assert.That(SkewBinaryRandomAccessList<string>.IsEmpty(list)).IsFalse();
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        await Assert.That(() => SkewBinaryRandomAccessList<string>.Head(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var list = SkewBinaryRandomAccessList<string>.Empty;
        await Assert.That(() => SkewBinaryRandomAccessList<string>.Tail(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task LookupTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(2, list)).IsEqualTo("now,");
    }

    [Test]
    public async Task UpdateTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        list = SkewBinaryRandomAccessList<string>.Update(1, "green", list);
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(1, list)).IsEqualTo("green");
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(SkewBinaryRandomAccessList<string>.Head(list)).IsEqualTo("cow?");
        list = SkewBinaryRandomAccessList<string>.Update(0, "dog?", list);
        await Assert.That(SkewBinaryRandomAccessList<string>.Head(list)).IsEqualTo("dog?");
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        list = SkewBinaryRandomAccessList<string>.Tail(list);
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(0, list)).IsEqualTo("brown");
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(1, list)).IsEqualTo("now,");
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(2, list)).IsEqualTo("How");
    }

    [Test]
    public async Task RoseTest()
    {
        const string data = "What's in a name? That which we call a rose by any other name would smell as sweet.";
        var list = data.Split().Aggregate(SkewBinaryRandomAccessList<string>.Empty, (current, word) => SkewBinaryRandomAccessList<string>.Cons(word, current));
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(0, list)).IsEqualTo("sweet.");
        await Assert.That(SkewBinaryRandomAccessList<string>.Lookup(17, list)).IsEqualTo("What's");
    }
}