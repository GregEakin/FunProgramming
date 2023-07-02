// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.Utilities;
using FunProgLib.sort;

namespace FunProgTests.sort;

public class ScheduledBottomUpMergeSortTests
{
    [Fact]
    public void EmptyTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        Assert.Equal(0, list.Size);
        Assert.Null(list.Segs);

        list = ScheduledBottomUpMergeSort<string>.Add("One", list);
        Assert.Equal(1, list.Size);
        Assert.NotNull(list.Segs);
    }

    [Fact]
    public void LazyTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        list = ScheduledBottomUpMergeSort<string>.Add("One", list);

        Assert.False(list.Segs.Element.Stream.IsValueCreated);
        Assert.Equal("One", list.Segs.Element.Stream.Value.Element);
        Assert.True(list.Segs.Element.Stream.IsValueCreated);
    }

    [Fact]
    public void SortLazyTest()
    {
        const string data = "How now, brown cow?";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        Assert.False(list.Segs.Element.Stream.IsValueCreated);
        var sorted = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.True(list.Segs.Element.Stream.IsValueCreated);
    }

    [Fact]
    public void AddTest()
    {
        var list = ScheduledBottomUpMergeSort<string>.Empty;
        list = ScheduledBottomUpMergeSort<string>.Add("One", list);
        Assert.Equal(1, list.Size);
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[One]", xs.ToReadableString());

        list = ScheduledBottomUpMergeSort<string>.Add("Two", list);
        Assert.Equal(2, list.Size);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[One, Two]", xs.ToReadableString());

        list = ScheduledBottomUpMergeSort<string>.Add("Three", list);
        Assert.Equal(3, list.Size);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[One, Three, Two]", xs.ToReadableString());

        list = ScheduledBottomUpMergeSort<string>.Add("Four", list);
        Assert.Equal(4, list.Size);
        xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[Four, One, Three, Two]", xs.ToReadableString());
    }

    [Fact]
    public void SimpleSortTest()
    {
        const string data = "How now, jack brown cow? zed";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal("[brown, cow?, How, jack, now,, zed]", xs.ToReadableString());
    }

    [Fact]
    public void SortAlphabetically()
    {
        const string data = "Alpha Bravo Charlie Delta Echo Foxtrot Golf Hotel India Juliet Kilo Lima Mike November Oscar Papa Quebec Romeo Sierra Tango Uniform Victor Whiskey X-ray Yankee Zulu";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal(data.Split().ToReadableString(), xs.ToReadableString());
    }

    [Fact]
    public void SortReverseAlphabetically()
    {
        const string data = "Zulu Yankee X-ray Whiskey Victor Uniform Tango Sierra Romeo Quebec Papa Oscar November Mike Lima Kilo Juliet India Hotel Golf Foxtrot Echo Delta Charlie Bravo Alpha";
        var list = data.Split().Aggregate(ScheduledBottomUpMergeSort<string>.Empty, (ts, x) => ScheduledBottomUpMergeSort<string>.Add(x, ts));
        var xs = ScheduledBottomUpMergeSort<string>.Sort(list);
        Assert.Equal(data.Split().Reverse().ToReadableString(), xs.ToReadableString());
    }
}