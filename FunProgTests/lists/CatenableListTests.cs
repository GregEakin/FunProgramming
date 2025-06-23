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

public class CatenableListTests
{
    private static string DumpList<T>(CatenableList<T>.C list)
    {
        if (CatenableList<T>.IsEmpty(list))
            return "\u2205";

        var result = new StringBuilder();
        while (!CatenableList<T>.IsEmpty(list))
        {
            result.Append(CatenableList<T>.Head(list));
            list = CatenableList<T>.Tail(list);
            result.Append(", ");
        }

        result.Remove(result.Length - 2, 2);
        return result.ToString();
    }

    [Test]
    public async Task IsEmptyTest()
    {
        var list = CatenableList<string>.Empty;
        await Assert.That(CatenableList<string>.IsEmpty(list)).IsTrue();
        await Assert.That(DumpList(list)).IsEqualTo("\u2205");
        list = CatenableList<string>.Cons("A", list);
        await Assert.That(CatenableList<string>.IsEmpty(list)).IsFalse();
        await Assert.That(DumpList(list)).IsEqualTo("A");
    }

    [Test]
    public async Task Cat1Test()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(list1, CatenableList<string>.Empty);
        await Assert.That(list3).IsSameReferenceAs(list1);
    }

    [Test]
    public async Task Cat2Test()
    {
        var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(CatenableList<string>.Empty, list2);
        await Assert.That(list3).IsSameReferenceAs(list2);
    }

    [Test]
    public async Task Cat3Test()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list2 = "x y z".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        var list3 = CatenableList<string>.Cat(list1, list2);
        await Assert.That(DumpList(list3)).IsEqualTo("c, b, a, z, y, x");
    }

    [Test]
    public async Task ConsTest()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Cons(word, current));
        await Assert.That(DumpList(list1)).IsEqualTo("c, b, a");
    }

    [Test]
    public async Task SnocTest()
    {
        var list1 = "a b c".Split().Aggregate(CatenableList<string>.Empty, (current, word) => CatenableList<string>.Snoc(current, word));
        await Assert.That(DumpList(list1)).IsEqualTo("a, b, c");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var list = CatenableList<string>.Empty;
        await Assert.That(() => CatenableList<string>.Head(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string head = "Head";
        var list0 = CatenableList<string>.Empty;
        var list1 = CatenableList<string>.Cons("Rest", list0);
        var list2 = CatenableList<string>.Cons(head, list1);
        await Assert.That(CatenableList<string>.Head(list2)).IsSameReferenceAs(head);
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var list = CatenableList<string>.Empty;
        await Assert.That(() => CatenableList<string>.Tail(list)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        var list0 = CatenableList<string>.Empty;
        var list1 = CatenableList<string>.Cons("Rest", list0);
        var list2 = CatenableList<string>.Cons("Head", list1);
        await Assert.That(CatenableList<string>.Tail(list2)).IsSameReferenceAs(list1);
    }
}