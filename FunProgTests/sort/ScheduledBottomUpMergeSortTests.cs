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

using FunProgLib.Utilities;
using FunProgLib.sort;

namespace FunProgTests.sort;

public class ScheduledBottomUpMergeSortTests
{
    [Test]
    public async Task EmptyTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        await Assert.That(list.Size).IsEqualTo(0);
        await Assert.That(list.Segs).IsNull();
        list = ScheduledBottomUpMergeSort<string>.Add("One", list);
        await Assert.That(list.Size).IsEqualTo(1);
        await Assert.That(list.Segs).IsNotNull();
    }

    [Test]
    public async Task LazyTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        list = ScheduledBottomUpMergeSort<string>.Add("One", list);
        await Assert.That(list.Segs.Element.Stream.IsValueCreated).IsFalse();
        await Assert.That(list.Segs.Element.Stream.Value.Element).IsEqualTo("One");
        await Assert.That(list.Segs.Element.Stream.IsValueCreated).IsTrue();
    }

    [Test]
    public async Task SortLazyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        await Assert.That(list.Segs.Element.Stream.IsValueCreated).IsFalse();
        var sorted = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(list.Segs.Element.Stream.IsValueCreated).IsTrue();
    }

    [Test]
    public async Task AddTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        list = ScheduledBottomUpMergeSort<string>.Add("One", list);
        await Assert.That(list.Size).IsEqualTo(1);
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo("[One]");
        list = ScheduledBottomUpMergeSort<string>.Add("Two", list);
        await Assert.That(list.Size).IsEqualTo(2);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo("[One, Two]");
        list = ScheduledBottomUpMergeSort<string>.Add("Three", list);
        await Assert.That(list.Size).IsEqualTo(3);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo("[One, Three, Two]");
        list = ScheduledBottomUpMergeSort<string>.Add("Four", list);
        await Assert.That(list.Size).IsEqualTo(4);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo("[Four, One, Three, Two]");
    }

    [Test]
    public async Task SimpleSortTest()
    {
        const string data = "How now, jack brown cow? zed";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo("[brown, cow?, How, jack, now,, zed]");
    }

    [Test]
    public async Task SortAlphabetically()
    {
        const string data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo(data.Split().ToReadableString());
    }

    [Test]
    public async Task SortReverseAlphabetically()
    {
        const string data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        await Assert.That(xs.ToReadableString()).IsEqualTo(data.Split().Reverse().ToReadableString());
    }
}