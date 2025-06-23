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

public class BatchedQueueTests
{
    private static string DumpQueue<T>(BatchedQueue<T>.Queue queue)
    {
        var builder = new StringBuilder();
        builder.Append('[');
        builder.Append(queue.F?.ToReadableString() ?? "null");
        builder.Append(", ");
        builder.Append(queue.R?.ToReadableString() ?? "null");
        builder.Append(']');
        return builder.ToString();
    }

    [Test]
    public async Task Test1()
    {
        var queue = BatchedQueue<string>.Empty;
        await Assert.That(DumpQueue(queue)).IsEqualTo("[null, null]");
    }

    [Test]
    public async Task Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        await Assert.That(DumpQueue(queue)).IsEqualTo("[[One], [Three, One, Three, Two]]");
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = BatchedQueue<string>.Empty;
        await Assert.That(BatchedQueue<string>.IsEmpty(queue)).IsTrue();
        queue = BatchedQueue<string>.Snoc(queue, "Item");
        await Assert.That(BatchedQueue<string>.IsEmpty(queue)).IsFalse();
        queue = BatchedQueue<string>.Tail(queue);
        await Assert.That(BatchedQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptySnocTest()
    {
        await Assert.That(() => BatchedQueue<string>.Snoc(null, "Item")).Throws<NullReferenceException>();
    }

    [Test]
    public async Task SnocTest()
    {
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = BatchedQueue<string>.Empty;
        await Assert.That(() => BatchedQueue<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var head = BatchedQueue<string>.Head(queue);
        await Assert.That(head).IsEqualTo("One");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = BatchedQueue<string>.Empty;
        await Assert.That(() => BatchedQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var tail = BatchedQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(tail)).IsEqualTo("[[Two, Three, One, Three], null]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var actual = BatchedQueue<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = BatchedQueue<string>.Tail(queue);
        }

        await Assert.That(BatchedQueue<string>.IsEmpty(queue)).IsTrue();
    }
}