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

public sealed class DictionarySemaphoreTests : DictionaryTests, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
    private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

    // 158 ms, 10 calls
    private void InsertAction()
    {
        for (var i = 0; i < Count; i++)
        {
            // 4 ms, 3,000 calls
            var word = NextWord(10);

            // 106 ms, 3,000 calls
            _semaphore.Wait();
            try
            {
                Interlocked.MemoryBarrier();
                // 43 ms, 3,000 calls
                var newSet = SplayHeap<string>.Insert(word, _set);
                Interlocked.Exchange(ref _set, newSet);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    // 329 ms, 10 calls
    private void RemoveAction()
    {
        for (var i = 0; i < Count; i++)
        {
            SplayHeap<string>.Heap localCopy;
            while (true)
            {
                // 294 ms, 3,000 calls
                _semaphore.Wait();
                try
                {
                    localCopy = _set;
                    Interlocked.MemoryBarrier();
                    if (SplayHeap<string>.IsEmpty(localCopy))
                    {
                        Thread.Yield();
                        continue;
                    }

                    // 4 ms, 3,000 calls
                    var newSet = SplayHeap<string>.DeleteMin(localCopy);
                    Interlocked.Exchange(ref _set, newSet);
                    break;
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            // 3 ms, 3,000 calls
            var _ = SplayHeap<string>.FindMin(localCopy);
        }
    }

    [Fact]
    public async Task Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 2)
        {
            taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
            taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
        }

        await Task.WhenAll(taskList.ToArray());
        Assert.Null(_set);
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}