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

    [Fact]
    public void Test1()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Equal("[null, null]", DumpQueue(queue));
    }

    [Fact]
    public void Test2()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        Assert.Equal("[[One], [Three, One, Three, Two]]", DumpQueue(queue));
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.True(BatchedQueue<string>.IsEmpty(queue));

        queue = BatchedQueue<string>.Snoc(queue, "Item");
        Assert.False(BatchedQueue<string>.IsEmpty(queue));

        queue = BatchedQueue<string>.Tail(queue);
        Assert.True(BatchedQueue<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptySnocTest()
    {
        Assert.Throws<NullReferenceException>(() => BatchedQueue<string>.Snoc(null, "Item"));
    }

    [Fact]
    public void SnocTest()
    {

    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BatchedQueue<string>.Head(queue));
    }

    [Fact]
    public void HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var head = BatchedQueue<string>.Head(queue);
        Assert.Equal("One", head);
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = BatchedQueue<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BatchedQueue<string>.Tail(queue));
    }

    [Fact]
    public void TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);
        var tail = BatchedQueue<string>.Tail(queue);
        Assert.Equal("[[Two, Three, One, Three], null]", DumpQueue(tail));
    }

    [Fact]
    public void PushPopTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BatchedQueue<string>.Empty, BatchedQueue<string>.Snoc);

        foreach (var expected in data.Split())
        {
            var actual = BatchedQueue<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = BatchedQueue<string>.Tail(queue);
        }

        Assert.True(BatchedQueue<string>.IsEmpty(queue));
    }
}