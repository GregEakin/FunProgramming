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
using FunProgLib.tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class MultiReaderMapTests
    {
        private readonly Random random = new Random();

        private RedBlackSet<string>.Tree set = RedBlackSet<string>.EmptyTree;
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

        public static void UpdateSet(string word, ref RedBlackSet<string>.Tree set, ref int retryCount)
        {
            RedBlackSet<string>.Tree comparand;
            RedBlackSet<string>.Tree before;
            do
            {
                comparand = set;
                var value = RedBlackSet<string>.Insert(word, set);
                before = Interlocked.CompareExchange(ref set, value, comparand);

                if (before != comparand)
                    Interlocked.Increment(ref retryCount);
            }
            while (before != comparand);
        }

        private readonly Action<object> writeAction = (object obj) =>
        {
            var map = (MultiReaderMapTests)obj;
            for (var i = 0; i < 200; i++)
            {
                var word = map.NextWord();
                Thread.Yield();
                UpdateSet(word, ref map.set, ref map.retryCount);
            }
            //Console.WriteLine("Task={0}, obj={1}, Thread={2} : 100 words added",
            //                  Task.CurrentId, obj.ToString(),
            //                  Thread.CurrentThread.ManagedThreadId);
        };

        private readonly Action<object> readAction = (object obj) =>
        {
            var map = (MultiReaderMapTests)obj;
            var count = 0;
            for (var i = 0; i < 100; i++)
            {
                var word = map.NextWord();
                if (RedBlackSet<string>.Member(word, map.set)) count++;
            }
            if (count > 0)
                Console.WriteLine("Task={0}, obj={1}, Thread={2} : {3} words found",
                              Task.CurrentId, obj.ToString(),
                              Thread.CurrentThread.ManagedThreadId, count);
        };

        [TestMethod]
        public void Test1()
        {
            var map = new MultiReaderMapTests();
            var watch = new Stopwatch();
            watch.Start();

            var taskList = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                taskList.Add(Task.Factory.StartNew(writeAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
            }
            Task.WaitAll(taskList.ToArray());

            watch.Stop();
            Console.WriteLine("Done.... retryCount = {0}, time = {1} ms", map.retryCount, watch.ElapsedMilliseconds);
        }
    }
}