// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@gdbtech.info>
//
// All Rights Reserved.
//

using System;
using FunProgLib.heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunProgTests.ephemeral
{
    [TestClass]
    public class DictionaryRwLockTests : DictionaryTests, IDisposable
    {
        private readonly ReaderWriterLockSlim _lockObject = new ReaderWriterLockSlim();
        private SplayHeap<string>.Heap _set = SplayHeap<string>.Empty;

        private void InsertAction()
        {
            for (var i = 0; i < Count; i++)
            {
                var word = NextWord(10);
                _lockObject.EnterWriteLock();
                try
                {
                    _set = SplayHeap<string>.Insert(word, _set);
                }
                finally
                {
                    _lockObject.ExitWriteLock();
                }
            }
        }

        private void RemoveAction()
        {
            for (var i = 0; i < Count; i++)
            {
                while (true)
                {
                    _lockObject.EnterWriteLock();
                    try
                    {
                        if (!SplayHeap<string>.IsEmpty(_set))
                        {
                            var word = SplayHeap<string>.FindMin(_set);
                            _set = SplayHeap<string>.DeleteMin(_set);
                            break;
                        }
                    }
                    finally
                    {
                        _lockObject.ExitWriteLock();
                    }

                    Thread.Yield();
                }
            }
        }

        [TestMethod]
        public void Test1()
        {
            var taskList = new List<Task>();
            for (var i = 0; i < Threads; i += 2)
            {
                taskList.Add(Task.Factory.StartNew(map => InsertAction(), this));
                taskList.Add(Task.Factory.StartNew(map => RemoveAction(), this));
            }

            Task.WaitAll(taskList.ToArray());
        }

        public void Dispose()
        {
            _lockObject.Dispose();
        }
    }
}