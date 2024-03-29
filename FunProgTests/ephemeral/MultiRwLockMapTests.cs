﻿// Fun Programming Data Structures 1.0
// 
// Copyright © 2014 Greg Eakin. 
//
// Greg Eakin <greg@eakin.dev>
//
// All Rights Reserved.
//

using FunProgLib.tree;

namespace FunProgTests.ephemeral;

public class MultiRwLockMapTests : DictionaryTests, IDisposable
{
    private readonly ReaderWriterLockSlim _lockObject = new ReaderWriterLockSlim();
    private RedBlackSet<string>.Tree _set = RedBlackSet<string>.EmptyTree;

    private void WriteAction()
    {
        for (var i = 0; i < 2 * Count; i++)
        {
            var word = NextWord(1);
            _lockObject.EnterWriteLock();
            try
            {
                _set = RedBlackSet<string>.Insert(word, _set);
            }
            finally
            {
                _lockObject.ExitWriteLock();
            }
        }
    }

    private void ReadAction()
    {
        var hits = 0;
        for (var i = 0; i < Count; i++)
        {
            var word = NextWord(1);
            _lockObject.EnterReadLock();
            try
            {
                if (RedBlackSet<string>.Member(word, _set)) hits++;
            }
            finally
            {
                _lockObject.ExitReadLock();
            }
        }

        Console.WriteLine("Task={0}, Thread={1} : {2} words found",
            Task.CurrentId, Environment.CurrentManagedThreadId, hits);
    }

    [Fact]
    public async Task Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 3)
        {
            taskList.Add(Task.Factory.StartNew(map => WriteAction(), this));
            taskList.Add(Task.Factory.StartNew(map => ReadAction(), this));
            taskList.Add(Task.Factory.StartNew(map => ReadAction(), this));
        }
        await Task.WhenAll(taskList.ToArray());
        Console.WriteLine("Done....");
    }

    public void Dispose()
    {
        _lockObject.Dispose();
    }
}