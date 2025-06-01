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

public class DictionaryInterlockTests : DictionaryTests
{
    private volatile SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

    // 132 ms, 10 calls
    private void InsertAction()
    {
        for (var i = 0; i < Count; i++)
        {
            // 18 ms, 3,000 calls
            var word = NextWord(10);
            while (true)
            {
                var localCopy = _set;
                Thread.MemoryBarrier();
                // 99 ms, 13,072 calls
                var newSet = SplayHeap<string>.Insert(word, localCopy);
                // 3 ms, 13,072 calls
                var oldSet = Interlocked.CompareExchange(ref _set, newSet, localCopy);
                if (ReferenceEquals(oldSet, localCopy))
                {
                    // 3,000 calls
                    break;
                }
            }
        }
    }

    // 91 ms, 10 calls
    private void RemoveAction()
    {
        for (var i = 0; i < Count; i++)
        {
            SplayHeap<string>.Heap localCopy;
            while (true)
            {
                localCopy = _set;
                Thread.MemoryBarrier();
                // 13 ms, 66,042 calls
                if (SplayHeap<string>.IsEmpty(localCopy))
                {
                    Thread.Yield();
                    continue;
                }

                // 15 ms, 7,594 calls
                var newSet = SplayHeap<string>.DeleteMin(localCopy);
                // 2 ms, 7,594 calls
                var oldSet = Interlocked.CompareExchange(ref _set, newSet, localCopy);
                if (ReferenceEquals(oldSet, localCopy))
                    break;
            }

            // 3,000 calls
            _ = SplayHeap<string>.FindMin(localCopy);
        }
    }

    //[AssertTraffic(AllocatedObjectsCount = 15128)]
    [Fact]
    public async Task Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 2)
        {
            taskList.Add(Task.Factory.StartNew(_ => InsertAction(), this, TestContext.Current.CancellationToken));
            taskList.Add(Task.Factory.StartNew(_ => RemoveAction(), this, TestContext.Current.CancellationToken));
        }

        await Task.WhenAll(taskList.ToArray());

        Assert.Null(_set);
    }
}