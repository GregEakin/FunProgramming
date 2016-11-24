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
    public class MultiInterlockMapTests : DictionaryTests
    {
        private RedBlackSet<string>.Tree _set = RedBlackSet<string>.EmptyTree;

        private void WriteAction()
        {
            for (var i = 0; i < 2 * Count; i++)
            {
                var word = NextWord(1);
                while (true)
                {
                    var workingSet = _set;
                    Thread.MemoryBarrier();
                    var newSet = RedBlackSet<string>.Insert(word, workingSet);
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, workingSet);
                    if (ReferenceEquals(oldSet, workingSet))
                        break;
                }
            }
        }

        private void ReadAction()
        {
            var hits = 0;
            for (var i = 0; i < Count; i++)
            {
                var word = NextWord(1);
                var workingSet = _set;
                Thread.MemoryBarrier();
                if (workingSet != RedBlackSet<string>.EmptyTree)
                    continue;
                if (RedBlackSet<string>.Member(word, workingSet))
                    hits++;
            }

            Console.WriteLine("Task={0}, Thread={1} : {2} words found",
                            Task.CurrentId, Thread.CurrentThread.ManagedThreadId, hits);
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < Threads; i += 3)
            {
                taskList.Add(Task.Factory.StartNew(map => WriteAction(), this));
                taskList.Add(Task.Factory.StartNew(map => ReadAction(), this));
                taskList.Add(Task.Factory.StartNew(map => ReadAction(), this));
            }
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Done....");
        }
    }
}