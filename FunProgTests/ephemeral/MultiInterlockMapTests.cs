// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FunProgLib.tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class MultiInterlockMapTests : DictionaryTests
    {
        private volatile RedBlackSet<string>.Tree _set = RedBlackSet<string>.EmptyTree;

        private void WriteAction()
        {
            var total = 0;
            for (var i = 0; i < 2 * Count; i++)
            {
                var word = NextWord(1);
                while (true)
                {
                    total++;
                    var localCopy = _set;
                    Thread.MemoryBarrier();
                    var newSet = RedBlackSet<string>.Insert(word, localCopy);
                    var oldSet = Interlocked.CompareExchange(ref _set, newSet, localCopy);
                    if (ReferenceEquals(localCopy, oldSet))
                        break;
                }
            }

            Console.WriteLine("Write Task={0}, Thread={1} : {2} average",
                Task.CurrentId, Thread.CurrentThread.ManagedThreadId, 2.0 * Count / total);
        }

        private void ReadAction()
        {
            var hits = 0;
            for (var i = 0; i < Count; i++)
            {
                var word = NextWord(1);
                if (RedBlackSet<string>.Member(word, _set))
                    hits++;
            }

            Console.WriteLine("Read Task={0}, Thread={1} : {2} words found",
                Task.CurrentId, Thread.CurrentThread.ManagedThreadId, hits);
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new ConcurrentBag<Task>();
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