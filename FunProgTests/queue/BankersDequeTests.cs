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

using FunProgLib.queue;
using static FunProgTests.streams.StreamTests;

namespace FunProgTests.queue;

public class BankersDequeTests
{
    private static string DumpQueue<T>(BankersDeque<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = BankersDeque<string>.Empty;
        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsTrue();
        queue = BankersDeque<string>.Cons("Head", queue);
        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsFalse();
        queue = BankersDeque<string>.Tail(queue);
        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsTrue();
        queue = BankersDeque<string>.Snoc(queue, "Tail");
        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsFalse();
        queue = BankersDeque<string>.Init(queue);
        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task ConsTest()
    {
        var queue = BankersDeque<string>.Empty;
        queue = BankersDeque<string>.Cons("Last", queue);
        queue = BankersDeque<string>.Cons("Head", queue);
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$Head}, 1, {$Last}]");
    }

    [Test]
    public async Task ConsHeadTailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersDeque<string>.Empty, (queue1, s) => BankersDeque<string>.Cons(s, queue1));
        foreach (var expected in data.Split().Reverse())
        {
            var actual = BankersDeque<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = BankersDeque<string>.Tail(queue);
        }

        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = BankersDeque<string>.Empty;
        queue = BankersDeque<string>.Snoc(queue, "Head");
        queue = BankersDeque<string>.Snoc(queue, "Last");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$Head}, 1, {$Last}]");
    }

    [Test]
    public async Task SnocLastInitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersDeque<string>.Empty, BankersDeque<string>.Snoc);
        var dat = data.Split().Reverse();
        foreach (var expected in dat)
        {
            var actual = BankersDeque<string>.Last(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = BankersDeque<string>.Init(queue);
        }

        await Assert.That(BankersDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = BankersDeque<string>.Empty;
        await Assert.That(() => BankersDeque<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = BankersDeque<string>.Empty;
        await Assert.That(() => BankersDeque<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyLastTest()
    {
        var queue = BankersDeque<string>.Empty;
        await Assert.That(() => BankersDeque<string>.Last(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EmptyInitTest()
    {
        var queue = BankersDeque<string>.Empty;
        await Assert.That(() => BankersDeque<string>.Init(queue)).Throws<ArgumentNullException>();
    }
}