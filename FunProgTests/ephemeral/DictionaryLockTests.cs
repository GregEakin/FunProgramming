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
        private readonly object lockObject = new object();
        private SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

        // 157 ms, 1o calls
        private void InsertAction()
        {
            for (var i = 0; i < count; i++)
            {
                // 5 ms, 3,000 calls
                var word = NextWord(10);
                // 132 ms, 3,000 calls
                lock (lockObject)
                {
                    // 13 ms, 3,000 calls
                    set = SplayHeap<string>.Insert(word, set);

                    // 2 ms, 3,000 calls
                    Monitor.Pulse(lockObject);
                }
            }
        }

        // 98 ms, 10 calls
        private void RemoveAction()
        {
            for (var i = 0; i < count; i++)
            {
                // 58 ms, 3,000 calls
                lock (lockObject)
                {
                    while (SplayHeap<string>.IsEmpty(set))
                    {
                        // 33 ms, 1,609 calls
                        Monitor.Wait(lockObject);
                    }

                    // 2 ms, 3,000 calls
                    var word = SplayHeap<string>.FindMin(set);
                    // 2 ms, 3,000 calls
                    set = SplayHeap<string>.DeleteMin(set);
                }
            }
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
                taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
            }

            Task.WaitAll(taskList.ToArray());
        }
    }
}