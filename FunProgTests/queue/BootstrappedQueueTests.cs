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

namespace FunProgTests.queue;

public class BootstrappedQueueTests
{
    private static string DumpQueue<T>(BootstrappedQueue<T>.Queue queue)
    {
        return queue == null 
            ? "null" 
            : $"[{queue.LenFM}, {DumpList(queue.F)}, {DumpQueue(queue.M)}, {queue.LenR}, {DumpList(queue.R)}]";
    }

    private static string DumpList<T>(FunList<T>.Node list)
    {
        var result = new StringBuilder();
        result.Append('{');
        var separator = string.Empty;
        while (true)
        {
            if (list == null)
                break;
            result.Append(separator);
            separator = ", ";
            var head = FunList<T>.Head(list);
            result.Append(head);
            list = FunList<T>.Tail(list);
        }

        result.Append('}');
        return result.ToString();
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        await Assert.That(BootstrappedQueue<string>.IsEmpty(queue)).IsTrue();
        queue = BootstrappedQueue<string>.Snoc(queue, "Item");
        await Assert.That(BootstrappedQueue<string>.IsEmpty(queue)).IsFalse();
        queue = BootstrappedQueue<string>.Tail(queue);
        await Assert.That(BootstrappedQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptySnocTest()
    {
        var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[1, {one}, null, 0, {}]");
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        queue = BootstrappedQueue<string>.Snoc(queue, "One");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[1, {One}, null, 0, {}]");
        queue = BootstrappedQueue<string>.Snoc(queue, "Two");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[1, {One}, null, 1, {Two}]");
        queue = BootstrappedQueue<string>.Snoc(queue, "Three");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[3, {One}, [1, {Value is not created.}, null, 0, {}], 0, {}]");
    }

    [Test]
    public async Task SnocThreeTest()
    {
        var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
        queue = BootstrappedQueue<string>.Snoc(queue, "two");
        queue = BootstrappedQueue<string>.Snoc(queue, "three");
        await Assert.That(DumpQueue(queue)).IsEqualTo("[3, {one}, [1, {Value is not created.}, null, 0, {}], 0, {}]");
        queue = BootstrappedQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(queue)).IsEqualTo("[2, {two, three}, null, 0, {}]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        await Assert.That(() => BootstrappedQueue<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
        var x = BootstrappedQueue<string>.Head(queue);
        await Assert.That(x).IsEqualTo("One");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        await Assert.That(() => BootstrappedQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
        queue = BootstrappedQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(queue)).IsEqualTo("[2, {Two, Three}, null, 2, {Three, One}]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var actual = BootstrappedQueue<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = BootstrappedQueue<string>.Tail(queue);
        }

        await Assert.That(BootstrappedQueue<string>.IsEmpty(queue)).IsTrue();
    }
}