// Fun Programming Data Structures 1.0
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
    public class DictionaryLockTests
    {
        const int threads = 20;
        const int count = 300;

        private readonly Random random = new Random(10000);

        private SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

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
            var map = (DictionaryLockTests)obj;
            for (var i = 0; i < count; i++)
            {
                var word = map.NextWord(10);
                lock (map)
                {
                    map.set = SplayHeap<string>.Insert(word, map.set);

                    //Console.WriteLine("--> Task={0}, obj={1}, Thread={2}",
                    //                  Task.CurrentId, word,
                    //                  Thread.CurrentThread.ManagedThreadId);

                    Monitor.PulseAll(map);
                }
            }
        };

        private readonly Action<object> removeAction = (object obj) =>
        {
            var map = (DictionaryLockTests)obj;
            for (var i = 0; i < count; i++)
            {
                lock (map)
                {
                    while (SplayHeap<string>.IsEmpty(map.set))
                    {
                        //Console.WriteLine("=== Task={0}, obj={1}, Thread={2}",
                        //  Task.CurrentId, "Wait",
                        //  Thread.CurrentThread.ManagedThreadId);

                        Monitor.Wait(map);
                    }

                    var word = SplayHeap<string>.FindMin(map.set);
                    map.set = SplayHeap<string>.DeleteMin(map.set);

                    //Console.WriteLine("<-- Task={0}, obj={1}, Thread={2}",
                    //  Task.CurrentId, word,
                    //  Thread.CurrentThread.ManagedThreadId);
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