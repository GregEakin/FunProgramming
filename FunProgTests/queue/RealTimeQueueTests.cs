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
using FunProgTests.streams;

namespace FunProgTests.queue;

public class RealTimeQueueTests
{
    public static string DumpQueue<T>(RealTimeQueue<T>.Queue queue, bool expandUnCreated)
    {
        if (RealTimeQueue<T>.IsEmpty(queue))
            return string.Empty;
        var result = new StringBuilder();
        result.Append("[{");
        result.Append(StreamTests.DumpStream(queue.F, expandUnCreated));
        result.Append("}, ");
        result.Append(queue.R?.ToReadableString() ?? "null");
        result.Append(", {");
        result.Append(StreamTests.DumpStream(queue.S, expandUnCreated));
        result.Append("}]");
        return result.ToString();
    }

    [Test]
    public async Task Test1()
    {
        var queue = RealTimeQueue<string>.Empty;
        await Assert.That(DumpQueue(queue, true)).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[{One, Two, $Three}, [Three, One], {Three}]");
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        await Assert.That(RealTimeQueue<string>.IsEmpty(queue)).IsTrue();
        queue = RealTimeQueue<string>.Snoc(queue, "Item");
        await Assert.That(RealTimeQueue<string>.IsEmpty(queue)).IsFalse();
        queue = RealTimeQueue<string>.Tail(queue);
        await Assert.That(RealTimeQueue<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptySnocTest()
    {
        await Assert.That(() => RealTimeQueue<string>.Snoc(null, "Item")).Throws<NullReferenceException>();
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        queue = RealTimeQueue<string>.Snoc(queue, "One");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[{$}, null, {$}]");
        queue = RealTimeQueue<string>.Snoc(queue, "Two");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[{One}, [Two], {}]");
        queue = RealTimeQueue<string>.Snoc(queue, "Three");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[{$One, $Two, $Three}, null, {One, Two, Three}]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        await Assert.That(() => RealTimeQueue<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var head = RealTimeQueue<string>.Head(queue);
        await Assert.That(head).IsEqualTo("One");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        await Assert.That(() => RealTimeQueue<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var tail = RealTimeQueue<string>.Tail(queue);
        await Assert.That(DumpQueue(tail, true)).IsEqualTo("[{Two, Three}, [Three, One], {}]");
    }

    [Test]
    public async Task PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        foreach (var expected in data.Split())
        {
            var head = RealTimeQueue<string>.Head(queue);
            await Assert.That(head).IsEqualTo(expected);
            queue = RealTimeQueue<string>.Tail(queue);
        }

        await Assert.That(RealTimeQueue<string>.IsEmpty(queue)).IsTrue();
    }

    private const int Size = 16;
    [Test]
    public async Task PerfTest()
    {
        var heap = RealTimeQueue<int>.Empty;
        for (var i = 0; i < Size; i++)
        {
            heap = RealTimeQueue<int>.Snoc(heap, i);
        }

        Console.WriteLine(DumpQueue(heap, true));
        var count = 0;
        while (!RealTimeQueue<int>.IsEmpty(heap))
        {
            var next = RealTimeQueue<int>.Head(heap);
            await Assert.That(next).IsEqualTo(count);
            heap = RealTimeQueue<int>.Tail(heap);
            count++;
        }

        await Assert.That(count).IsEqualTo(Size);
    }
}