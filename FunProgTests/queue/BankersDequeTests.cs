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

namespace FunProgTests.queue;

using FunProgLib.queue;
using static streams.StreamTests;

public class BankersDequeTests
{
    private static string DumpQueue<T>(BankersDeque<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}]";
    }

    [Fact]
    public void EmptyTest()
    {
        var queue = BankersDeque<string>.Empty;
        Assert.True(BankersDeque<string>.IsEmpty(queue));

        queue = BankersDeque<string>.Cons("Head", queue);
        Assert.False(BankersDeque<string>.IsEmpty(queue));
        queue = BankersDeque<string>.Tail(queue);
        Assert.True(BankersDeque<string>.IsEmpty(queue));

        queue = BankersDeque<string>.Snoc(queue, "Tail");
        Assert.False(BankersDeque<string>.IsEmpty(queue));
        queue = BankersDeque<string>.Init(queue);
        Assert.True(BankersDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void ConsTest()
    {
        var queue = BankersDeque<string>.Empty;
        queue = BankersDeque<string>.Cons("Last", queue);
        queue = BankersDeque<string>.Cons("Head", queue);

        Assert.Equal("[1, {$Head}, 1, {$Last}]", DumpQueue(queue, true));
    }

    [Fact]
    public void ConsHeadTailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersDeque<string>.Empty, (queue1, s) => BankersDeque<string>.Cons(s, queue1));

        foreach (var expected in data.Split().Reverse())
        {
            var actual = BankersDeque<string>.Head(queue);
            Assert.Equal(expected, actual);
            queue = BankersDeque<string>.Tail(queue);
        }

        Assert.True(BankersDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void SnocTest()
    {
        var queue = BankersDeque<string>.Empty;
        queue = BankersDeque<string>.Snoc(queue, "Head");
        queue = BankersDeque<string>.Snoc(queue, "Last");

        Assert.Equal("[1, {$Head}, 1, {$Last}]", DumpQueue(queue, true));
    }

    [Fact]
    public void SnocLastInitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(BankersDeque<string>.Empty, BankersDeque<string>.Snoc);

        var dat = data.Split().Reverse();
        foreach (var expected in dat)
        {
            var actual = BankersDeque<string>.Last(queue);
            Assert.Equal(expected, actual);
            queue = BankersDeque<string>.Init(queue);
        }

        Assert.True(BankersDeque<string>.IsEmpty(queue));
    }

    [Fact]
    public void EmptyHeadTest()
    {
        var queue = BankersDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BankersDeque<string>.Head(queue));
    }

    [Fact]
    public void EmptyTailTest()
    {
        var queue = BankersDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BankersDeque<string>.Tail(queue));
    }

    [Fact]
    public void EmptyLastTest()
    {
        var queue = BankersDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BankersDeque<string>.Last(queue));
    }

    [Fact]
    public void EmptyInitTest()
    {
        var queue = BankersDeque<string>.Empty;
        Assert.Throws<ArgumentNullException>(() => BankersDeque<string>.Init(queue));
    }
}