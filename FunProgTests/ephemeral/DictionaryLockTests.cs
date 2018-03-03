// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.ephemeral
{
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
            item = default(string);

            // 58 ms, 3,000 calls
            lock (_lockObject)
            {
                while (!_token.IsCancellationRequested && SplayHeap<string>.IsEmpty(_set))
                    // 33 ms, 1,609 calls
                    Monitor.Wait(_lockObject);

                if (SplayHeap<string>.IsEmpty(_set))
                    return false;

                // 2 ms, 3,000 calls
                item = SplayHeap<string>.FindMin(_set);
                // 2 ms, 3,000 calls
                _set = SplayHeap<string>.DeleteMin(_set);
            }

            return true;
        }
    }

    [TestClass]
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
                    return;

                var unused = Convert.FromBase64String(item);
                // Console.WriteLine(string.Join(", ", unused));
            }
        }

        [TestMethod]
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
            }
        }
    }
}