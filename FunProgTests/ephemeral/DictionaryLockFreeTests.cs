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
    public class DictionaryLockFreeTests
    {
        private const int Threads = 10;
        private const int Count = 300;

        private readonly Random random = new Random();
        private readonly EventWaitHandle event1 = new AutoResetEvent(false);

        private SplayHeap<string>.Heap heap = SplayHeap<string>.Empty;
        private int retryCount;

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

        public static void InsertHeap(EventWaitHandle event1, string word, ref SplayHeap<string>.Heap heap)
        {
            SplayHeap<string>.Heap comparand;
            SplayHeap<string>.Heap before;
            do
            {
                comparand = heap;
                var value = SplayHeap<string>.Insert(word, comparand);
                before = Interlocked.CompareExchange(ref heap, value, comparand);
            }
            while (before != comparand);

            // if (SplayHeap<string>.IsEmpty(comparand))
                event1.Set();
        }

        private readonly Action<object> writeAction = obj =>
        {
            var map = (DictionaryLockFreeTests)obj;
            for (var i = 0; i < Count; i++)
            {
                var word = map.NextWord();
                InsertHeap(map.event1, word, ref map.heap);
                //Console.WriteLine("--> Task={0}, obj={1}, Thread={2}",
                //                  Task.CurrentId, word, Thread.CurrentThread.ManagedThreadId);
            }
        };

        public static string DeleteHeap(EventWaitHandle event1, ref SplayHeap<string>.Heap heap, ref int retryCount)
        {
            string word;
            SplayHeap<string>.Heap comparand;
            SplayHeap<string>.Heap before;
            do
            {
                comparand = heap;
                while (SplayHeap<string>.IsEmpty(comparand))
                {
                    //Console.WriteLine("=== Task={0}, obj={1}, Thread={2}",
                    //  Task.CurrentId, "Wait", Thread.CurrentThread.ManagedThreadId);
                    var b = event1.WaitOne();
                    comparand = heap;
                }

                word = SplayHeap<string>.FindMin(comparand);
                var value = SplayHeap<string>.DeleteMin(comparand);
                before = Interlocked.CompareExchange(ref heap, value, comparand);
                if (before != comparand)
                    Interlocked.Increment(ref retryCount);
            }
            while (before != comparand);

            if (!SplayHeap<string>.IsEmpty(comparand))
                event1.Set();

            return word;
        }

        private readonly Action<object> readAction = obj =>
        {
            var map = (DictionaryLockFreeTests)obj;
            for (var i = 0; i < Count; i++)
            {
                var word = DeleteHeap(map.event1, ref map.heap, ref map.retryCount);
                //Console.WriteLine("<-- Task={0}, obj={1}, Thread={2}",
                //  Task.CurrentId, word, Thread.CurrentThread.ManagedThreadId);
            }
        };

        [TestMethod]
        public void Test1()
        {
            var map = new DictionaryLockFreeTests();
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
            Console.WriteLine("Done.... retryCount = {0}, time = {1} ms", map.retryCount, watch.ElapsedMilliseconds);
        }
    }
}