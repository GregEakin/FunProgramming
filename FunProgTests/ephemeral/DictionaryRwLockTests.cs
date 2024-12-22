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

public class DictionaryRwLockTests : DictionaryTests, IDisposable
{
    private readonly ReaderWriterLockSlim _lockObject = new ReaderWriterLockSlim();
    private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

    private void InsertAction()
    {
        for (var i = 0; i < Count; i++)
        {
            var word = NextWord(10);
            _lockObject.EnterWriteLock();
            try
            {
                _set = SplayHeap<string>.Insert(word, _set);
            }
            finally
            {
                _lockObject.ExitWriteLock();
            }
        }
    }

    private void RemoveAction()
    {
        for (var i = 0; i < Count; i++)
        {
            SplayHeap<string>.Heap localCopy;
            while (true)
            {
                _lockObject.EnterWriteLock();
                try
                {
                    if (!SplayHeap<string>.IsEmpty(_set))
                    {
                        localCopy = _set;
                        _set = SplayHeap<string>.DeleteMin(localCopy);
                        break;
                    }
                }
                finally
                {
                    _lockObject.ExitWriteLock();
                }

                Thread.Yield();
            }

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
        _lockObject.Dispose();
    }
}