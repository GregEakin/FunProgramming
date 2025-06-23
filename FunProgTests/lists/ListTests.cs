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

using FunProgLib.Utilities;
using FunProgLib.lists;

namespace FunProgTests.lists;

public class ListTests
{
    [Test]
    public async Task IsEmptyTest()
    {
        var list = FunList<string>.Empty;
        await Assert.That(FunList<string>.IsEmpty(list)).IsTrue();
        list = FunList<string>.Cons("A", list);
        await Assert.That(FunList<string>.IsEmpty(list)).IsFalse();
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var list = FunList<string>.Empty;
        var exception = await Assert.That(() => FunList<string>.Head(list)).Throws<ArgumentNullException>();
        await Assert.That(exception.Message).IsEqualTo("Value cannot be null. (Parameter 'list')");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var list = FunList<string>.Empty;
        var exception = await Assert.That(() => FunList<string>.Tail(list)).Throws<ArgumentNullException>();
        await Assert.That(exception.Message).IsEqualTo("Value cannot be null. (Parameter 'list')");
    }

    [Test]
    public async Task EnumeratorTest()
    {
        const string data = "a b c";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        await Assert.That(list.ToReadableString()).IsEqualTo("[c, b, a]");
    }

    [Test]
    public async Task ReverseEmptyListTest()
    {
        var list = FunList<string>.Reverse(FunList<string>.Empty);
        await Assert.That(FunList<string>.IsEmpty(list)).IsTrue();
    }

    [Test]
    public async Task ReverseSingleListTest()
    {
        var list = FunList<string>.Cons("Wow", FunList<string>.Empty);
        var reverse = FunList<string>.Reverse(list);
        await Assert.That(reverse).IsSameReferenceAs(list);
    }

    [Test]
    public async Task ReverseListTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        var reverse = FunList<string>.Reverse(list);
        await Assert.That(reverse.ToReadableString()).IsEqualTo("[How, now,, brown, cow?]");
    }

    [Test]
    public async Task CatBothEmptyTest()
    {
        var list = FunList<string>.Cat(FunList<string>.Empty, FunList<string>.Empty);
        await Assert.That(FunList<string>.IsEmpty(list)).IsTrue();
    }

    [Test]
    public async Task CatLeftEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        var list2 = FunList<string>.Cat(FunList<string>.Empty, list);
        await Assert.That(list2).IsSameReferenceAs(list);
    }

    [Test]
    public async Task CatRightEmptyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        var list2 = FunList<string>.Cat(list, FunList<string>.Empty);
        await Assert.That(list2).IsSameReferenceAs(list);
    }

    [Test]
    public async Task CatTest()
    {
        const string data1 = "How now,";
        var list1 = data1.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        const string data2 = "brown cow?";
        var list2 = data2.Split().Aggregate(FunList<string>.Empty, (current, word) => FunList<string>.Cons(word, current));
        var list3 = FunList<string>.Cat(list1, list2);
        await Assert.That(list3.ToReadableString()).IsEqualTo("[now,, How, cow?, brown]");
    }

    [Test]
    public async Task FoldRightSumTest()
    {
        var data = new[]
        {
            1,
            2,
            3,
            4,
            5
        };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));
        var sum = FunList<int>.FoldRight(list, 0, (x, y) => x + y);
        await Assert.That(sum).IsEqualTo(15);
    }

    [Test]
    public async Task FoldLeftSumTest()
    {
        var data = new[]
        {
            1,
            2,
            3,
            4,
            5
        };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));
        var sum = FunList<int>.FoldLeft(list, 0, (x, y) => x + y);
        await Assert.That(sum).IsEqualTo(15);
    }

    [Test]
    public async Task FoldLeftRSumTest()
    {
        var data = new[]
        {
            1,
            2,
            3,
            4,
            5
        };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));
        var sum = FunList<int>.FoldLeftR(list, 0, (x, y) => x + y);
        await Assert.That(sum).IsEqualTo(15);
    }

    [Test]
    public async Task FoldRightLSumTest()
    {
        var data = new[]
        {
            1,
            2,
            3,
            4,
            5
        };
        var list = data.Aggregate(FunList<int>.Empty, (current, word) => FunList<int>.Cons(word, current));
        var sum = FunList<int>.FoldRightL(list, 0, (x, y) => x + y);
        await Assert.That(sum).IsEqualTo(15);
    }

    [Test]
    public async Task FoldRightProductTest()
    {
        var data = new[]
        {
            1.0,
            2.0,
            3.0,
            4.0,
            5.0
        };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));
        var product = FunList<double>.FoldRight(list, 1.0, (x, y) => x * y);
        await Assert.That(product).IsEqualTo(120.0);
    }

    [Test]
    public async Task FoldLeftProductTest()
    {
        var data = new[]
        {
            1.0,
            2.0,
            3.0,
            4.0,
            5.0
        };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));
        var product = FunList<double>.FoldLeft(list, 1.0, (x, y) => x * y);
        await Assert.That(product).IsEqualTo(120.0);
    }

    [Test]
    public async Task FoldLeftRProductTest()
    {
        var data = new[]
        {
            1.0,
            2.0,
            3.0,
            4.0,
            5.0
        };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));
        var product = FunList<double>.FoldLeftR(list, 1.0, (x, y) => x * y);
        await Assert.That(product).IsEqualTo(120.0);
    }

    [Test]
    public async Task FoldRightLProductTest()
    {
        var data = new[]
        {
            1.0,
            2.0,
            3.0,
            4.0,
            5.0
        };
        var list = data.Aggregate(FunList<double>.Empty, (current, word) => FunList<double>.Cons(word, current));
        var product = FunList<double>.FoldRightL(list, 1.0, (x, y) => x * y);
        await Assert.That(product).IsEqualTo(120.0);
    }
}