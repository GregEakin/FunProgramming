// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.heap;

namespace FunProgTests.ephemeral;

public class DictionarySingleThreadTests : DictionaryTests
{
    private readonly Random _random = new Random();
    private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

    private int InsertAction(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var word = NextWord(10);
            _set = SplayHeap<string>.Insert(word, _set);
        }

        return count;
    }

    private int RemoveAction(int count)
    {
        var i = 0;
        while (i < count)
        {
            if (SplayHeap<string>.IsEmpty(_set))
                return i;

            var localCopy = _set;
            _set = SplayHeap<string>.DeleteMin(localCopy);
            var _ = SplayHeap<string>.FindMin(localCopy);
            i++;
        }

        return i;
    }

    [Fact]
    public void Test1()
    {
        const int size = Threads * Count / 2;
        var writes = 0;
        var reads = 0;
        while (writes < size || reads < size)
        {
            var count = _random.Next(50);
            var next = _random.Next(10);
            if (next < 5 && writes < size)
            {
                if (count > size - writes)
                    count = size - writes;
                writes += InsertAction(count);
            }
            else
            {
                if (count > size - reads)
                    count = size - reads;
                reads += RemoveAction(count);
            }
        }

        Assert.Null(_set);
    }
}