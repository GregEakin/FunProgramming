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

public class DictionaryLock<T>
{
    private readonly object _lockObject = new object();
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
                item = default(string);
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
        var map = (DictionaryLock<string>) ojb;
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
        var map = (DictionaryLock<string>) ojb;
        for (var i = 0; i < Count; i++)
        {
            var removed = map.Remove(out var item);
            if (!removed)
                break;

            var _ = Convert.FromBase64String(item);
            // Console.WriteLine(string.Join(", ", unused));
        }
    }

    [Fact]
    public void Test1()
    {
        using (var tokenSource = new CancellationTokenSource())
        {
            var token = tokenSource.Token;
            var taskList = new ConcurrentBag<Task>();
            var dictionary = new DictionaryLock<string>(token);

            for (var i = 0; i < Threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(InsertAction, dictionary, token));
                taskList.Add(Task.Factory.StartNew(RemoveAction, dictionary, token));
            }

            Task.WaitAll(taskList.ToArray());
            Assert.True(dictionary.IsEmpty);
        }
    }
}