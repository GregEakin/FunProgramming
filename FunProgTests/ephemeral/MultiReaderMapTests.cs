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

        private volatile RedBlackSet<string>.Tree set = RedBlackSet<string>.EmptyTree;

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
            var map = (MultiReaderMapTests)obj;
            for (var i = 0; i < 200; i++)
            {
                var word = map.NextWord();
                lock (map)
                {
                    Thread.Yield();
                    map.set = RedBlackSet<string>.Insert(word, map.set);
                }
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

            var taskList = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                taskList.Add(Task.Factory.StartNew(writeAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
                taskList.Add(Task.Factory.StartNew(readAction, map));
            }
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Done....");
        }
    }
}