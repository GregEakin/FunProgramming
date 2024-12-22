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

using FunProgLib.sort;
using FunProgLib.Utilities;

namespace FunProgTests.sort;

public class BottomUpMergeSortTests
{
    [Fact]
    public void EmptyTest()
    {
        var list = BottomUpMergeSort<string>.Empty;
        // Assert.False(list.Segs.IsValueCreated);
        Assert.Null(list.Segs.Value);
        Assert.True(list.Segs.IsValueCreated);
    }

    [Fact]
    public void LazyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
        Assert.False(list.Segs.IsValueCreated);
        var xs = BottomUpMergeSort<string>.Sort(list);
        Assert.True(list.Segs.IsValueCreated);
    }

    [Fact]
    public void SimpleSortTest()
    {
        const string data = "How now, jack brown cow? zed";
        var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
        var xs = BottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[brown, cow?, How, jack, now,, zed]", xs.ToReadableString());
    }

    [Fact]
    public void SortAlphabetically()
    {
        const string data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
        var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
        var xs = BottomUpMergeSort<string>.Sort(list);
        Assert.Equal(data.Split().ToReadableString(), xs.ToReadableString());
    }

    [Fact]
    public void SortReverseAlphabetically()
    {
        const string data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
        var list = data.Split().Aggregate(BottomUpMergeSort<string>.Empty, (ts, x) => BottomUpMergeSort<string>.Add(x, ts));
        var xs = BottomUpMergeSort<string>.Sort(list);
        Assert.Equal(data.Split().Reverse().ToReadableString(), xs.ToReadableString());
    }
}