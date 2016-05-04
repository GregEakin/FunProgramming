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
    public class DictionaryLockTests : DictionaryTests
    {
        private readonly object lockObject = new object();
        private SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

        // 157 ms, 1o calls
        private readonly Action<object> insertAction = obj =>
        {
            var map = (DictionaryLockTests)obj;
            for (var i = 0; i < count; i++)
            {
                // 5 ms, 3,000 calls
                var word = map.NextWord(10);
                // 132 ms, 3,000 calls
                lock (map.lockObject)
                {
                    // 13 ms, 3,000 calls
                    map.set = SplayHeap<string>.Insert(word, map.set);

                    // 2 ms, 3,000 calls
                    Monitor.Pulse(map.lockObject);
                }
            }
        };

        // 98 ms, 10 calls
        private readonly Action<object> removeAction = obj =>
        {
            var map = (DictionaryLockTests)obj;
            for (var i = 0; i < count; i++)
            {
                // 58 ms, 3,000 calls
                lock (map.lockObject)
                {
                    while (SplayHeap<string>.IsEmpty(map.set))
                    {
                        // 33 ms, 1,609 calls
                        Monitor.Wait(map.lockObject);
                    }

                    // 2 ms, 3,000 calls
                    var word = SplayHeap<string>.FindMin(map.set);
                    // 2 ms, 3,000 calls
                    map.set = SplayHeap<string>.DeleteMin(map.set);
                }
            }
        };

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(insertAction, this));
                taskList.Add(Task.Factory.StartNew(removeAction, this));
            }

            Task.WaitAll(taskList.ToArray());
        }
    }
}