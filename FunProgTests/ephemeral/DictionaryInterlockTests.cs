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
    public class DictionaryInterlockTests : DictionaryTests
    {
        private volatile SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

        // 132 ms, 10 calls
        private void InsertAction()
        {
            for (var i = 0; i < Count; i++)
            {
                // 18 ms, 3,000 calls
                var word = NextWord(10);
                while (true)
                {
                    var workingSet = _set;
                    Thread.MemoryBarrier();
                    // 99 ms, 13,072 calls
                    var newSet = SplayHeap<string>.Insert(word, workingSet);
                    // 3 ms, 13,072 calls
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, workingSet);
                    if (ReferenceEquals(oldSet, workingSet))
                    {
                        // 3,000 calls
                        break;
                    }
                }
            }
        }

        // 91 ms, 10 calls
        private void RemoveAction()
        {
            for (var i = 0; i < Count; i++)
            {
                while (true)
                {
                    var workingSet = _set;
                    Thread.MemoryBarrier();
                    // 13 ms, 66,042 calls
                    if (SplayHeap<string>.IsEmpty(workingSet))
                    {
                        continue;
                    }

                    // 15 ms, 7,594 calls
                    var unused = SplayHeap<string>.FindMin(workingSet);
                    // 15 ms, 7,594 calls
                    var newSet = SplayHeap<string>.DeleteMin(workingSet);
                    // 2 ms, 7,594 calls
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, workingSet);
                    if (ReferenceEquals(oldSet, workingSet))
                    {
                        // 3,000 calls
                        break;
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
    }
}
