// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                    var localCopy = _set;
                    Thread.MemoryBarrier();
                    // 99 ms, 13,072 calls
                    var newSet = SplayHeap<string>.Insert(word, localCopy);
                    // 3 ms, 13,072 calls
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, localCopy);
                    if (ReferenceEquals(oldSet, localCopy))
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
                SplayHeap<string>.Heap localCopy;
                while (true)
                {
                    localCopy = _set;
                    Thread.MemoryBarrier();
                    // 13 ms, 66,042 calls
                    if (SplayHeap<string>.IsEmpty(localCopy))
                    {
                        Thread.Yield();
                        continue;
                    }

                    // 15 ms, 7,594 calls
                    var newSet = SplayHeap<string>.DeleteMin(localCopy);
                    // 2 ms, 7,594 calls
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, localCopy);
                    if (ReferenceEquals(oldSet, localCopy))
                        break;
                }

                // 3,000 calls
                var unused = SplayHeap<string>.FindMin(localCopy);
            }
        }

        //[AssertTraffic(AllocatedObjectsCount = 15128)]
        [TestMethod]
        public void Test1()
        {
            var taskList = new ConcurrentBag<Task>();
            for (var i = 0; i < Threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
                taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
            }

            Task.WaitAll(taskList.ToArray());

            Assert.IsNull(_set);
        }
    }
}