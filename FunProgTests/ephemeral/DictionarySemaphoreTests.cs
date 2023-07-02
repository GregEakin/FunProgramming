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
    public void Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 2)
        {
            taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
            taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
        }

        Task.WaitAll(taskList.ToArray());
        Assert.Null(_set);
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}