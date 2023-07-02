// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

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