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

public class RealTimeDequeTests

{
    private static string DumpQueue<T>(RealTimeDeque<T>.Queue queue, bool expandUnCreated)
    {
        return $"[{queue.LenF}, {{{DumpStream(queue.F, expandUnCreated)}}}, {{{DumpStream(queue.Sf, expandUnCreated)}}}, " + $"{queue.LenR}, {{{DumpStream(queue.R, expandUnCreated)}}}, {{{DumpStream(queue.Sr, expandUnCreated)}}}]";
    }

    [Test]
    public async Task EmptyTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsTrue();
        queue = RealTimeDeque<string>.Cons("Head", queue);
        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsFalse();
        queue = RealTimeDeque<string>.Tail(queue);
        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsTrue();
        queue = RealTimeDeque<string>.Snoc(queue, "Tail");
        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsFalse();
        queue = RealTimeDeque<string>.Init(queue);
        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task EmptyConsTest()
    {
        await Assert.That(() => RealTimeDeque<string>.Cons("Item", null)).Throws<NullReferenceException>();
    }

    [Test]
    public async Task ConsTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        queue = RealTimeDeque<string>.Cons("Last", queue);
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[1, {$}, {}, 0, {}, {}]");
        queue = RealTimeDeque<string>.Cons("Head", queue);
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$Head}, {Head}, 1, {$Last}, {Last}]");
    }

    [Test]
    public async Task EmptyHeadTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        await Assert.That(() => RealTimeDeque<string>.Head(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task HeadTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        var head = RealTimeDeque<string>.Head(queue);
        await Assert.That(head).IsEqualTo("Three");
    }

    [Test]
    public async Task EmptyTailTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        await Assert.That(() => RealTimeDeque<string>.Tail(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task TailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        var tail = RealTimeDeque<string>.Tail(queue);
        await Assert.That(DumpQueue(tail, true)).IsEqualTo("[1, {One}, {}, 3, {One, Two, $Three}, {Three}]");
    }

    [Test]
    public async Task ConsHeadTailTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        foreach (var expected in Enumerable.Reverse(data.Split()))
        {
            var actual = RealTimeDeque<string>.Head(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = RealTimeDeque<string>.Tail(queue);
        }

        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task IncrementalHeadTest()
    {
        const string data = "One Two Three Four Five";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, (queue1, s) => RealTimeDeque<string>.Cons(s, queue1));
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[2, {$}, {$}, 3, {$}, {$}]");
        // After looking at the first element, the rest of the queue should be not created.
        var head = RealTimeDeque<string>.Head(queue);
        await Assert.That(head).IsEqualTo("Five");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[2, {Five, $}, {Five, $}, 3, {$}, {$}]");
    }

    [Test]
    public async Task EmptySoncTest()
    {
        await Assert.That(() => RealTimeDeque<string>.Snoc(null, "Item")).Throws<NullReferenceException>();
    }

    [Test]
    public async Task SnocTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        queue = RealTimeDeque<string>.Snoc(queue, "Head");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[0, {}, {}, 1, {$}, {}]");
        queue = RealTimeDeque<string>.Snoc(queue, "Last");
        await Assert.That(DumpQueue(queue, true)).IsEqualTo("[1, {$Head}, {Head}, 1, {$Last}, {Last}]");
    }

    [Test]
    public async Task LastTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        var last = RealTimeDeque<string>.Last(queue);
        await Assert.That(last).IsEqualTo("Three");
    }

    [Test]
    public async Task EmptyLastTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        await Assert.That(() => RealTimeDeque<string>.Last(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task InitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        var init = RealTimeDeque<string>.Init(queue);
        await Assert.That(DumpQueue(init, true)).IsEqualTo("[3, {One, Two, $Three}, {Three}, 1, {One}, {}]");
    }

    [Test]
    public async Task EmptyInitTest()
    {
        var queue = RealTimeDeque<string>.Empty;
        await Assert.That(() => RealTimeDeque<string>.Init(queue)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task SnocLastInitTest()
    {
        const string data = "One Two Three One Three";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        var dat = Enumerable.Reverse(data.Split());
        foreach (var expected in dat)
        {
            var actual = RealTimeDeque<string>.Last(queue);
            await Assert.That(actual).IsEqualTo(expected);
            queue = RealTimeDeque<string>.Init(queue);
        }

        await Assert.That(RealTimeDeque<string>.IsEmpty(queue)).IsTrue();
    }

    [Test]
    public async Task IncrementalLastTest()
    {
        const string data = "One Two Three Four Five";
        var queue = data.Split().Aggregate(RealTimeDeque<string>.Empty, RealTimeDeque<string>.Snoc);
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[3, {$}, {$}, 2, {$}, {$}]");
        // After looking at the last element, the rest of the queue should be not created.
        var last = RealTimeDeque<string>.Last(queue);
        await Assert.That(last).IsEqualTo("Five");
        await Assert.That(DumpQueue(queue, false)).IsEqualTo("[3, {$}, {$}, 2, {Five, $}, {Five, $}]");
    }
}