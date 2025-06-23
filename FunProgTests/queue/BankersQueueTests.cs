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

public class BankersQueueTests
{
    private static string DumpQueue<T>(BankersQueue<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
    }

    [Test]
    public async Task NullTest()
    {
        await Assert.That(() => BankersQueue<string>.IsEmpty(null)).Throws<NullReferenceException>();
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = BankersQueue<string>.Empty;
        await Assert.That(BankersQueue<string>.IsEmpty(queue)).IsTrue();
        queue = BankersQueue<string>.Snoc(queue, "Item");
        await Assert.That(BankersQueue<string>.IsEmpty(queue)).IsFalse();
        queue = BankersQueue<string>.Tail(queue);
        await Assert.That(BankersQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task NullSnocTest()
    {
        var ex = await Assert.That(() => BankersQueue<string>.Snoc(null, "one")).Throws<NullReferenceException>();
        await Assert.That(ex.Message).IsEqualTo("Object reference not set to an instance of an object.");
    }

    [Test]
    public async Task EmptySnocTest()
    {
        var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$one}, 0, {}]");
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = BankersQueue<string>.Snoc(BankersQueue<string>.Empty, "one");
        queue = BankersQueue<string>.Snoc(queue, "two");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$one}, 1, {$two}]");
    }

    [Test]
    public async Task NullHeadTest()
    {
        var ex = await Assert.That(() => BankersQueue<string>.Head(null)).Throws<NullReferenceException>();
        await Assert.That(ex.Message).IsEqualTo("Object reference not set to an instance of an object.");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = BankersQueue<string>.Empty;
        var ex = await Assert.That(() => BankersQueue<string>.Head(queue)).Throws<ArgumentNullException>();
        await Assert.That(ex.Message).IsEqualTo("Value cannot be null. (Parameter 'queue')");
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
        var item = BankersQueue<string>.Head(queue);
        await Assert.That(item).IsEqualTo("One");
    }

    [Test]
    public async Task NullTailTest()
    {
        var ex = await Assert.That(() => BankersQueue<string>.Tail(null)).Throws<NullReferenceException>();
        await Assert.That(ex.Message).IsEqualTo("Object reference not set to an instance of an object.");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = BankersQueue<string>.Empty;
        var ex = await Assert.That(() => BankersQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
        await Assert.That(ex.Message).IsEqualTo("Value cannot be null. (Parameter 'queue')");
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
        var tail = BankersQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(tail, true)).IsEqualTo("[2, {$Two, $Three}, 2, {$Three, $One}]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersQueue<string>.Empty, BankersQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var actual = BankersQueue<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = BankersQueue<string>.Tail(queue);
        }

        await Assert.That(BankersQueue<string>.IsEmpty(queue)).IsTrue();
    }
}