// Copyright 2014 Gregory Eakin <greg@eakin.dev>
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
using Xunit.Abstractions;

namespace FunProgTests.queue;

public class RealTimeQueueTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RealTimeQueueTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public static string DumpQueue<T>(RealTimeQueue<T>.Queue queue, bool expandUnCreated)
    {
        if (RealTimeQueue<T>.IsEmpty(queue)) return string.Empty;

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

    [Fact]
    public void Test1()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Equal(string.Empty, DumpQueue(queue, true));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        Assert.Equal("[{One, Two, $Three}, [Three, One], {Three}]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.True(RealTimeQueue<string>.IsEmpty(queue));

        queue = RealTimeQueue<string>.Snoc(queue, "Item");
        Assert.False(RealTimeQueue<string>.IsEmpty(queue));

        queue = RealTimeQueue<string>.Tail(queue);
        Assert.True(RealTimeQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        Assert.Throws<NullReferenceException>(() => RealTimeQueue<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        queue = RealTimeQueue<string>.Snoc(queue, "One");
        Assert.Equal("[{$}, null, {$}]", DumpQueue(queue, false));

        queue = RealTimeQueue<string>.Snoc(queue, "Two");
        Assert.Equal("[{One}, [Two], {}]", DumpQueue(queue, false));

        queue = RealTimeQueue<string>.Snoc(queue, "Three");
        Assert.Equal("[{$One, $Two, $Three}, null, {One, Two, Three}]", DumpQueue(queue, true));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var head = RealTimeQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = RealTimeQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => RealTimeQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);
        var tail = RealTimeQueue<string>.Tail(queue);
        Assert.Equal("[{Two, Three}, [Three, One], {}]", DumpQueue(tail, true));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeQueue<string>.Empty, RealTimeQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var head = RealTimeQueue<string>.Head(queue);
            Assert.Equal(expected, head);
            queue = RealTimeQueue<string>.Tail(queue);
        }

        Assert.True(RealTimeQueue<string>.IsEmpty(queue));
    }

    private const int Size = 16;

    [Fact]
    public void PerfTest()
    {
        var heap = RealTimeQueue<int>.Empty;
        for (var i = 0; i < Size; i++)
        {
            heap = RealTimeQueue<int>.Snoc(heap, i);
        }

        _testOutputHelper.WriteLine(DumpQueue(heap, true));

        var count = 0;
        while (!RealTimeQueue<int>.IsEmpty(heap))
        {
            var next = RealTimeQueue<int>.Head(heap);
            Assert.Equal(count, next);
            heap = RealTimeQueue<int>.Tail(heap);
            count++;
        }

        Assert.Equal(Size, count);
    }
}