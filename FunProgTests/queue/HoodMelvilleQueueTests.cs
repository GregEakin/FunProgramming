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
using FunProgLib.Utilities;

namespace FunProgTests.queue;

public class HoodMelvilleQueueTests
{
    private static string DumpQueue<T>(HoodMelvilleQueue<T>.Queue queue)
    {
        if (queue == null)
            return "null";

        var result = new StringBuilder();
        result.Append('[');
        result.Append(queue.LenF);
        result.Append(", ");
        result.Append(queue.F?.ToReadableString() ?? "null");
        // result.Append(", ");
        // result.Append(queue.State.GetType());
        result.Append(", ");
        result.Append(queue.LenR);
        result.Append(", ");
        result.Append(queue.R?.ToReadableString() ?? "null");
        result.Append(']');
        return result.ToString();
    }

    [Test]
    public async Task Test1()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        await Assert.That(DumpQueue(queue)).IsEqualTo("[0, null, 0, null]");
    }

    [Test]
    public async Task Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        await Assert.That(DumpQueue(queue)).IsEqualTo("[3, [One, Two, Three], 2, [Three, One]]");
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        await Assert.That(HoodMelvilleQueue<string>.IsEmpty(queue)).IsTrue();
        queue = HoodMelvilleQueue<string>.Snoc(queue, "Item");
        await Assert.That(HoodMelvilleQueue<string>.IsEmpty(queue)).IsFalse();
        queue = HoodMelvilleQueue<string>.Tail(queue);
        await Assert.That(HoodMelvilleQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task SnocEmptyTest()
    {
        await Assert.That(() => HoodMelvilleQueue<string>.Snoc(null, "Item")).Throws<NullReferenceException>();
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        queue = HoodMelvilleQueue<string>.Snoc(queue, "One");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[1, [One], 0, null]");
        queue = HoodMelvilleQueue<string>.Snoc(queue, "Two");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[1, [One], 1, [Two]]");
        queue = HoodMelvilleQueue<string>.Snoc(queue, "Three");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[3, [One], 0, null]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        await Assert.That(() => HoodMelvilleQueue<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        var head = HoodMelvilleQueue<string>.Head(queue);
        await Assert.That(head).IsEqualTo("One");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = HoodMelvilleQueue<string>.Empty;
        await Assert.That(() => HoodMelvilleQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        var tail = HoodMelvilleQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(tail)).IsEqualTo("[2, [Two, Three], 2, [Three, One]]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(HoodMelvilleQueue<string>.Empty, HoodMelvilleQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var actual = HoodMelvilleQueue<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = HoodMelvilleQueue<string>.Tail(queue);
        }

        await Assert.That(HoodMelvilleQueue<string>.IsEmpty(queue)).IsTrue();
    }
}