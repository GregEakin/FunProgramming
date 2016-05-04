// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using FunProgLib.tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class MultiReaderMapTests : DictionaryTests
    {
        private readonly object lockObject = new object();
        private volatile RedBlackSet<string>.Tree set = RedBlackSet<string>.EmptyTree;

        private readonly Action<object> writeAction = obj =>
        {
            var map = (MultiReaderMapTests)obj;
            for (var i = 0; i < 2 * count; i++)
            {
                var word = map.NextWord(1);
                lock (map.lockObject)
                {
                    map.set = RedBlackSet<string>.Insert(word, map.set);
                }
            }
        };

        private readonly Action<object> readAction = obj =>
        {
            var map = (MultiReaderMapTests)obj;
            var hits = 0;
            for (var i = 0; i < count; i++)
            {
                var word = map.NextWord(1);
                if (RedBlackSet<string>.Member(word, map.set)) hits++;
            }

            Console.WriteLine("Task={0}, Thread={1} : {2} words found",
                            Task.CurrentId, Thread.CurrentThread.ManagedThreadId, hits);
        };

        [TestMethod]
        public void Test1()
        {
            var map = new MultiReaderMapTests();

            var taskList = new List<Task>();
            for (var i = 0; i < threads; i += 3)
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