// Copyright 2014 Gregory Eakin <greg@eakin.dev>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using FunProgLib.tree;
using Xunit.Abstractions;

namespace FunProgTests.ephemeral;

public class MultiRwLockMapTests : DictionaryTests, IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ReaderWriterLockSlim _lockObject = new ReaderWriterLockSlim();
    private RedBlackSet<string>.Tree _set = RedBlackSet<string>.EmptyTree;

    public MultiRwLockMapTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    private void ReadAction(ITestOutputHelper testOutputHelper)
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

        testOutputHelper.WriteLine("Task={0}, Thread={1} : {2} words found",
            Task.CurrentId, Environment.CurrentManagedThreadId, hits);
    }

    [Fact]
    public async Task Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 3)
        {
            taskList.Add(Task.Factory.StartNew(map => WriteAction(), this));
            taskList.Add(Task.Factory.StartNew(map => ReadAction(_testOutputHelper), this));
            taskList.Add(Task.Factory.StartNew(map => ReadAction(_testOutputHelper), this));
        }
        await Task.WhenAll(taskList.ToArray());
        _testOutputHelper.WriteLine("Done....");
    }

    public void Dispose()
    {
        _lockObject.Dispose();
    }
}