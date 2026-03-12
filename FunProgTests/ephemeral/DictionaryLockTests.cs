// Copyright 2025 Gregory Eakin <greg@eakin.dev>
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

public class DictionaryLock<T>
{
    private readonly object _lockObject = new();
    private readonly CancellationToken _token;
    private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

    public DictionaryLock(CancellationToken token)
    {
        _token = token;

        token.Register(() =>
        {
            lock (_lockObject)
                Monitor.PulseAll(_lockObject);
        });
    }

    public bool Insert(string word)
    {
        if (_token.IsCancellationRequested)
            return false;

        // 132 ms, 3,000 calls
        lock (_lockObject)
        {
            // 13 ms, 3,000 calls
            _set = SplayHeap<string>.Insert(word, _set);

            // 2 ms, 3,000 calls
            Monitor.Pulse(_lockObject);
        }

        return true;
    }

    public bool Remove(out string item)
    {
        // 58 ms, 3,000 calls
        SplayHeap<string>.Heap localCopy;
        lock (_lockObject)
        {
            while (!_token.IsCancellationRequested && SplayHeap<string>.IsEmpty(_set))
                // 33 ms, 1,609 calls
                Monitor.Wait(_lockObject);

            if (SplayHeap<string>.IsEmpty(_set))
            {
                item = null;
                return false;
            }

            // 2 ms, 3,000 calls
            localCopy = _set;
            _set = SplayHeap<string>.DeleteMin(localCopy);
        }

        // 2 ms, 3,000 calls
        item = SplayHeap<string>.FindMin(localCopy);
        return true;
    }

    public bool IsEmpty
    {
        get
        {
            lock (_lockObject)
            {
                return SplayHeap<string>.IsEmpty(_set);
            }
        }
    }
}

public class DictionaryLockTests : DictionaryTests
{
    // 157 ms, 10 calls
    private void InsertAction(object ojb)
    {
        var map = (DictionaryLock<string>)ojb;
        for (var i = 0; i < Count; i++)
        {
            // 5 ms, 3,000 calls
            var word = NextWord(10);
            var inserted = map.Insert(word);
            if (!inserted)
                break;
        }
    }

    // 98 ms, 10 calls
    private static void RemoveAction(object ojb)
    {
        var map = (DictionaryLock<string>)ojb;
        for (var i = 0; i < Count; i++)
        {
            var removed = map.Remove(out var item);
            if (!removed)
                break;

            _ = Convert.FromBase64String(item);
            // Console.WriteLine(string.Join(", ", unused));
        }
    }

    [Test]
    public async Task Test1(CancellationToken token)
    {
        var taskList = new ConcurrentBag<Task>();
        var dictionary = new DictionaryLock<string>(token);

        for (var i = 0; i < Threads; i += 2)
        {
            taskList.Add(Task.Factory.StartNew(InsertAction, dictionary, token));
            taskList.Add(Task.Factory.StartNew(RemoveAction, dictionary, token));
        }

        await Task.WhenAll(taskList.ToArray());
        await Assert.That(dictionary.IsEmpty).IsTrue();
    }
}