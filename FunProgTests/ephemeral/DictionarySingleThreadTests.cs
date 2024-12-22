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