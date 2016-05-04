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

        private void writeAction()
        {
            for (var i = 0; i < 2 * count; i++)
            {
                var word = NextWord(1);
                lock (lockObject)
                {
                    set = RedBlackSet<string>.Insert(word, set);
                }
            }
        }

        private void readAction()
        {
            var hits = 0;
            for (var i = 0; i < count; i++)
            {
                var word = NextWord(1);
                if (RedBlackSet<string>.Member(word, set)) hits++;
            }

            Console.WriteLine("Task={0}, Thread={1} : {2} words found",
                            Task.CurrentId, Thread.CurrentThread.ManagedThreadId, hits);
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < threads; i += 3)
            {
                taskList.Add(Task.Factory.StartNew(map => writeAction(), this));
                taskList.Add(Task.Factory.StartNew(map => readAction(), this));
                taskList.Add(Task.Factory.StartNew(map => readAction(), this));
            }
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Done....");
        }
    }
}