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
            if (list == null) break;
            result.Append(separator);
            separator = ", ";
            var head = FunList<T>.Head(list);
            result.Append(head);
            list = FunList<T>.Tail(list);
        }
        result.Append('}');
        return result.ToString();
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        Assert.True(BootstrappedQueue<string>.IsEmpty(queue));

        queue = BootstrappedQueue<string>.Snoc(queue, "Item");
        Assert.False(BootstrappedQueue<string>.IsEmpty(queue));

        queue = BootstrappedQueue<string>.Tail(queue);
        Assert.True(BootstrappedQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
        Assert.Equal("[1, {one}, null, 0, {}]", DumpQueue(queue));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        queue = BootstrappedQueue<string>.Snoc(queue, "One");
        Assert.Equal("[1, {One}, null, 0, {}]", DumpQueue(queue));

        queue = BootstrappedQueue<string>.Snoc(queue, "Two");
        Assert.Equal("[1, {One}, null, 1, {Two}]", DumpQueue(queue));

        queue = BootstrappedQueue<string>.Snoc(queue, "Three");
        Assert.Equal("[3, {One}, [1, {Value is not created.}, null, 0, {}], 0, {}]", DumpQueue(queue));
    }

    [Fact]
    public void SnocThreeTest()
    {
        var queue = BootstrappedQueue<string>.Snoc(BootstrappedQueue<string>.Empty, "one");
        queue = BootstrappedQueue<string>.Snoc(queue, "two");
        queue = BootstrappedQueue<string>.Snoc(queue, "three");
        Assert.Equal("[3, {one}, [1, {Value is not created.}, null, 0, {}], 0, {}]", DumpQueue(queue));

        queue = BootstrappedQueue<string>.Tail(queue);
        Assert.Equal("[2, {two, three}, null, 0, {}]", DumpQueue(queue));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BootstrappedQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
        var x = BootstrappedQueue<string>.Head(queue);
        Assert.Equal("One", x);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = BootstrappedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BootstrappedQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);
        queue = BootstrappedQueue<string>.Tail(queue);
        Assert.Equal("[2, {Two, Three}, null, 2, {Three, One}]", DumpQueue(queue));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BootstrappedQueue<string>.Empty, BootstrappedQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var actual = BootstrappedQueue<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = BootstrappedQueue<string>.Tail(queue);
        }

        Assert.True(BootstrappedQueue<string>.IsEmpty(queue));
    }
}