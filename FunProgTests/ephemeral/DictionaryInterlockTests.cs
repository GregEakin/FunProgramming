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
    public class DictionaryInterlockTests : DictionaryTests
    {
        private SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

        // 132 ms, 10 calls
        private readonly Action<object> insertAction = (object obj) =>
        {
            var map = (DictionaryInterlockTests)obj;
            for (var i = 0; i < count; i++)
            {
                // 18 ms, 3,000 calls
                var word = map.NextWord(10);
                while (true)
                {
                    Interlocked.MemoryBarrier();
                    var workingSet = map.set;
                    // 99 ms, 13,072 calls
                    var newSet = SplayHeap<string>.Insert(word, workingSet);
                    // 3 ms, 13,072 calls
                    var oldSet = Interlocked.CompareExchange(ref map.set, newSet, workingSet);
                    if (ReferenceEquals(oldSet, workingSet))
                    {
                        // 3,000 calls
                        break;
                    }
                }
            }
        };

        // 91 ms, 10 calls
        private readonly Action<object> removeAction = (object obj) =>
        {
            var map = (DictionaryInterlockTests)obj;
            for (var i = 0; i < count; i++)
            {
                while (true)
                {
                    Interlocked.MemoryBarrier();
                    var workingSet = map.set;
                    // 13 ms, 66,042 calls
                    if (SplayHeap<string>.IsEmpty(workingSet))
                    {
                        continue;
                    }

                    // 15 ms, 7,594 calls
                    var word = SplayHeap<string>.FindMin(workingSet);
                    // 15 ms, 7,594 calls
                    var newSet = SplayHeap<string>.DeleteMin(workingSet);
                    // 2 ms, 7,594 calls
                    var oldSet = Interlocked.CompareExchange(ref map.set, newSet, workingSet);
                    if (ReferenceEquals(oldSet, workingSet))
                    {
                        // 3,000 calls
                        break;
                    }
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
