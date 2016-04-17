﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

namespace FunProgTests.ephemeral
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using FunProgLib.heap;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionarySemaphoreTests
    {
        const int threads = 20;
        const int count = 300;

        private readonly Random random = new Random(10000);

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        private volatile SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

        private string NextWord(int length)
        {
            var stringBuilder = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var x = random.Next(32, 126);
                stringBuilder.Append((char)x);
            }
            return stringBuilder.ToString();
        }

        private readonly Action<object> insertAction = (object obj) =>
        {
            var map = (DictionarySemaphoreTests)obj;
            for (var i = 0; i < count; i++)
            {
                var word = map.NextWord(10);

                map.semaphore.Wait();
                try
                {
                    Interlocked.MemoryBarrier();
                    var newSet = SplayHeap<string>.Insert(word, map.set);
                    Interlocked.Exchange(ref map.set, newSet);
                }
                finally
                {
                    map.semaphore.Release();
                }

                //Console.WriteLine("--> Task={0}, obj={1}, Thread={2}",
                //                  Task.CurrentId, word,
                //                  Thread.CurrentThread.ManagedThreadId);
            }
        };

        private readonly Action<object> removeAction = (object obj) =>
        {
            var map = (DictionarySemaphoreTests)obj;
            for (var i = 0; i < count; i++)
            {
                while (true)
                {
                    Interlocked.MemoryBarrier();
                    if (SplayHeap<string>.IsEmpty(map.set))
                    {
                        //Console.WriteLine("=== Task={0}, obj={1}, Thread={2}",
                        //  Task.CurrentId, "Wait",
                        //  Thread.CurrentThread.ManagedThreadId);

                        Thread.Yield();
                        continue;
                    }

                    map.semaphore.Wait();
                    try
                    {
                        Interlocked.MemoryBarrier();
                        var localSet = map.set;
                        if (SplayHeap<string>.IsEmpty(localSet))
                        {
                            continue;
                        }

                        var word = SplayHeap<string>.FindMin(localSet);
                        var newSet = SplayHeap<string>.DeleteMin(localSet);
                        Interlocked.Exchange(ref map.set, newSet);

                        //Console.WriteLine("<-- Task={0}, obj={1}, Thread={2}",
                        //                  Task.CurrentId, word,
                        //                  Thread.CurrentThread.ManagedThreadId);

                        break;
                    }
                    finally
                    {
                        map.semaphore.Release();
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