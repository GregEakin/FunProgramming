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
using FunProgLib.Utilities;

namespace FunProgTests.lists;

public class RListTests
{
    [Test]
    public async Task IsEmptyTest()
    {
        var list = RList<string>.Empty;
        await Assert.That(RList<string>.IsEmpty(list)).IsTrue();
        list = RList<string>.Cons("A", list);
        await Assert.That(RList<string>.IsEmpty(list)).IsFalse();
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var list = RList<string>.Empty;
        await Assert.That(() => RList<string>.Head(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var list = RList<string>.Empty;
        await Assert.That(() => RList<string>.Tail(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EnumeratorTest()
    {
        const string data = "a b c";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        await Assert.That(list.ToReadableString()).IsEqualTo(list.ToReadableString());
    }

    [Test]
    public async Task ReverseEmptyListTest()
    {
        var list = RList<string>.Reverse(RList<string>.Empty);
        await Assert.That(RList<string>.IsEmpty(list)).IsTrue();
    }

    [Test]
    public async Task ReverseSingleListTest()
    {
        var list = RList<string>.Cons("Wow", RList<string>.Empty);
        var reverse = RList<string>.Reverse(list);
        await Assert.That(reverse).IsSameReferenceAs(list);
    }

    [Test]
    public async Task ReverseListTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var reverse = RList<string>.Reverse(list);
        await Assert.That(reverse.ToReadableString()).IsEqualTo(reverse.ToReadableString());
    }

    [Test]
    public async Task CatBothEmptyTest()
    {
        var list = RList<string>.Cat(RList<string>.Empty, RList<string>.Empty);
        await Assert.That(RList<string>.IsEmpty(list)).IsTrue();
    }

    [Test]
    public async Task CatLeftEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var list2 = RList<string>.Cat(RList<string>.Empty, list);
        await Assert.That(list2).IsSameReferenceAs(list);
    }

    [Test]
    public async Task CatRightEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var list2 = RList<string>.Cat(list, RList<string>.Empty);
        await Assert.That(list2).IsSameReferenceAs(list);
    }

    [Test]
    public async Task CatTest()
    {
        const string data1 = "How now,";
        var list1 = data1.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        const string data2 = "brown cow?";
        var list2 = data2.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var list3 = RList<string>.Cat(list1, list2);
        await Assert.That(list3.ToReadableString()).IsEqualTo(list3.ToReadableString());
    }

    [Test]
    public async Task LookupEmptyTest()
    {
        var exception = await Assert.That(() => RList<string>.Lookup(0, RList<string>.Empty)).Throws<ArgumentNullException>();
        await Assert.That(exception.Message).IsEqualTo("Value cannot be null. (Parameter 'list')");
    }

    [Test]
    public async Task LookupNegativeTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var exception = await Assert.That(() => RList<string>.Lookup(-1, list)).Throws<ArgumentException>();
        await Assert.That(exception.Message).IsEqualTo("neg (Parameter 'i')");
    }

    [Test]
    public async Task LookupZeroTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var item = RList<string>.Lookup(0, list);
        await Assert.That(item).IsEqualTo("cow?");
    }

    [Test]
    public async Task LookupOneTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var item = RList<string>.Lookup(1, list);
        await Assert.That(item).IsEqualTo("brown");
    }

    [Test]
    public async Task UpdateEmptyTest()
    {
        var exception = await Assert.That(() => RList<string>.Fupdate(null, 0, RList<string>.Empty)).Throws<ArgumentNullException>();
        await Assert.That(exception.Message).IsEqualTo("Value cannot be null. (Parameter 'ts')");
    }

    [Test]
    public async Task UpdateNegativeTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(RList<string>.Empty, (current, word) => RList<string>.Cons(word, current));
        var exception = await Assert.That(() => RList<string>.Fupdate(null, -1, list)).Throws<ArgumentException>();
        await Assert.That(exception.Message).IsEqualTo("Negative (Parameter 'i')");
    }
}