// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunProgTests.ephemeral
{
    [TestClass]
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
                    // 0 ms, 3,000 calls
                    Interlocked.MemoryBarrier();
                    // 43 ms, 3,000 calls
                    var newSet = SplayHeap<string>.Insert(word, _set);
                    // 0 ms, 3,000 calls
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
                while (true)
                {
                    Interlocked.MemoryBarrier();
                    if (SplayHeap<string>.IsEmpty(_set))
                    {
                        // 14 ms, 17,011 calls
                        Thread.Yield();
                        continue;
                    }

                    // 294 ms, 3,000 calls
                    _semaphore.Wait();
                    try
                    {
                        Interlocked.MemoryBarrier();
                        var localSet = _set;
                        if (SplayHeap<string>.IsEmpty(localSet))
                        {
                            continue;
                        }

                        // 3 ms, 3,000 calls
                        var word = SplayHeap<string>.FindMin(localSet);
                        // 4 ms, 3,000 calls
                        var newSet = SplayHeap<string>.DeleteMin(localSet);
                        Interlocked.Exchange(ref _set, newSet);
                        break;
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < Threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
                taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
            }

            Task.WaitAll(taskList.ToArray());
        }

        private bool _disposed;

        ~DictionarySemaphoreTests()
        {
            Dispose(false);    
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _semaphore.Dispose();
                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }
    }
}
