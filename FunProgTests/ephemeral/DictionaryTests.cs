// Project Console Application 0.1
// Copyright © 2014-2014. All Rights Reserved.
// 
// SUBSYSTEM:	FunPrograming
// FILE:		DictionaryTests.cs
// AUTHOR:		Greg Eakin
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
    public class DictionaryTests
    {
        private readonly Random random = new Random();

        private volatile SplayHeap<string>.Heap set = SplayHeap<string>.Empty;

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
            for (var i = 0; i < 300; i++)
            {
                lock (map)
                {
                    var word = map.NextWord();
                    map.set = SplayHeap<string>.Insert(word, map.set);

                    //Console.WriteLine("--> Task={0}, obj={1}, Thread={2}",
                    //                  Task.CurrentId, word,
                    //                  Thread.CurrentThread.ManagedThreadId);

                    Monitor.Pulse(map);
                }
            }
        };

        private readonly Action<object> readAction = (object obj) =>
        {
            var map = (DictionaryTests)obj;
            for (var i = 0; i < 300; i++)
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

            var taskList = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                taskList.Add(Task.Factory.StartNew(writeAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
            }
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Done....");
        }
    }
}