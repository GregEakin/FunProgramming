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
using FunProgLib.queue;
using FunProgLib.Utilities;

namespace FunProgTests.queue;

public class PhysicistQueueTests
{
    private static string DumpLazyList<T>(Lazy<FunList<T>.Node> lazyNode, bool expandUnCreated)
    {
        // TODO: check for lazyNode == null;

        if (!expandUnCreated && !lazyNode.IsValueCreated)
            return "$";

        var result = new StringBuilder();
        if (!lazyNode.IsValueCreated)
            result.Append('$');
        result.Append(lazyNode.Value?.ToReadableString() ?? "null");
        return result.ToString();
    }

    private static string DumpQueue<T>(PhysicistsQueue<T>.Queue queue, bool expandUnCreated)
    {
        if (queue == null)
            return string.Empty;
        var builder = new StringBuilder();
        builder.Append('[');
        builder.Append(queue.W?.ToReadableString() ?? "null");
        builder.Append(", ");
        builder.Append(queue.LenF);
        builder.Append(", ");
        builder.Append(DumpLazyList(queue.F, expandUnCreated));
        builder.Append(", ");
        builder.Append(queue.LenR);
        builder.Append(", ");
        builder.Append(queue.R?.ToReadableString() ?? "null");
        builder.Append(']');
        return builder.ToString();
    }

    [Test]
    public async Task Test1()
    {
        // pre-create the null in the empty value, to get the unit tests working
        var empty = PhysicistsQueue<string>.Empty;
        await Assert.That(empty.F.Value).IsNull();
        var queue = PhysicistsQueue<string>.Empty;
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[null, 0, null, 0, null]");
    }

    [Test]
    public async Task Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[[One], 3, $[One, Two, Three], 2, [Three, One]]");
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        await Assert.That(PhysicistsQueue<string>.IsEmpty(queue)).IsTrue();
        queue = PhysicistsQueue<string>.Snoc(queue, "Item");
        await Assert.That(PhysicistsQueue<string>.IsEmpty(queue)).IsFalse();
        queue = PhysicistsQueue<string>.Tail(queue);
        await Assert.That(PhysicistsQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptySnocTest()
    {
        var ex = await Assert.That(() => PhysicistsQueue<string>.Snoc(null, "Item")).Throws<NullReferenceException>();
        await Assert.That(ex.Message).IsEqualTo("Object reference not set to an instance of an object.");
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        queue = PhysicistsQueue<string>.Snoc(queue, "One");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[[One], 1, [One], 0, null]");
        queue = PhysicistsQueue<string>.Snoc(queue, "Two");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[[One], 1, [One], 1, [Two]]");
        queue = PhysicistsQueue<string>.Snoc(queue, "Three");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[[One], 3, $[One, Two, Three], 0, null]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        await Assert.That(() => PhysicistsQueue<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        var head = PhysicistsQueue<string>.Head(queue);
        await Assert.That(head).IsEqualTo("One");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = PhysicistsQueue<string>.Empty;
        await Assert.That(() => PhysicistsQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        var tail = PhysicistsQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(tail, true)).IsEqualTo("[[Two, Three], 2, [Two, Three], 2, [Three, One]]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(PhysicistsQueue<string>.Empty, PhysicistsQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var head = PhysicistsQueue<string>.Head(queue);
            await Assert.That(head).IsEqualTo(expected);
            queue = PhysicistsQueue<string>.Tail(queue);
        }

        await Assert.That(PhysicistsQueue<string>.IsEmpty(queue)).IsTrue();
    }
}