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

namespace FunProgTests.ephemeral;

public class MultiInterlockMapTests : DictionaryTests
{
    private volatile RedBlackSet<string>.Tree _set = RedBlackSet<string>.EmptyTree;
    private readonly ITestOutputHelper _testOutputHelper;

    public MultiInterlockMapTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

        _testOutputHelper.WriteLine("Write Task={0}, Thread={1} : {2} average",
            Task.CurrentId ?? -1, Environment.CurrentManagedThreadId, 2.0 * Count / total);
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

        _testOutputHelper.WriteLine("Read Task={0}, Thread={1} : {2} words found",
            Task.CurrentId ?? -1, Environment.CurrentManagedThreadId, hits);
    }

    [Fact]
    public async Task Test1()
    {
        var taskList = new ConcurrentBag<Task>();
        for (var i = 0; i < Threads; i += 3)
        {
            taskList.Add(Task.Factory.StartNew(_ => WriteAction(), this, TestContext.Current.CancellationToken));
            taskList.Add(Task.Factory.StartNew(_ => ReadAction(), this, TestContext.Current.CancellationToken));
            taskList.Add(Task.Factory.StartNew(_ => ReadAction(), this, TestContext.Current.CancellationToken));
        }

        await Task.WhenAll(taskList.ToArray());
        _testOutputHelper.WriteLine("Done....");
    }
}