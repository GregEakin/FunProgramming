// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class DictionaryTests
    {
        private const int Threads = 10;
        private const int Count = 300;

        private readonly Random random = new Random();

        private SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

        private string NextWord()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var x = random.Next(32, 126);
                stringBuilder.Append((char)x);
            }
            return stringBuilder.ToString();
        }

        private readonly Action<object> writeAction = (object obj) =>
        {
            var map = (DictionaryTests)obj;
            for (var i = 0; i < Count; i++)
            {
                var word = map.NextWord();
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

        private readonly Action<object> readAction = (object obj) =>
        {
            var map = (DictionaryTests)obj;
            for (var i = 0; i < Count; i++)
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
            var map = new DictionaryTests();
            var watch = new Stopwatch();
            watch.Start();

            var taskList = new List<Task>();
            for (var i = 0; i < Threads; i++)
            {
                taskList.Add(Task.Factory.StartNew(writeAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
            }
            Task.WaitAll(taskList.ToArray());

            watch.Stop();
            Console.WriteLine("Done.... time = {0} ms", watch.ElapsedMilliseconds);
        }
    }
}