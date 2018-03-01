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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class DictionaryLockTests : DictionaryTests
    {
        private readonly object _lockObject = new object();
        private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

        // 157 ms, 10 calls
        private void InsertAction()
        {
            for (var i = 0; i < Count; i++)
            {
                // 5 ms, 3,000 calls
                var word = NextWord(10);
                // 132 ms, 3,000 calls
                lock (_lockObject)
                {
                    // 13 ms, 3,000 calls
                    _set = SplayHeap<string>.Insert(word, _set);

                    // 2 ms, 3,000 calls
                    Monitor.Pulse(_lockObject);
                }
            }
        }

        // 98 ms, 10 calls
        private void RemoveAction()
        {
            for (var i = 0; i < Count; i++)
            {
                // 58 ms, 3,000 calls
                lock (_lockObject)
                {
                    while (SplayHeap<string>.IsEmpty(_set))
                    {
                        // 33 ms, 1,609 calls
                        Monitor.Wait(_lockObject);
                    }

                    // 2 ms, 3,000 calls
                    var unused = SplayHeap<string>.FindMin(_set);
                    // 2 ms, 3,000 calls
                    _set = SplayHeap<string>.DeleteMin(_set);
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
    }
}